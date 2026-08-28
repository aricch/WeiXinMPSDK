/*----------------------------------------------------------------
    Copyright (C) 2026 Senparc

    文件名：IWeixinCredentialProvider.cs
    文件功能描述：微信凭据外部提供器接口


    创建标识：Senparc - 20260717

    修改标识：Senparc - 20260718
    修改描述：v6.24.0 新增外部微信凭据提供器契约

----------------------------------------------------------------*/

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Senparc.Weixin.Containers
{
    /// <summary>
    /// 延迟提供微信应用密钥，便于接入 Key Vault、KMS 或业务自己的密钥轮换系统。
    /// </summary>
    public interface IWeixinCredentialProvider
    {
        Task<string> GetSecretAsync(string credentialId, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// 委托形式的凭据提供器。
    /// </summary>
    public sealed class DelegateWeixinCredentialProvider : IWeixinCredentialProvider
    {
        private readonly Func<string, CancellationToken, Task<string>> _provider;

        public DelegateWeixinCredentialProvider(Func<string, CancellationToken, Task<string>> provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public Task<string> GetSecretAsync(string credentialId, CancellationToken cancellationToken = default)
        {
            return _provider(credentialId, cancellationToken);
        }
    }
}
