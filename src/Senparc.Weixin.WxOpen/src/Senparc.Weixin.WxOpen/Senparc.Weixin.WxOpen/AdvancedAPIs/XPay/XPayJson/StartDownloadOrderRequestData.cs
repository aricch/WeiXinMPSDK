using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Senparc.Weixin.WxOpen.AdvancedAPIs.XPay
{
    /// <summary>
    /// 下载支付订单
    /// </summary>
    public class StartDownloadOrderRequestData
    {
        /// <summary>
        /// 开始日期，格式 YYYYMMDD
        /// </summary>
        public long begin_ds { get; set; }

        /// <summary>
        /// 结束日期，格式 YYYYMMDD，与 begin_ds 间隔不超过 31 天
        /// </summary>
        public long end_ds { get; set; }

        /// <summary>
        /// 订单类型：1=代币交易订单 /2=道具直购交易订单 /3=会员订阅订单 /4=退款订单
        /// </summary>
        public int order_type { get; set; }

        /// <summary>
        /// 订单信息搜索关键字，支持按交易单号/商户单号/用户ID 模糊匹配
        /// </summary>
        public string order_info { get; set; }

        /// <summary>
        /// 发货状态，order_type 为 2(道具) 或 3(会员订阅) 时必须传入；true=已发货 /false=未发货；不传默认 true
        /// </summary>
        public bool? is_provided { get; set; }

        /// <summary>
        /// 退款状态筛选，仅 order_type=4(退款订单) 时有效；0=全部 /2=已退款 /4=退款中 /5=退款失败；不传默认 0（全部）
        /// </summary>
        public int refund_status { get; set; }

        /// <summary>
        /// 环境标识：0=现网 /1=沙箱（用于基类签名校验）
        /// </summary>
        public int env { get; set; }

        /// <summary>
        /// 支付渠道：1=普通虚拟支付 /2=苹果IAP
        /// </summary>
        public int pay_channel { get; set; }

    }
}
