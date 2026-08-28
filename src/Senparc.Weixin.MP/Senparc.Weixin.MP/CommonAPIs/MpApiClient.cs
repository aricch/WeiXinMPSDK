/*----------------------------------------------------------------
    Copyright (C) 2026 Senparc

    文件名：MpApiClient.cs
    文件功能描述：微信公众号可注入实例 API 客户端


    创建标识：Senparc - 20260717

    修改标识：Senparc - 20260718
    修改描述：v16.25.0 新增支持取消令牌的可注入公众号实例客户端

----------------------------------------------------------------*/

using System;
using System.Threading;
using System.Threading.Tasks;
using Senparc.Weixin.CommonAPIs.ApiHandlerWapper;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP.Containers;

namespace Senparc.Weixin.MP
{
    /// <summary>
    /// 公众号实例 API 客户端。适合由业务 DI 容器按账号创建，避免修改静态 Service Locator 委托。
    /// </summary>
    public sealed class MpApiClient
    {
        public MpApiClient(string appId)
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
                PlatformType.MP,
                () => AppId,
                AccessTokenContainer.CheckRegistered,
                AccessTokenContainer.GetAccessTokenResult,
                ApiHandlerWapper.InvalidCredentialValues,
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
                PlatformType.MP,
                () => Task.FromResult(AppId),
                AccessTokenContainer.CheckRegisteredAsync,
                AccessTokenContainer.GetAccessTokenResultAsync,
                ApiHandlerWapper.InvalidCredentialValues,
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
