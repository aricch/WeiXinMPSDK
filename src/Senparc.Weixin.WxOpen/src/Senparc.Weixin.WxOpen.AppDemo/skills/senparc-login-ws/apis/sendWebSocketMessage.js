const { errorResult, successResult } = require('../utils/util')
const { ensureLogin } = require('../utils/request')
const { connectWebSocket, sendSocketMessage, isSocketOpen } = require('../utils/websocket')

async function sendWebSocketMessage(params = {}) {
  console.info('[ai-mode] sendWebSocketMessage 入口, params=', JSON.stringify(params))
  try {
    const message = params.message
    if (!message || !String(message).trim()) {
      return errorResult('请提供要发送的消息内容')
    }

    await ensureLogin()

    if (!isSocketOpen()) {
      console.info('[ai-mode] sendWebSocketMessage 未连接，自动建立连接')
      await connectWebSocket('/SenparcHub')
    }

    const result = await sendSocketMessage(String(message).trim())
    console.info('[ai-mode] sendWebSocketMessage 出口 sent=', result.sent)
    return successResult('消息已发送', result)
  } catch (err) {
    console.error('[ai-mode] sendWebSocketMessage 出错:', err.message)
    return errorResult('发送失败: ' + err.message)
  }
}

module.exports = { sendWebSocketMessage }
