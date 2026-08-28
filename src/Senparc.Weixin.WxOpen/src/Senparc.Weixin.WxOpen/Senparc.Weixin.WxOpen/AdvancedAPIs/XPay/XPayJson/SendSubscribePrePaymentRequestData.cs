using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Senparc.Weixin.WxOpen.AdvancedAPIs.XPay
{
    /// <summary>
    /// 预通知扣款
    /// </summary>
    public class SendSubscribePrePaymentRequestData
    {
        /// <summary>
        /// 用户的openid
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// 金额/分，属于 [100，道具价格]
        /// </summary>
        public int deduct_price { get; set; }

        /// <summary>
        /// 道具 id，需为订阅制道具
        /// </summary>
        public string product_id { get; set; }

        /// <summary>
        /// 签约时传入的协议号
        /// </summary>
        public string out_contract_code { get; set; }

    }
}
