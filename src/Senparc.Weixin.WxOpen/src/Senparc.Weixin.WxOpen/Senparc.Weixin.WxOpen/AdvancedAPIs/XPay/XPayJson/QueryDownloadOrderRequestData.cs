using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Senparc.Weixin.WxOpen.AdvancedAPIs.XPay
{
    /// <summary>
    /// 查询下载订单任务
    /// </summary>
    public class QueryDownloadOrderRequestData
    {
        /// <summary>
        /// 下载任务 ID，由 StartDownloadOrder 接口返回
        /// </summary>
        public string task_id { get; set; }

        /// <summary>
        /// 环境标识：0=现网 /1=沙箱（用于基类签名校验）
        /// </summary>
        public int env { get; set; }

    }
}
