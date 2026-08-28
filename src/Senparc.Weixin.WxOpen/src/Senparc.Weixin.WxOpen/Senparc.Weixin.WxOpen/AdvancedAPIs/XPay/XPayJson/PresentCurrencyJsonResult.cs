using Senparc.Weixin.Entities;
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
    public class PresentCurrencyJsonResult : WxJsonResult
    {
        /// <summary>
        /// 赠送后用户的代币余额
        /// </summary>
        public int balance { get; set; }

        /// <summary>
        /// 赠送单号
        /// </summary>
        public string order_id { get; set; }

        /// <summary>
        /// 用户收到的总赠送金额
        /// </summary>
        public long present_balance { get; set; }
    }
}
