#region Apache License Version 2.0
/*----------------------------------------------------------------

Copyright 2026 Jeffrey Su & Suzhou Senparc Network Technology Co.,Ltd.

Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file
except in compliance with the License. You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software distributed under the
License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
either express or implied. See the License for the specific language governing permissions
and limitations under the License.

Detail: https://github.com/JeffreySu/WeiXinMPSDK/blob/master/license.md

----------------------------------------------------------------*/
#endregion Apache License Version 2.0

/*----------------------------------------------------------------
    Copyright (C) 2026 Senparc
 
    文件名：TenPayV3Info.cs
    文件功能描述：微信支付 V3 商户配置、API 实例与平台公钥管理

    
    创建标识：Senparc - 20140810

    修改标识：Senparc - 20210822
    修改描述：修改BasePayApis 此类型不再为静态类 使用ISenparcWeixinSettingForTenpayV3初始化实例

    修改标识：Senparc - 20210829
    修改描述：添加 V3 中新增加的属性

    修改标识：Senparc - 20260118
    修改描述：V3 中新增加 CertType 属性

    修改标识：Senparc - 20260718
    修改描述：v2.4.1 增加平台公钥缓存过期与未命中并发刷新机制

    修改标识：Senparc - 20260718
    修改描述：v2.5.0 为支付公钥刷新增加取消传播与并发控制

----------------------------------------------------------------*/

