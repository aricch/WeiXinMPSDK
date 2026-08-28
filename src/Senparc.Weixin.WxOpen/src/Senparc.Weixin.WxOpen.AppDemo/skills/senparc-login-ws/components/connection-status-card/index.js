Component({
  data: {
    statusText: '等待连接结果',
    hubUrl: '',
  },
  lifetimes: {
    created() {
      console.info('[ai-mode] connection-status-card created')
      const { NotificationType } = wx.modelContext
      const viewCtx = wx.modelContext.getViewContext(this)
      const modelCtx = wx.modelContext.getContext(this)

      viewCtx.on(NotificationType.Overflow, (data) => {
        const overflowed = !!(data && data.overflowHeight > 0)
        console.info('[ai-mode] connection-status-card overflow overflowed=' + overflowed + ' data=' + JSON.stringify(data))
      })
      console.info('[ai-mode] connection-status-card overflow monitor=on')

      modelCtx.on(NotificationType.Result, (data) => {
        const sc = data.result && data.result.structuredContent
        console.info('[ai-mode] connection-status-card 收到 Result:', sc)
        if (!sc) return
        this.setData({
          statusText: sc.status || (sc.connected ? '已连接' : '未连接'),
          hubUrl: sc.hubUrl || '',
        })
        console.info('[ai-mode] connection-status-card setData done')
      })
    },
  },
  methods: {
    onTapSend() {
      console.info('[ai-mode] connection-status-card send api/call name=sendWebSocketMessage args={"message":"TEST"}')
      wx.modelContext.getContext(this).sendFollowUpMessage({
        content: [
          { type: 'text', text: '发送测试消息' },
          { type: 'api/call', data: { name: 'sendWebSocketMessage', arguments: { message: 'TEST' } } },
        ],
      })
    },
    onTapMessages() {
      console.info('[ai-mode] connection-status-card send api/call name=getWebSocketMessages args={}')
      wx.modelContext.getContext(this).sendFollowUpMessage({
        content: [
          { type: 'text', text: '查看消息列表' },
          { type: 'api/call', data: { name: 'getWebSocketMessages', arguments: { limit: 10 } } },
        ],
      })
    },
  },
})
