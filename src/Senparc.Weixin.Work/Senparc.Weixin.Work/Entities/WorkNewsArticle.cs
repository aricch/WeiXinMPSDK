/*----------------------------------------------------------------
    Copyright (C) 2026 Senparc
    
    文件名：WorkNewsArticle.cs
    文件功能描述：企业微信图文消息Article实体（扩展支持小程序跳转）
    
    
    创建标识：Senparc - 20260523
    创建描述：添加appid和pagepath属性以支持跳转到小程序
    
    修改标识：Senparc - 20260523
    修改描述：补充更新日志，完善文件头修改记录

----------------------------------------------------------------*/

using Senparc.NeuChar.Entities;

namespace Senparc.Weixin.Work.Entities
{
    /// <summary>
    /// 企业微信图文消息Article实体（扩展支持小程序跳转）
    /// </summary>
    public class WorkNewsArticle : Article
    {
        /// <summary>
        /// 小程序appid，必须是与当前应用关联的小程序，appid和pagepath必须同时填写，填写后会忽略url字段
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 点击消息卡片后的小程序页面，仅限本小程序内的页面。appid和pagepath必须同时填写，填写后会忽略url字段
        /// </summary>
        public string PagePath { get; set; }
    }
}
