/*----------------------------------------------------------------
    Copyright (C) 2026 Senparc

    文件名：ContainerCacheCapabilities.cs
    文件功能描述：描述容器缓存策略支持的能力


    创建标识：Senparc - 20260717

    修改标识：Senparc - 20260718
    修改描述：v6.24.0 新增容器缓存能力描述契约

----------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Senparc.Weixin.Containers;

namespace Senparc.Weixin.Cache
{
    /// <summary>
    /// 容器缓存后端可提供的可选能力。
    /// </summary>
    [Flags]
    public enum ContainerCacheCapabilities
    {
        None = 0,
        Enumeration = 1,
        AsyncEnumeration = 2,
        Paging = 4
    }

    /// <summary>
    /// 容器缓存分页结果。
    /// </summary>
    public sealed class ContainerCachePage<TBag> where TBag : IBaseContainerBag
    {
        public ContainerCachePage(IReadOnlyList<KeyValuePair<string, TBag>> items, int offset, int totalCount)
        {
            Items = items ?? throw new ArgumentNullException(nameof(items));
            Offset = offset;
            TotalCount = totalCount;
        }

        public IReadOnlyList<KeyValuePair<string, TBag>> Items { get; }
        public int Offset { get; }
        public int TotalCount { get; }
        public bool HasMore => Offset + Items.Count < TotalCount;
    }

    /// <summary>
    /// 容器缓存能力检测和有界分页读取扩展。
    /// </summary>
    public static class ContainerCacheStrategyExtensions
    {
        public static ContainerCacheCapabilities GetCapabilities(this IContainerCacheStrategy strategy)
        {
            if (strategy == null)
            {
                throw new ArgumentNullException(nameof(strategy));
            }

            return strategy is BaseContainerCacheStrategy baseStrategy
                ? baseStrategy.Capabilities
                : ContainerCacheCapabilities.None;
        }

        public static bool Supports(this IContainerCacheStrategy strategy, ContainerCacheCapabilities capability)
        {
            return (strategy.GetCapabilities() & capability) == capability;
        }

        public static bool TryGetAll<TBag>(this IContainerCacheStrategy strategy, out IDictionary<string, TBag> items)
            where TBag : IBaseContainerBag
        {
            if (!strategy.Supports(ContainerCacheCapabilities.Enumeration))
            {
                items = new Dictionary<string, TBag>();
                return false;
            }

            items = strategy.GetAll<TBag>();
            return true;
        }

        public static async Task<ContainerCachePage<TBag>> GetPageAsync<TBag>(
            this IContainerCacheStrategy strategy,
            int offset,
            int pageSize,
            CancellationToken cancellationToken = default)
            where TBag : IBaseContainerBag
        {
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            if (pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            }

            if (!strategy.Supports(ContainerCacheCapabilities.AsyncEnumeration))
            {
                throw new NotSupportedException($"缓存策略 {strategy.GetType().Name} 不支持枚举，请先检查 GetCapabilities()。");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var all = await strategy.GetAllAsync<TBag>().ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();
            var page = all.OrderBy(item => item.Key, StringComparer.Ordinal)
                .Skip(offset)
                .Take(pageSize)
                .ToList();
            return new ContainerCachePage<TBag>(page, offset, all.Count);
        }
    }
}
