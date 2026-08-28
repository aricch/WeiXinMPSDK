using Senparc.Weixin.Entities;
using System.Collections.Generic;

namespace Senparc.Weixin.WxOpen.AdvancedAPIs.XPay
{
    /// <summary>
    /// 下载支付订单
    /// </summary>
    public class StartDownloadOrderJsonResult: WxJsonResult
    {
        /// <summary>
        /// 下载任务 ID，用于后续查询下载结果
        /// </summary>
        public string task_id { get; set; }
    }
}
