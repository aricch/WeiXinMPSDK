const { errorResult, successResult } = require('../utils/util')
const { ensureLogin } = require('../utils/request')
const { connectWebSocket: connectWs } = require('../utils/websocket')

async function connectWebSocket(params = {}) {
  console.info('[ai-mode] connectWebSocket 入口, params=', JSON.stringify(params))
  try {
    await ensureLogin()
    const hubPath = params.hubPath || '/SenparcHub'
    const result = await connectWs(hubPath)
    console.info('[ai-mode] connectWebSocket 出口 connected=', result.connected)
    return successResult(result.status, result)
  } catch (err) {
    console.error('[ai-mode] connectWebSocket 出错:', err.message)
    return errorResult('WebSocket 连接失败: ' + err.message)
  }
}

module.exports = { connectWebSocket }
