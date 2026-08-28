Component({
  data: {
    statusText: '等待登录结果',
    sessionId: '',
  },
  lifetimes: {
    created() {
      console.info('[ai-mode] login-status-card created')
      const { NotificationType } = wx.modelContext
      const viewCtx = wx.modelContext.getViewContext(this)
      const modelCtx = wx.modelContext.getContext(this)

      viewCtx.on(NotificationType.Overflow, (data) => {
        const overflowed = !!(data && data.overflowHeight > 0)
        console.info('[ai-mode] login-status-card overflow overflowed=' + overflowed + ' data=' + JSON.stringify(data))
      })
      console.info('[ai-mode] login-status-card overflow monitor=on')

      modelCtx.on(NotificationType.Result, (data) => {
        const sc = data.result && data.result.structuredContent
        console.info('[ai-mode] login-status-card 收到 Result:', sc)
        if (!sc) return
        this.setData({
          statusText: sc.message || (sc.success ? '登录成功' : '登录失败'),
          sessionId: sc.sessionId || '',
        })
        console.info('[ai-mode] login-status-card setData done')
      })
    },
  },
  methods: {
    onTapConnect() {
      console.info('[ai-mode] login-status-card send api/call name=connectWebSocket args={}')
      wx.modelContext.getContext(this).sendFollowUpMessage({
        content: [
          { type: 'text', text: '连接 WebSocket' },
          { type: 'api/call', data: { name: 'connectWebSocket', arguments: {} } },
        ],
      })
    },
  },
})
