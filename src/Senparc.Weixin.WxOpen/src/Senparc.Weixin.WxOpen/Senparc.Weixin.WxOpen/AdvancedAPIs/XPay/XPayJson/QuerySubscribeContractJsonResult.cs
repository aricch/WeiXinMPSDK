using Senparc.Weixin.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Senparc.Weixin.WxOpen.AdvancedAPIs.XPay
{
    /// <summary>
    /// 查询签约关系
    /// </summary>
    public class QuerySubscribeContractJsonResult : WxJsonResult
    {
        /// <summary>
        /// SIGNED: 签约生效中。TERMINATED: 生效的签约协议已被解约。此时协议已经到达终态，该协议无法再次进行签约；可更换协议号再发起签约。UNBINDUSER: 从未签约过
        /// </summary>
        public string authorization_state { get; set; }
    }
}
