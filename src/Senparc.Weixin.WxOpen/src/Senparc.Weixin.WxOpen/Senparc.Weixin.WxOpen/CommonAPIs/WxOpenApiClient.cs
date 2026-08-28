/*----------------------------------------------------------------
    Copyright (C) 2026 Senparc

    文件名：WxOpenApiClient.cs
    文件功能描述：微信小程序可注入实例 API 客户端


    创建标识：Senparc - 20260717

    修改标识：Senparc - 20260718
    修改描述：v3.28.0 新增支持取消令牌的可注入小程序实例客户端

----------------------------------------------------------------*/

using System;
using System.Threading;
using System.Threading.Tasks;
using Senparc.Weixin.CommonAPIs.ApiHandlerWapper;
using Senparc.Weixin.Entities;
using Senparc.Weixin.WxOpen.Containers;

namespace Senparc.Weixin.WxOpen
{
    /// <summary>
    /// 微信小程序实例 API 客户端。适合由业务 DI 容器按应用创建。
    /// </summary>
    public sealed class WxOpenApiClient
    {
        private static readonly int[] InvalidCredentialValues =
            { (int)ReturnCode.获取access_token时AppSecret错误或者access_token无效 };

        public WxOpenApiClient(string appId)
        {
            AppId = !string.IsNullOrWhiteSpace(appId)
                ? appId
                : throw new ArgumentException("AppId 不能为空。", nameof(appId));
        }

        public string AppId { get; }

        public T Execute<T>(Func<string, T> operation, bool retryInvalidCredential = true)
            where T : WxJsonResult, new()
        {
            if (operation == null)
            {
                throw new ArgumentNullException(nameof(operation));
            }

            return ApiHandlerWapperBase.TryCommonApiBase(
                PlatformType.WxOpen,
                () => AppId,
                AccessTokenContainer.CheckRegistered,
                AccessTokenContainer.GetAccessTokenResult,
                InvalidCredentialValues,
                operation,
                AppId,
                retryInvalidCredential);
        }

        public Task<T> ExecuteAsync<T>(
            Func<string, CancellationToken, Task<T>> operation,
            CancellationToken cancellationToken = default,
            bool retryInvalidCredential = true)
            where T : WxJsonResult, new()
        {
            if (operation == null)
            {
                throw new ArgumentNullException(nameof(operation));
            }

            cancellationToken.ThrowIfCancellationRequested();
            return ApiHandlerWapperBase.TryCommonApiBaseAsync(
                PlatformType.WxOpen,
                () => Task.FromResult(AppId),
                AccessTokenContainer.CheckRegisteredAsync,
                AccessTokenContainer.GetAccessTokenResultAsync,
                InvalidCredentialValues,
                token =>
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    return operation(token, cancellationToken);
                },
                AppId,
                retryInvalidCredential);
        }
    }
}
