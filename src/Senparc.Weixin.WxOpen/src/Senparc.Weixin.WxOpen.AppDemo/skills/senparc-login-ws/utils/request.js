const { ensureStorageInit } = require('./util')

const BASE_URL = 'https://sdk.weixin.senparc.com'

let _sessionId = ''
let _loginPromise = null

function getSessionId() {
  return _sessionId
}

function request(options) {
  ensureStorageInit()
  const domain = wx.getStorageSync('domainName') || BASE_URL
  return new Promise((resolve, reject) => {
    console.info('[ai-mode] request 发起', options.url)
    wx.request({
      ...options,
      url: options.url.startsWith('http') ? options.url : domain + options.url,
      header: Object.assign(
        { 'content-type': 'application/x-www-form-urlencoded' },
        options.header || {}
      ),
      success(res) {
        console.info('[ai-mode] request 响应', options.url, res.statusCode)
        if (res.statusCode >= 200 && res.statusCode < 300) {
          resolve(res.data)
        } else {
          reject(new Error('HTTP ' + res.statusCode))
        }
      },
      fail(err) {
        console.error('[ai-mode] request 失败', options.url, err)
        reject(err)
      },
    })
  })
}

function ensureLogin() {
  if (_sessionId) {
    console.info('[ai-mode] ensureLogin 复用已有 sessionId')
    return Promise.resolve(_sessionId)
  }
  if (_loginPromise) {
    console.info('[ai-mode] ensureLogin 等待进行中的登录')
    return _loginPromise
  }

  _loginPromise = new Promise((resolve, reject) => {
    console.info('[ai-mode] ensureLogin 开始 wx.login')
    wx.login({
      success(loginRes) {
        if (!loginRes.code) {
          _loginPromise = null
          reject(new Error('wx.login 未返回 code'))
          return
        }
        request({
          url: '/WxOpen/OnLogin',
          method: 'POST',
          data: { code: loginRes.code },
        })
          .then((result) => {
            console.info('[ai-mode] ensureLogin OnLogin 结果', result && result.success)
            if (result && result.success && result.sessionId) {
              _sessionId = result.sessionId
              resolve(_sessionId)
            } else {
              reject(new Error('登录失败，未获取 sessionId'))
            }
          })
          .catch((err) => {
            reject(err)
          })
          .finally(() => {
            _loginPromise = null
          })
      },
      fail(err) {
        _loginPromise = null
        reject(err)
      },
    })
  })

  return _loginPromise
}

module.exports = {
  request,
  ensureLogin,
  getSessionId,
}
