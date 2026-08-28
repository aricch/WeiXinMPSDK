Component({
  data: {
    messages: [],
    visibleMessages: [],
    totalCount: 0,
    omittedCount: 0,
  },
  lifetimes: {
    created() {
      console.info('[ai-mode] message-list-card created')
      const { NotificationType } = wx.modelContext
      const viewCtx = wx.modelContext.getViewContext(this)
      const modelCtx = wx.modelContext.getContext(this)

      viewCtx.on(NotificationType.Overflow, (data) => {
        const overflowed = !!(data && data.overflowHeight > 0)
        console.info('[ai-mode] message-list-card overflow overflowed=' + overflowed + ' data=' + JSON.stringify(data))
      })
      console.info('[ai-mode] message-list-card overflow monitor=on')

      modelCtx.on(NotificationType.Result, (data) => {
        const sc = data.result && data.result.structuredContent
        console.info('[ai-mode] message-list-card 收到 Result:', sc)
        if (!sc || !sc.messages) return

        const messages = (sc.messages || []).map(function (item, idx) {
          return {
            index: item.index || idx + 1,
            content: item.content || '',
            time: item.time || '',
          }
        })

        const maxVisible = 4
        const visibleMessages = messages.slice(0, maxVisible)
        const omittedCount = messages.length - visibleMessages.length

        this.setData({
          messages: messages,
          visibleMessages: visibleMessages,
          totalCount: sc.total || messages.length,
          omittedCount: omittedCount,
        })
        console.info('[ai-mode] message-list-card setData total=' + messages.length)
      })
    },
  },
  methods: {
    onTapSend() {
      console.info('[ai-mode] message-list-card send api/call name=sendWebSocketMessage args={"message":"Hello"}')
      wx.modelContext.getContext(this).sendFollowUpMessage({
        content: [
          { type: 'text', text: '发送消息' },
          { type: 'api/call', data: { name: 'sendWebSocketMessage', arguments: { message: 'Hello' } } },
        ],
      })
    },
    onTapRefresh() {
      console.info('[ai-mode] message-list-card send api/call name=getWebSocketMessages args={"limit":20}')
      wx.modelContext.getContext(this).sendFollowUpMessage({
        content: [
          { type: 'text', text: '刷新消息列表' },
          { type: 'api/call', data: { name: 'getWebSocketMessages', arguments: { limit: 20 } } },
        ],
      })
    },
  },
})
