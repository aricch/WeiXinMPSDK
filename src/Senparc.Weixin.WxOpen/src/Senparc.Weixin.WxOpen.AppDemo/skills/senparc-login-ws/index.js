const { performLogin } = require('./apis/performLogin')
const { connectWebSocket } = require('./apis/connectWebSocket')
const { sendWebSocketMessage } = require('./apis/sendWebSocketMessage')
const { getWebSocketMessages } = require('./apis/getWebSocketMessages')
const { ensureStorageInit } = require('./utils/util')

ensureStorageInit()

const skill = wx.modelContext.createSkill('skills/senparc-login-ws')

skill.use(async (ctx, next) => {
  console.info('[ai-mode] middleware: ensureStorageInit, api=' + ctx.name)
  ensureStorageInit()
  await next()
})

skill.registerAPI('performLogin', performLogin)
skill.registerAPI('connectWebSocket', connectWebSocket)
skill.registerAPI('sendWebSocketMessage', sendWebSocketMessage)
skill.registerAPI('getWebSocketMessages', getWebSocketMessages)
