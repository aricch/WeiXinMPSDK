Component({
  data: {
    sent: false,
    statusText: '等待发送结果',
    message: '',
  },
  lifetimes: {
    created() {
      console.info('[ai-mode] send-result-card created')
      const { NotificationType } = wx.modelContext
      const viewCtx = wx.modelContext.getViewContext(this)
      const modelCtx = wx.modelContext.getContext(this)

      viewCtx.on(NotificationType.Overflow, (data) => {
        const overflowed = !!(data && data.overflowHeight > 0)
        console.info('[ai-mode] send-result-card overflow overflowed=' + overflowed + ' data=' + JSON.stringify(data))
      })
      console.info('[ai-mode] send-result-card overflow monitor=on')

      modelCtx.on(NotificationType.Result, (data) => {
        const sc = data.result && data.result.structuredContent
        console.info('[ai-mode] send-result-card 收到 Result:', sc)
        if (!sc) return
        this.setData({
          sent: !!sc.sent,
          statusText: sc.sent ? '消息已发送' : '发送未完成',
          message: sc.message || '',
        })
        console.info('[ai-mode] send-result-card setData done')
      })
    },
  },
  methods: {
    onTapMessages() {
      console.info('[ai-mode] send-result-card send api/call name=getWebSocketMessages args={"limit":10}')
      wx.modelContext.getContext(this).sendFollowUpMessage({
        content: [
          { type: 'text', text: '查看消息列表' },
          { type: 'api/call', data: { name: 'getWebSocketMessages', arguments: { limit: 10 } } },
        ],
      })
    },
  },
})
