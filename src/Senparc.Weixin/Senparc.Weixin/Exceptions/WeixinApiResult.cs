/*----------------------------------------------------------------
    Copyright (C) 2026 Senparc

    文件名：WeixinApiResult.cs
    文件功能描述：微信 API 统一错误模型与结果执行器


    创建标识：Senparc - 20260717

    修改标识：Senparc - 20260718
    修改描述：v6.24.0 新增微信 API 统一错误模型与结果执行器

----------------------------------------------------------------*/

using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Senparc.Weixin.Exceptions
{
    /// <summary>
    /// 微信 API 失败分类。取消始终通过 <see cref="OperationCanceledException"/> 传播，不转换为失败结果。
    /// </summary>
    public enum WeixinApiErrorKind
    {
        Unknown,
        Validation,
        Configuration,
        Transport,
        Http,
        Business,
        Signature
    }

    /// <summary>
    /// 跨 MP、Work、WxOpen 和 TenPay 可共同使用的错误信息。
    /// </summary>
    public sealed class WeixinApiError
    {
        public WeixinApiError(
            WeixinApiErrorKind kind,
            string message,
            string code = null,
            HttpStatusCode? statusCode = null,
            bool isTransient = false,
            Exception exception = null)
        {
            Kind = kind;
            Message = message ?? string.Empty;
            Code = code;
            StatusCode = statusCode;
            IsTransient = isTransient;
            Exception = exception;
        }

        public WeixinApiErrorKind Kind { get; }
        public string Message { get; }
        public string Code { get; }
        public HttpStatusCode? StatusCode { get; }
        public bool IsTransient { get; }
        public Exception Exception { get; }
    }

    /// <summary>
    /// 显式成功/失败 API 结果，用于不希望通过异常表达预期业务错误的新代码。
    /// </summary>
    public sealed class WeixinApiResult<T>
    {
        private WeixinApiResult(T value, WeixinApiError error)
        {
            Value = value;
            Error = error;
        }

        public bool IsSuccess => Error == null;
        public T Value { get; }
        public WeixinApiError Error { get; }

        public static WeixinApiResult<T> Success(T value) => new WeixinApiResult<T>(value, null);
        public static WeixinApiResult<T> Failure(WeixinApiError error) =>
            new WeixinApiResult<T>(default, error ?? throw new ArgumentNullException(nameof(error)));
    }

    /// <summary>
    /// 将现有异常边界适配到统一错误模型。
    /// </summary>
    public static class WeixinApiExecutor
    {
        public static async Task<WeixinApiResult<T>> ExecuteAsync<T>(
            Func<CancellationToken, Task<T>> operation,
            CancellationToken cancellationToken = default)
        {
            if (operation == null)
            {
                throw new ArgumentNullException(nameof(operation));
            }

            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                return WeixinApiResult<T>.Success(await operation(cancellationToken).ConfigureAwait(false));
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (ErrorJsonResultException ex)
            {
                return WeixinApiResult<T>.Failure(new WeixinApiError(
                    WeixinApiErrorKind.Business,
                    ex.Message,
                    ex.JsonResult?.errcode.ToString(),
                    exception: ex));
            }
            catch (ArgumentException ex)
            {
                return WeixinApiResult<T>.Failure(new WeixinApiError(
                    WeixinApiErrorKind.Validation,
                    ex.Message,
                    exception: ex));
            }
            catch (Exception ex)
            {
                return WeixinApiResult<T>.Failure(new WeixinApiError(
                    WeixinApiErrorKind.Unknown,
                    ex.Message,
                    isTransient: false,
                    exception: ex));
            }
        }
    }
}
