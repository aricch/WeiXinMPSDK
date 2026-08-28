function errorResult(msg) {
  return { isError: true, content: [{ type: 'text', text: msg }] }
}

function successResult(msg, structuredContent) {
  const result = { isError: false, content: [{ type: 'text', text: msg }] }
  if (structuredContent !== undefined) result.structuredContent = structuredContent
  return result
}

let _storageInited = false
function ensureStorageInit() {
  if (_storageInited) return
  console.info('[ai-mode] ensureStorageInit 初始化域名配置')
  if (!wx.getStorageSync('domainName')) {
    wx.setStorageSync('domainName', 'https://sdk.weixin.senparc.com')
  }
  if (!wx.getStorageSync('wssDomainName')) {
    wx.setStorageSync('wssDomainName', 'wss://sdk.weixin.senparc.com')
  }
  _storageInited = true
}

module.exports = {
  errorResult,
  successResult,
  ensureStorageInit,
}
