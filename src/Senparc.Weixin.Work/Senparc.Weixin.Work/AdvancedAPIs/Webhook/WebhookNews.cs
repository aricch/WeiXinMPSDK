/*----------------------------------------------------------------
    Copyright (C) 2026 Senparc
    
    文件名：WebhookNews.cs
    文件功能描述：Webhook群机器人相关Api，News 传入参数实例
    
    
    创建标识：lishewen - 20190701
  
    修改标识：Senparc - 20260523
    修改描述：补充更新日志，完善文件头修改记录

----------------------------------------------------------------*/

/*
    官方文档：https://work.weixin.qq.com/api/doc?notreplace=true#90000/90135/91760
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Senparc.Weixin.Work.AdvancedAPIs.Webhook
{
    public class WebhookNews
    {
        public string msgtype { get; set; } = "news";
        public News news { get; set; }
    }

    public class News
    {
        /// <summary>
        /// 图文消息，一个图文消息支持1到8条图文
        /// </summary>
        public Article[] articles { get; set; }
    }

    public class Article
    {
        /// <summary>
        /// 标题，不超过128个字节，超过会自动截断
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 描述，不超过512个字节，超过会自动截断
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 点击后跳转的链接。
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 图文消息的图片链接，支持JPG、PNG格式，较好的效果为大图 1068*455，小图150*150。
        /// </summary>
        public string picurl { get; set; }
        /// <summary>
        /// 小程序appid，必须是与当前应用关联的小程序，appid和pagepath必须同时填写，填写后会忽略url字段
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// 点击消息卡片后的小程序页面，仅限本小程序内的页面。appid和pagepath必须同时填写，填写后会忽略url字段
        /// </summary>
        public string pagepath { get; set; }
    }

}

