const { errorResult, successResult } = require('../utils/util')
const { ensureLogin } = require('../utils/request')

async function performLogin(params = {}) {
  console.info('[ai-mode] performLogin 入口, params=', JSON.stringify(params))
  try {
    const sessionId = await ensureLogin()
    console.info('[ai-mode] performLogin 出口 sessionId 已获取')
    return successResult('登录成功，已获取会话标识', {
      success: true,
      sessionId: sessionId,
      message: '已通过 wx.login 完成 Senparc SDK 登录',
    })
  } catch (err) {
    console.error('[ai-mode] performLogin 出错:', err.message)
    return errorResult('登录失败: ' + err.message)
  }
}

module.exports = { performLogin }
