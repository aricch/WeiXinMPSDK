using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Senparc.Weixin.WxOpen.AdvancedAPIs.XPay
{
    /// <summary>
    /// 下载广告金对应的商户订单信息
    /// </summary>
    public class DownloadAdverfundsOrderRequestData
    {
        /// <summary>
        /// 广告金发放ID
        /// </summary>
        public string fund_id { get; set; }

        /// <summary>
        /// 0-正式环境 1-沙箱环境
        /// </summary>
        public int env { get; set; }
    }
}
