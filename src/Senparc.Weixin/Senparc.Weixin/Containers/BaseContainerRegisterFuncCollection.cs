/*----------------------------------------------------------------
    Copyright (C) 2026 Senparc

    文件名：BaseContainerRegisterFuncCollection.cs
    文件功能描述：容器重新注册回调的并发与容量管理集合


    创建标识：Senparc - 20190503

    修改标识：Senparc - 20260718
    修改描述：v6.24.0 新增注册回调并发、容量和生命周期管理

----------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Senparc.Weixin.Containers
{
    /// <summary>
    /// 储存 Container 注册过程方法的集合
    /// </summary>
    /// <typeparam name="TBag"></typeparam>
    public class BaseContainerRegisterFuncCollection<TBag> : Dictionary<string, Func<Task<TBag>>>
         where TBag : class, IBaseContainerBag, new()
    {
        private readonly object _capacityLock = new object();
        private int _maximumCount = 10000;

        public BaseContainerRegisterFuncCollection()
            : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        /// <summary>
        /// 最多保留的自动重注册委托数量。必须大于 0，默认为 10000。
        /// </summary>
        public int MaximumCount
        {
            get => _maximumCount;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "注册委托容量必须大于 0。");
                }

                lock (_capacityLock)
                {
                    if (value < Count)
                    {
                        throw new InvalidOperationException($"当前已有 {Count} 个注册委托，不能把容量降低到 {value}。");
                    }

                    _maximumCount = value;
                }
            }
        }

        /// <summary>
        /// 使用容量检查设置注册委托；替换已有键不占用新容量。
        /// </summary>
        public new Func<Task<TBag>> this[string key]
        {
            get
            {
                lock (_capacityLock)
                {
                    return base[key];
                }
            }
            set
            {
                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }

                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                lock (_capacityLock)
                {
                    if (!ContainsKey(key) && Count >= _maximumCount)
                    {
                        throw new InvalidOperationException($"注册委托数量已达到上限 {_maximumCount}，请先注销不再使用的账号或提高 MaximumCount。");
                    }

                    base[key] = value;
                }
            }
        }

        /// <summary>
        /// 获取当前注册委托数量。
        /// </summary>
        public new int Count
        {
            get
            {
                lock (_capacityLock)
                {
                    return base.Count;
                }
            }
        }

        public new void Add(string key, Func<Task<TBag>> value)
        {
            lock (_capacityLock)
            {
                if (base.Count >= _maximumCount)
                {
                    throw new InvalidOperationException($"注册委托数量已达到上限 {_maximumCount}。");
                }

                base.Add(key, value);
            }
        }

        public new bool Remove(string key)
        {
            lock (_capacityLock)
            {
                return base.Remove(key);
            }
        }

        public new bool ContainsKey(string key)
        {
            lock (_capacityLock)
            {
                return base.ContainsKey(key);
            }
        }

        public new bool TryGetValue(string key, out Func<Task<TBag>> value)
        {
            lock (_capacityLock)
            {
                return base.TryGetValue(key, out value);
            }
        }

        public new void Clear()
        {
            lock (_capacityLock)
            {
                base.Clear();
            }
        }
    }
}
