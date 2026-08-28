using Senparc.Weixin.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Senparc.Weixin.WxOpen.AdvancedAPIs.XPay
{
    /// <summary>
    /// 
    /// </summary>
    public class DownloadAdverfundsOrderJsonResult : WxJsonResult
    {
        /// <summary>
        /// 订单下载链接
        /// </summary>
        public string url { get; set; }
    }
}
