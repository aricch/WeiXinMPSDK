/*----------------------------------------------------------------
    Copyright (C) 2026 Senparc

    文件名：TenPayDownloadHelper.cs
    文件功能描述：微信支付 V3 流式下载与增量哈希辅助类


    创建标识：Senparc - 20260717

    修改标识：Senparc - 20260718
    修改描述：v2.5.0 新增 ResponseHeadersRead 流式下载与增量哈希

----------------------------------------------------------------*/

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

using Senparc.Weixin.Exceptions;
using System;
using System.Buffers;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace Senparc.Weixin.TenPayV3.Helpers
{
    /// <summary>
    /// 微信支付下载流工具。响应按块写入目标流，并在同一次遍历中计算摘要。
    /// </summary>
    internal static class TenPayDownloadHelper
    {
        private const int BufferSize = 81920;

        internal static async Task<bool> DownloadAndVerifyAsync(
            TenPayApiRequest request,
            string url,
            Stream destination,
            string hashType,
            string expectedHash,
            int timeOut,
            CancellationToken cancellationToken)
        {
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            using (var response = await request.GetHttpResponseMessageAsync(
                url,
                null,
                cancellationToken,
                HttpCompletionOption.ResponseHeadersRead,
                timeOut,
                ApiRequestMethod.GET).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    throw new TenpayApiRequestException(
                        $"下载微信支付文件失败，HTTP {(int)response.StatusCode} ({response.ReasonPhrase})：{errorBody}");
                }

                if (destination.CanSeek)
                {
                    destination.SetLength(0);
                    destination.Seek(0, SeekOrigin.Begin);
                }

                using (var hashAlgorithm = CreateHashAlgorithm(hashType))
                using (var source = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                {
                    var buffer = ArrayPool<byte>.Shared.Rent(BufferSize);
                    try
                    {
                        int read;
                        while ((read = await source.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false)) > 0)
                        {
                            cancellationToken.ThrowIfCancellationRequested();
                            hashAlgorithm?.TransformBlock(buffer, 0, read, buffer, 0);
                            await destination.WriteAsync(buffer, 0, read, cancellationToken).ConfigureAwait(false);
                        }

                        hashAlgorithm?.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
                    }
                    finally
                    {
                        ArrayPool<byte>.Shared.Return(buffer);
                    }

                    if (destination.CanSeek)
                    {
                        destination.Seek(0, SeekOrigin.Begin);
                    }

                    if (hashAlgorithm == null || string.IsNullOrWhiteSpace(expectedHash))
                    {
                        return true;
                    }

                    var actualHash = BitConverter.ToString(hashAlgorithm.Hash).Replace("-", string.Empty);
                    return actualHash.Equals(expectedHash, StringComparison.OrdinalIgnoreCase);
                }
            }
        }

        private static HashAlgorithm CreateHashAlgorithm(string hashType)
        {
            switch (hashType?.Replace("-", string.Empty).ToUpperInvariant())
            {
                case null:
                case "":
                    return null;
                case "MD5":
                    return MD5.Create();
                case "SHA1":
                    return SHA1.Create();
                case "SHA256":
                    return SHA256.Create();
                case "SHA384":
                    return SHA384.Create();
                case "SHA512":
                    return SHA512.Create();
                default:
                    throw new NotSupportedException($"不支持的文件摘要算法：{hashType}");
            }
        }
    }
}
