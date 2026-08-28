using Senparc.Weixin.Entities;

namespace Senparc.Weixin.MP.AdvancedAPIs.Media
{
    /// <summary>
    /// 非图文/视频永久素材下载结果
    /// </summary>
    public class GetForeverMediaResultJson : WxJsonResult
    {
        /// <summary>
        /// 微信接口响应的 Content-Type
        /// </summary>
        public string content_type { get; set; }
    }
}
