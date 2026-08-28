/*----------------------------------------------------------------
    Copyright (C) 2026 Senparc
    
    文件名：ChatStore.cs
    文件功能描述：按个人信息隔离的 Chat 缓存
    
    
    创建标识：Senparc - 20240525

    修改标识：Wang Qian - 20250728
    修改描述：为长对话模式的支持新增了LastStoredMemory、LastStoredPrompt、UseLongChat属性

    修改标识：Senparc - 20260718
    修改描述：v3.1.0 迁移聊天历史到 Microsoft.Extensions.AI 消息模型

----------------------------------------------------------------*/

using Microsoft.Extensions.AI;
using AuthorRole = Microsoft.Extensions.AI.ChatRole;
using System.Collections.Generic;

namespace Senparc.Weixin.MP.Sample.CommonService.AI.MessageHandlers
{
    public class WeixinAiChatHistory(AuthorRole role, string content)
    {
        public AuthorRole Role { get; set; } = role;
        public string Content { get; set; } = content;

    }

    /// <summary>
    /// 按个人信息隔离的 Chat 缓存
    /// </summary>
    public class ChatStore
    {
        public ChatStatus Status { get; set; }

        public MultimodelType MultimodelType { get; set; }
        //public string History { get; set; }

        public List<WeixinAiChatHistory> History { get; set; }

        /// <summary>
        /// 是否使用Markdown格式输出
        /// </summary>
        public bool UseMarkdown { get; set; }

        /// <summary>
        /// 是否使用长对话模式
        /// </summary>
        public bool UseLongChat { get; set; }

        /// <summary>
        /// 上一次保存的记忆
        /// </summary>
        public string LastStoredMemory { get; set; }

        /// <summary>
        /// 上一次提问的prompt
        /// </summary>
        public string LastStoredPrompt { get; set; }

        public ChatStore()
        {
            Status = ChatStatus.None;
            MultimodelType = MultimodelType.None;
            UseMarkdown = true;
            UseLongChat = false;
        }

        public List<ChatMessage> GetChatHistory()
        {
            var history = new List<ChatMessage>();
            foreach (var item in History)
            {
                history.Add(new ChatMessage(item.Role, item.Content));
            }
            return history;
        }

        public void SetChatHistory(IEnumerable<ChatMessage> chatHistory)
        {
            if (chatHistory == null)
            {
                ClearHistory();
                return;
            }

            History = new List<WeixinAiChatHistory>();
            foreach (var message in chatHistory)
            {
                History.Add(new WeixinAiChatHistory(message.Role, message.Text));
            }
        }

        public void AddUserMessage(string content)
        {
            History.Add(new WeixinAiChatHistory(AuthorRole.User, content));
        }

        public void AddAssistantMessage(string content)
        {
            History.Add(new WeixinAiChatHistory(AuthorRole.Assistant, content));
        }

        public void AddSystemMessage(string content)
        {
            History.Add(new WeixinAiChatHistory(AuthorRole.System, content));
        }

        public void AddToolMessage(string content)
        {
            History.Add(new WeixinAiChatHistory(AuthorRole.Tool, content));
        }

        public void ClearHistory()
        {
            History.Clear();
        }
    }

    /// <summary>
    /// 聊天状态
    /// </summary>
    public enum ChatStatus
    {
        /// <summary>
        /// 默认状态（可能是转换失败）
        /// </summary>
        None,
        /// <summary>
        /// 聊天中
        /// </summary>
        Chat,
        /// <summary>
        /// 暂停
        /// </summary>
        Paused
    }

    /// <summary>
    /// 多模态综合对话状态
    /// </summary>
    public enum MultimodelType
    {
        None,
        SimpleChat,
        ChatAndImage
    }
    
}
