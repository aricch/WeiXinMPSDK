/*----------------------------------------------------------------
    Copyright (C) 2026 Senparc

    文件名：WorkApiClient.cs
    文件功能描述：企业微信可注入实例 API 客户端


    创建标识：Senparc - 20260717

    修改标识：Senparc - 20260718
    修改描述：v3.32.0 新增支持取消令牌的可注入企业微信实例客户端

----------------------------------------------------------------*/

using System;
using System.Threading;
using System.Threading.Tasks;
using Senparc.Weixin.CommonAPIs.ApiHandlerWapper;
using Senparc.Weixin.Entities;
using Senparc.Weixin.Work.CommonAPIs;
using Senparc.Weixin.Work.Containers;

namespace Senparc.Weixin.Work
{
    /// <summary>
    /// 企业微信实例 API 客户端。适合由业务 DI 容器按应用创建。
    /// </summary>
    public sealed class WorkApiClient
    {
        public WorkApiClient(string corpId, string corpSecret)
            : this(AccessTokenContainer.BuildingKey(corpId, corpSecret))
        {
        }

        public WorkApiClient(string appKey)
        {
            AppKey = !string.IsNullOrWhiteSpace(appKey)
                ? appKey
                : throw new ArgumentException("AppKey 不能为空。", nameof(appKey));
        }

        public string AppKey { get; }

        public T Execute<T>(Func<string, T> operation, bool retryInvalidCredential = true)
            where T : WorkJsonResult, new()
        {
            if (operation == null)
            {
                throw new ArgumentNullException(nameof(operation));
            }

            return ApiHandlerWapperBase.TryCommonApiBase(
                PlatformType.Work,
                () => AppKey,
                AccessTokenContainer.CheckRegistered,
                AccessTokenContainer.GetTokenResult,
                ApiHandlerWapper.InvalidCredentialValues,
                operation,
                AppKey,
                retryInvalidCredential);
        }

        public Task<T> ExecuteAsync<T>(
            Func<string, CancellationToken, Task<T>> operation,
            CancellationToken cancellationToken = default,
            bool retryInvalidCredential = true)
            where T : WorkJsonResult, new()
        {
            if (operation == null)
            {
                throw new ArgumentNullException(nameof(operation));
            }

            cancellationToken.ThrowIfCancellationRequested();
            return ApiHandlerWapperBase.TryCommonApiBaseAsync(
                PlatformType.Work,
                () => Task.FromResult(AppKey),
                AccessTokenContainer.CheckRegisteredAsync,
                AccessTokenContainer.GetTokenResultAsync,
                ApiHandlerWapper.InvalidCredentialValues,
                token =>
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    return operation(token, cancellationToken);
                },
                AppKey,
                retryInvalidCredential);
        }
    }
}
