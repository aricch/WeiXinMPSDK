using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Senparc.Weixin.WxOpen.AdvancedAPIs.XPay
{
    /// <summary>
    /// 代币赠送
    /// </summary>
    public class PresentCurrencyRequestData
    {
        /// <summary>
        /// 用户的openid
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// 0-正式环境 1-沙箱环境
        /// </summary>
        public int env { get; set; }

        /// <summary>
        /// 赠送单号
        /// </summary>
        public string order_id { get; set; }

        /// <summary>
        /// 赠送金额
        /// </summary>
        public int amount { get; set; }
    }
}
