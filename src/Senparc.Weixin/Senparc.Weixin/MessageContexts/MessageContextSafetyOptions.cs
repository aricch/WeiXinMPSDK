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

using Senparc.NeuChar.Context;
using System;
using System.Threading;

namespace Senparc.Weixin.MessageContexts
{
    /// <summary>
    /// 微信消息上下文的安全容量设置。
    /// </summary>
    public static class MessageContextSafetyOptions
    {
        /// <summary>
        /// 显式请求安全全局默认记录数时使用的值。
        /// </summary>
        public const int DefaultRecordCount = -2;

        /// <summary>
        /// 显式请求无限记录时使用的值。为保持历史兼容，公开构造函数参数 0 同样表示无限记录。
        /// </summary>
        public const int UnlimitedRecordCount = -1;

        private static int _maximumRecordCount = 1000;
        private static long _createdHandlerCount;
        private static long _unlimitedHandlerCount;

        /// <summary>
        /// 单个会话允许配置的最大记录数，防止误配置造成无界增长。默认 1000。
        /// </summary>
        public static int MaximumRecordCount
        {
            get => Volatile.Read(ref _maximumRecordCount);
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }
                Volatile.Write(ref _maximumRecordCount, value);
            }
        }

        /// <summary>
        /// 当前全局默认记录数。Senparc.NeuChar 默认值为 20。
        /// </summary>
        public static int DefaultMaxRecordCount
        {
            get => MessageContextGlobalConfig.MaxRecordCount;
            set
            {
                if (value <= 0 || value > MaximumRecordCount)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }
                MessageContextGlobalConfig.MaxRecordCount = value;
            }
        }

        /// <summary>
        /// 会话空闲过期分钟数。Senparc.NeuChar 默认值为 30。
        /// </summary>
        public static int ExpireMinutes
        {
            get => MessageContextGlobalConfig.ExpireMinutes;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }
                MessageContextGlobalConfig.ExpireMinutes = value;
            }
        }

        /// <summary>
        /// 本进程创建消息处理器的累计数量，可用于运行时观测。
        /// </summary>
        public static long CreatedHandlerCount => Interlocked.Read(ref _createdHandlerCount);

        /// <summary>
        /// 显式启用无限记录的处理器累计数量。
        /// </summary>
        public static long UnlimitedHandlerCount => Interlocked.Read(ref _unlimitedHandlerCount);

        /// <summary>
        /// 将公开构造函数参数转换为底层上下文容量。
        /// </summary>
        public static int ResolveMaxRecordCount(int requestedMaxRecordCount)
        {
            Interlocked.Increment(ref _createdHandlerCount);

            if (requestedMaxRecordCount == 0 || requestedMaxRecordCount == UnlimitedRecordCount)
            {
                Interlocked.Increment(ref _unlimitedHandlerCount);
                return 0;
            }

            if (requestedMaxRecordCount == DefaultRecordCount)
            {
                return DefaultMaxRecordCount;
            }

            if (requestedMaxRecordCount < DefaultRecordCount)
            {
                throw new ArgumentOutOfRangeException(nameof(requestedMaxRecordCount));
            }

            // 正数是旧 API 的显式容量，不做静默截断。
            return requestedMaxRecordCount;
        }
    }
}
