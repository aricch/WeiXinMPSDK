/*----------------------------------------------------------------
    Copyright (C) 2026 Senparc
    
    文件名：RequestMessageEvent_XPaySubscribeIosRefundQueryNotify.cs
    文件功能描述：小程序虚拟支付 iOS 会员订阅 - 退款推送
    
    
    创建标识：Senparc - 20260617

    修改标识：lilinjian2001 - 20260626
    修改描述：v3.27.2 fix: 修复 微信小程序虚拟支付，IOS退款消息推送，属性错误！ / Issue #3310 / PR #3311

----------------------------------------------------------------*/

namespace Senparc.Weixin.WxOpen.Entities
{
    /// <summary>
    /// iOS 会员订阅退款推送
    /// https://developers.weixin.qq.com/miniprogram/dev/platform-capabilities/business-capabilities/vip.html
    /// </summary>
    public class RequestMessageEvent_XPaySubscribeIosRefundQueryNotify : RequestMessageEventBase, IRequestMessageEventBase
    {
        public override Event Event
        {
            get { return Event.xpay_subscribe_ios_refund_query_notify; }
        }

        /// <summary>
        /// 问询时间，Unix时间戳
        /// </summary>
        public string refund_time { get; set; }

        /// <summary>
        /// 该笔退款的订单时间（退款订单对应的交易时间），Unix时间戳
        /// </summary>
        public string order_time { get; set; }

        /// <summary>
        /// Apple 支付票据号
        /// </summary>
        public string channel_bill { get; set; }

        /// <summary>
        /// 道具 id
        /// </summary>
        public string product_id { get; set; }

        /// <summary>
        /// 道具/代币数量
        /// </summary>
        public string p_count { get; set; }

        /// <summary>
        /// 用户请求退款的原因
        /// </summary>
        public string refund_request_reason { get; set; }

        /// <summary>
        /// 发货状态，0 : 未发货 1：已发货 2：发货中
        /// </summary>
        public string provide_status { get; set; }

        /// <summary>
        /// 退款对应支付订单号
        /// </summary>
        public string pay_order_id { get; set; }
    }
}