using Senparc.CO2NET.Extensions;
using Senparc.CO2NET.Trace;
using Senparc.Weixin.Entities;
using Senparc.Weixin.TenPayV3.Apis;
using Senparc.Weixin.TenPayV3.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Senparc.Weixin.TenPayV3
{
    /// <summary>
    /// 微信支付基础信息储存类
    /// </summary>
    public class TenPayV3Info
    {
        private static readonly TimeSpan PublicKeyCacheDuration = TimeSpan.FromHours(1);
        private readonly SemaphoreSlim _publicKeyRefreshLock = new SemaphoreSlim(1, 1);
        private PublicKeyCollection _publicKeys;
        private long _publicKeysExpiresAtUtcTicks;

        /// <summary>
        /// 第三方用户唯一凭证appid
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// 第三方用户唯一凭证密钥，即appsecret
        /// </summary>
        public string AppSecret { get; set; }
        /// <summary>
        /// 商户ID
        /// </summary>
        public string MchId { get; set; }
        /// <summary>
        /// 商户支付密钥Key。登录微信商户后台，进入栏目【账户设置】【密码安全】【API 安全】【API 密钥】
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 微信支付证书位置（物理路径），在 .NET Core 下执行 TenPayV3InfoCollection.Register() 方法会为 HttpClient 自动添加证书
        /// </summary>
        public string CertPath { get; set; }
        /// <summary>
        /// 微信支付证书密码
        /// </summary>
        public string CertSecret { get; set; }
        /// <summary>
        /// 支付完成后的回调处理页面
        /// </summary>
        public string TenPayV3Notify { get; set; } // = "http://localhost/payNotifyUrl.aspx";
        /// <summary>
        /// 小程序支付完成后的回调处理页面
        /// </summary>
        public string TenPayV3_WxOpenNotify { get; set; }

        /// <summary>
        /// 服务商模式下，特约商户的开发配置中的AppId
        /// </summary>
        public string Sub_AppId { get; set; }
        /// <summary>
        /// 服务商模式下，特约商户的开发配置中的AppSecret
        /// </summary>
        public string Sub_AppSecret { get; set; }
        /// <summary>
        /// 服务商模式下，特约商户的商户Id
        /// </summary>
        public string Sub_MchId { get; set; }


        #region 新版微信支付 V3 新增

        /// <summary>
        /// 微信支付（V3）证书私钥
        /// <para>获取途径：apiclient_key.pem</para>
        /// </summary>
        public string TenPayV3_PrivateKey { get; set; }
        /// <summary>
        /// 微信支付（V3）证书序列号
        /// <para>查看地址：https://pay.weixin.qq.com/index.php/core/cert/api_cert#/api-cert-manage</para>
        /// </summary>
        public string TenPayV3_SerialNumber { get; set; }
        /// <summary>
        /// APIv3 密钥。在微信支付后台设置：https://pay.weixin.qq.com/index.php/core/cert/api_cert#/
        /// </summary>
        public string TenPayV3_APIv3Key { get; set; }

        /// <summary>
        /// 证书类型
        /// </summary>
        public CertType? TenPayV3_CertType { get; set; }

        #endregion


        /// <summary>
        /// 普通服务商 微信支付 V3 参数 构造函数
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <param name="mchId"></param>
        /// <param name="key"></param>
        /// <param name="certPath"></param>
        /// <param name="certSecret"></param>
        /// <param name="tenPayV3Notify"></param>
        /// <param name="tenPayV3WxOpenNotify"></param>
        /// <param name="privateKey"></param>
        /// <param name="serialNumber"></param>
        /// <param name="apiV3Key"></param>
        public TenPayV3Info(string appId, string appSecret, string mchId, string key, string certPath, string certSecret,
            string tenPayV3Notify, string tenPayV3WxOpenNotify, string privateKey, string serialNumber, string apiV3Key)
            : this(appId, appSecret, mchId, key, certPath, certSecret, tenPayV3Notify, tenPayV3WxOpenNotify,
                  privateKey, serialNumber, apiV3Key, CertType.RSA)
        {
        }

        /// <summary>
        /// 普通服务商微信支付 V3 参数构造函数，并显式指定证书类型。
        /// </summary>
        public TenPayV3Info(string appId, string appSecret, string mchId, string key, string certPath, string certSecret, 
            string tenPayV3Notify, string tenPayV3WxOpenNotify, string privateKey, string serialNumber, string apiV3Key, CertType certType)
            : this(appId, appSecret, mchId, key, certPath, certSecret, "", "", "", tenPayV3Notify, 
                  tenPayV3WxOpenNotify, privateKey, serialNumber, apiV3Key, certType)
        {

        }
        /// <summary>
        /// 服务商户 微信支付 V3 参数 构造函数
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <param name="mchId"></param>
        /// <param name="key"></param>
        /// <param name="certPath"></param>
        /// <param name="certSecret"></param>
        /// <param name="subAppId"></param>
        /// <param name="subAppSecret"></param>
        /// <param name="subMchId"></param>
        /// <param name="tenPayV3Notify"></param>
        /// <param name="tenPayV3WxOpenNotify"></param>
        /// <param name="privateKey"></param>
        /// <param name="serialNumber"></param>
        /// <param name="apiV3Key"></param>
        public TenPayV3Info(string appId, string appSecret, string mchId, string key, string certPath, string certSecret, string subAppId,
            string subAppSecret, string subMchId, string tenPayV3Notify, string tenPayV3WxOpenNotify, string privateKey, string serialNumber,
            string apiV3Key)
            : this(appId, appSecret, mchId, key, certPath, certSecret, subAppId, subAppSecret, subMchId,
                  tenPayV3Notify, tenPayV3WxOpenNotify, privateKey, serialNumber, apiV3Key, CertType.RSA)
        {
        }

        /// <summary>
        /// 服务商微信支付 V3 参数构造函数，并显式指定证书类型。
        /// </summary>
        /// <param name="certType"></param>
        public TenPayV3Info(string appId, string appSecret, string mchId, string key, string certPath, string certSecret, string subAppId, 
            string subAppSecret, string subMchId, string tenPayV3Notify, string tenPayV3WxOpenNotify, string privateKey, string serialNumber, 
            string apiV3Key, CertType? certType)
        {
            AppId = appId;
            AppSecret = appSecret;
            MchId = mchId;
            Key = key;
            CertPath = certPath;
            CertSecret = certSecret;
            TenPayV3Notify = tenPayV3Notify;
            TenPayV3_WxOpenNotify = tenPayV3WxOpenNotify;
            Sub_AppId = subAppId;
            Sub_AppSecret = subAppSecret;
            Sub_MchId = subMchId;
            TenPayV3_PrivateKey = privateKey;
            TenPayV3_SerialNumber = serialNumber;
            TenPayV3_APIv3Key = apiV3Key;
            TenPayV3_CertType = certType;

        }

        /// <summary>
        /// 微信支付 V3 参数 构造函数
        /// </summary>
        /// <param name="senparcWeixinSettingForTenpayV3">已经填充过微信支付参数的 SenparcWeixinSetting 对象</param>
        public TenPayV3Info(ISenparcWeixinSettingForTenpayV3 senparcWeixinSettingForTenpayV3 = null)
            : this(senparcWeixinSettingForTenpayV3.TenPayV3_AppId,
                  senparcWeixinSettingForTenpayV3.TenPayV3_AppSecret,
                  senparcWeixinSettingForTenpayV3.TenPayV3_MchId,
                  senparcWeixinSettingForTenpayV3.TenPayV3_Key,
                  senparcWeixinSettingForTenpayV3.TenPayV3_CertPath,
                  senparcWeixinSettingForTenpayV3.TenPayV3_CertSecret,
                  senparcWeixinSettingForTenpayV3.TenPayV3_SubAppId,
                  senparcWeixinSettingForTenpayV3.TenPayV3_SubAppSecret,
                  senparcWeixinSettingForTenpayV3.TenPayV3_SubMchId,
                  senparcWeixinSettingForTenpayV3.TenPayV3_TenpayNotify,
                  senparcWeixinSettingForTenpayV3.TenPayV3_WxOpenTenpayNotify,
                  senparcWeixinSettingForTenpayV3.TenPayV3_PrivateKey,
                  senparcWeixinSettingForTenpayV3.TenPayV3_SerialNumber,
                  senparcWeixinSettingForTenpayV3.TenPayV3_APIv3Key,
                  senparcWeixinSettingForTenpayV3.EncryptionType
                  )
        {
            //_tenpayV3Setting = senparcWeixinSettingForTenpayV3 ?? Senparc.Weixin.Config.SenparcWeixinSetting.TenpayV3Setting;

        }

        /// <summary>
        /// 获取当前支付账号下所有公钥信息
        /// </summary>
        public Task<PublicKeyCollection> GetPublicKeysAsync(ISenparcWeixinSettingForTenpayV3 tenpayV3Setting)
        {
            return GetPublicKeysAsync(tenpayV3Setting, CancellationToken.None);
        }

        public async Task<PublicKeyCollection> GetPublicKeysAsync(ISenparcWeixinSettingForTenpayV3 tenpayV3Setting, CancellationToken cancellationToken)
        {
            var cachedKeys = Volatile.Read(ref _publicKeys);
            if (IsPublicKeyCacheFresh(cachedKeys))
            {
                return cachedKeys;
            }

            await _publicKeyRefreshLock.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                cachedKeys = Volatile.Read(ref _publicKeys);
                if (IsPublicKeyCacheFresh(cachedKeys))
                {
                    return cachedKeys;
                }

                return await FetchAndCachePublicKeysAsync(tenpayV3Setting, cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                _publicKeyRefreshLock.Release();
            }
        }

        private bool IsPublicKeyCacheFresh(PublicKeyCollection keys)
        {
            return keys != null && DateTimeOffset.UtcNow.UtcDateTime.Ticks < Interlocked.Read(ref _publicKeysExpiresAtUtcTicks);
        }

        private async Task<PublicKeyCollection> FetchAndCachePublicKeysAsync(ISenparcWeixinSettingForTenpayV3 tenpayV3Setting, CancellationToken cancellationToken)
        {
            var basePayApis = new BasePayApis(tenpayV3Setting);
            var keys = await basePayApis.GetPublicKeysAsync(cancellationToken).ConfigureAwait(false) ?? new PublicKeyCollection();

            Volatile.Write(ref _publicKeys, keys);
            Interlocked.Exchange(
                ref _publicKeysExpiresAtUtcTicks,
                DateTimeOffset.UtcNow.Add(PublicKeyCacheDuration).UtcDateTime.Ticks);

            return keys;
        }

        private async Task<PublicKeyCollection> RefreshPublicKeysAfterMissAsync(
            PublicKeyCollection observedKeys,
            ISenparcWeixinSettingForTenpayV3 tenpayV3Setting,
            CancellationToken cancellationToken)
        {
            await _publicKeyRefreshLock.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                // 如果等待锁期间已有请求完成刷新，直接复用该结果，避免同一序列号未命中时重复请求。
                var currentKeys = Volatile.Read(ref _publicKeys);
                if (!ReferenceEquals(observedKeys, currentKeys))
                {
                    return currentKeys;
                }

                return await FetchAndCachePublicKeysAsync(tenpayV3Setting, cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                _publicKeyRefreshLock.Release();
            }
        }

        /// <summary>
        /// 获取单个公钥
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public Task<string> GetPublicKeyAsync(string serialNumber, ISenparcWeixinSettingForTenpayV3 tenpayV3Setting)
        {
            return GetPublicKeyAsync(serialNumber, tenpayV3Setting, CancellationToken.None);
        }

        public async Task<string> GetPublicKeyAsync(string serialNumber, ISenparcWeixinSettingForTenpayV3 tenpayV3Setting, CancellationToken cancellationToken)
        {
            var keys = await GetPublicKeysAsync(tenpayV3Setting, cancellationToken).ConfigureAwait(false);
            if (keys.TryGetValue(serialNumber, out string publicKey))
            {
                return publicKey;
            }

            // 平台证书/公钥可能刚完成轮换；未命中时立即刷新一次，而不是等待常规缓存过期。
            keys = await RefreshPublicKeysAfterMissAsync(keys, tenpayV3Setting, cancellationToken).ConfigureAwait(false);
            if (keys != null && keys.TryGetValue(serialNumber, out publicKey))
            {
                return publicKey;
            }

            // 日志仅记录定位所需标识，禁止序列化整个对象（其中包含私钥、APIv3 Key 等敏感信息）。
            SenparcTrace.BaseExceptionLog(new TenpaySecurityException(
                $"公钥序列号不存在！serialNumber:{serialNumber},MchId:{MchId},SubMchId:{Sub_MchId}"));
            throw new TenpaySecurityException("公钥序列号不存在！请查看日志！", true);
        }
    }
}
