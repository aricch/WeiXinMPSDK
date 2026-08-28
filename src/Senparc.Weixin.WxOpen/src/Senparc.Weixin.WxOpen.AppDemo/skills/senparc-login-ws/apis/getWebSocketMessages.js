const { errorResult, successResult } = require('../utils/util')
const { getBufferedMessages } = require('../utils/websocket')

async function getWebSocketMessages(params = {}) {
  console.info('[ai-mode] getWebSocketMessages 入口, params=', JSON.stringify(params))
  try {
    const limit = params.limit || 10
    const messages = getBufferedMessages(limit)
    console.info('[ai-mode] getWebSocketMessages 出口 count=', messages.length)
    return successResult(
      messages.length > 0 ? '获取到 ' + messages.length + ' 条消息' : '暂无 WebSocket 消息',
      { messages: messages, total: messages.length }
    )
  } catch (err) {
    console.error('[ai-mode] getWebSocketMessages 出错:', err.message)
    return errorResult('获取消息失败: ' + err.message)
  }
}

module.exports = { getWebSocketMessages }
