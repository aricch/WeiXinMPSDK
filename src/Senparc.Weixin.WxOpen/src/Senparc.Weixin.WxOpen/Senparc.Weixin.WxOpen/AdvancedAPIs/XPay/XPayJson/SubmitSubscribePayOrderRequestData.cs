using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Senparc.Weixin.WxOpen.AdvancedAPIs.XPay
{
    /// <summary>
    /// 发起订阅扣款
    /// </summary>
    public class SubmitSubscribePayOrderRequestData
    {
        /// <summary>
        /// 用户的openid
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// 在米大师侧申请的应用 id, mp-支付基础配置中的offerid
        /// </summary>
        public string offer_id { get; set; }

        /// <summary>
        /// 购买数量，填：1
        /// </summary>
        public string buy_quantity { get; set; }

        /// <summary>
        /// 0-正式环境 1-沙箱环境
        /// </summary>
        public int env { get; set; }

        /// <summary>
        /// 币种，填：CNY
        /// </summary>
        public string currency_type { get; set; }

        /// <summary>
        /// 订阅道具ID
        /// </summary>
        public string product_id { get; set; }

        /// <summary>
        /// 扣款金额(分), 属于 [1，道具价格]
        /// </summary>
        public int deduct_price { get; set; }

        /// <summary>
        /// 业务订单号, 每个订单号只能使用一次, 重复使用会失败(不建议业务强依赖平台对这里的唯一性校验)，要求8-32个字符内, 只能是数字、大小写字母、符号 _-\
        /// </summary>
        public string order_id { get; set; }

        /// <summary>
        /// 透传数据, 发货通知时会透传给开发者
        /// </summary>
        public string attach { get; set; }
    }
}
