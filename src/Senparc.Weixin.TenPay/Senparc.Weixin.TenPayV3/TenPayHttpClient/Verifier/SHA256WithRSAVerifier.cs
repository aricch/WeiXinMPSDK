/*----------------------------------------------------------------
    Copyright (C) 2026 Senparc

    文件名：SHA256WithRSAVerifier.cs
    文件功能描述：微信支付 SHA256WithRSA 签名验证器


    创建标识：Senparc - 20220224

    修改标识：Senparc - 20260718
    修改描述：v2.4.1 完善 RSA 签名验证资源释放与空公钥校验

----------------------------------------------------------------*/

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Senparc.Weixin.Helpers;
using Senparc.Weixin.TenPayV3.Helpers;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Senparc.Weixin.TenPayV3.TenPayHttpClient.Verifier
{
    public class SHA256WithRSAVerifier : IVerifier
    {
        public bool Verify(string wechatpayTimestamp, string wechatpayNonce, string wechatpaySignatureBase64, string content, string pubKey, bool isTenPayPubKey = false)
        {
            //验签名串
            string contentForSign = $"{wechatpayTimestamp}\n{wechatpayNonce}\n{content}\n";

            if (isTenPayPubKey)
            {
                var publicKeyParam = (RsaKeyParameters)PublicKeyFactory.CreateKey(Convert.FromBase64String(pubKey));
                var publicKeyXml = string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent></RSAKeyValue>",
                    Convert.ToBase64String(publicKeyParam.Modulus.ToByteArrayUnsigned()),
                    Convert.ToBase64String(publicKeyParam.Exponent.ToByteArrayUnsigned()));

                using (var rsa = new RSACryptoServiceProvider())
                using (var sha256 = SHA256.Create())
                {
                    SecurityHelper.RSAFromXmlString(rsa, publicKeyXml);

                    //RSAPKCS1SignatureDeformatter 对象
                    RSAPKCS1SignatureDeformatter df = new RSAPKCS1SignatureDeformatter(rsa);
                    //指定 SHA256
                    df.SetHashAlgorithm("SHA256");
                    //应答签名
                    byte[] signature = Convert.FromBase64String(wechatpaySignatureBase64);
                    //对比签名
                    byte[] compareByte = sha256.ComputeHash(Encoding.UTF8.GetBytes(contentForSign));
                    //验证签名
                    return df.VerifySignature(compareByte, signature);
                }
            }
            else
            {

                //Base64 解码 pubKey（必须已经使用 ApiSecurityHelper.GetUnwrapCertKey() 方法进行 Unwrap）
                var bs = Convert.FromBase64String(pubKey);
                //使用 X509Certificate2 证书
                using (var x509 = new X509Certificate2(bs))
                using (var key = x509.GetRSAPublicKey())
                using (var sha256 = SHA256.Create())
                {
                    if (key == null)
                    {
                        throw new CryptographicException("证书中未包含 RSA 公钥。");
                    }

                    //RSAPKCS1SignatureDeformatter 对象
                    RSAPKCS1SignatureDeformatter df = new RSAPKCS1SignatureDeformatter(key);
                    //指定 SHA256
                    df.SetHashAlgorithm("SHA256");
                    //应答签名
                    byte[] signature = Convert.FromBase64String(wechatpaySignatureBase64);
                    //对比签名
                    byte[] compareByte = sha256.ComputeHash(Encoding.UTF8.GetBytes(contentForSign));
                    //验证签名
                    return df.VerifySignature(compareByte, signature);
                }
            }
        }
    }
}
