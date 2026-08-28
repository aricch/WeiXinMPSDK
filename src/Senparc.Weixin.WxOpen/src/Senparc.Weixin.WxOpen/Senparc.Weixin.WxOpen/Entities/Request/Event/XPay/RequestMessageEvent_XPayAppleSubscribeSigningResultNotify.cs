/*----------------------------------------------------------------
    Copyright (C) 2026 Senparc
    
    文件名：RequestMessageEvent_XPayAppleSubscribeSigningResultNotify.cs
    文件功能描述：小程序虚拟支付 iOS 会员订阅 - 签约结果推送
    
    
    创建标识：Senparc - 20260617

----------------------------------------------------------------*/

namespace Senparc.Weixin.WxOpen.Entities
{
    /// <summary>
    /// iOS 会员订阅签约结果推送
    /// https://developers.weixin.qq.com/miniprogram/dev/platform-capabilities/business-capabilities/vip.html
    /// </summary>
    public class RequestMessageEvent_XPayAppleSubscribeSigningResultNotify : RequestMessageEventBase, IRequestMessageEventBase
    {
        public override Event Event
        {
            get { return Event.xpay_apple_subscribe_signing_result_notify; }
        }

        /// <summary>
        /// 用户 openid
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 业务订单号
        /// </summary>
        public string OutTradeNo { get; set; }

        /// <summary>
        /// 环境配置 0：现网环境（正式环境）1：沙箱环境
        /// </summary>
        public int Env { get; set; }

        /// <summary>
        /// 订阅道具 ID
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// 签约结果，0 为成功，非 0 为失败
        /// </summary>
        public int ContractResult { get; set; }

        /// <summary>
        /// 签约结果描述，失败时为失败原因
        /// </summary>
        public string ContractResultMsg { get; set; }

        /// <summary>
        /// 签约协议号（签约成功时返回）
        /// </summary>
        public string ContractId { get; set; }

        /// <summary>
        /// 透传数据
        /// </summary>
        public string Attach { get; set; }
    }
}
