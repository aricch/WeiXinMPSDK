/*----------------------------------------------------------------
    Copyright (C) 2026 Senparc
    
    文件名：RequestAppleSubscribeSignData.cs
    文件功能描述：小程序虚拟支付 iOS 会员订阅 - wx.requestAppleSubscribeSign 签名数据
    
    
    创建标识：Senparc - 20260617

----------------------------------------------------------------*/

namespace Senparc.Weixin.WxOpen.AdvancedAPIs.XPay
{
    /// <summary>
    /// iOS 会员订阅签名数据（wx.requestAppleSubscribeSign 的 signData 字段）
    /// https://developers.weixin.qq.com/miniprogram/dev/platform-capabilities/business-capabilities/vip.html
    /// </summary>
    public class RequestAppleSubscribeSignData
    {
        /// <summary>
        /// 在米大师侧申请的应用 id（mp-支付基础配置中的 offerid）
        /// </summary>
        public string offerId { get; set; }

        /// <summary>
        /// 订阅道具 ID（须为双端通用订阅道具）
        /// </summary>
        public string productId { get; set; }

        /// <summary>
        /// 商品单价，单位：分
        /// </summary>
        public int goodsPrice { get; set; }

        /// <summary>
        /// 首开优惠价，单位：分。0 表示免费试用；不传或传原价则表示无优惠
        /// </summary>
        public int activitySellingPrice { get; set; }

        /// <summary>
        /// 透传数据，发货通知时会透传给开发者
        /// </summary>
        public string attach { get; set; }
    }
}
