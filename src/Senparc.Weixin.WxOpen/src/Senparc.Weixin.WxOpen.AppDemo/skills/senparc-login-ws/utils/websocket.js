const { ensureStorageInit } = require('./util')
const { getSessionId } = require('./request')

let _socketOpen = false
let _listenersInited = false
let _connectPromise = null
let _messageBuffer = []
const MAX_BUFFER = 50

function initListeners() {
  if (_listenersInited) return
  _listenersInited = true
  console.info('[ai-mode] websocket initListeners')

  wx.onSocketOpen(function () {
    console.info('[ai-mode] websocket onSocketOpen')
    _socketOpen = true
  })

  wx.onSocketMessage(function (res) {
    console.info('[ai-mode] websocket onSocketMessage', res.data)
    try {
      const jsonResult = JSON.parse(res.data)
      _messageBuffer.unshift({
        index: _messageBuffer.length + 1,
        content: jsonResult.content || String(res.data),
        time: jsonResult.time || new Date().toLocaleString(),
      })
      if (_messageBuffer.length > MAX_BUFFER) {
        _messageBuffer = _messageBuffer.slice(0, MAX_BUFFER)
      }
    } catch (e) {
      _messageBuffer.unshift({
        index: _messageBuffer.length + 1,
        content: String(res.data),
        time: new Date().toLocaleString(),
      })
    }
  })

  wx.onSocketClose(function () {
    console.info('[ai-mode] websocket onSocketClose')
    _socketOpen = false
    _connectPromise = null
  })

  wx.onSocketError(function (err) {
    console.error('[ai-mode] websocket onSocketError', err)
    _socketOpen = false
    _connectPromise = null
  })
}

function connectWebSocket(hubPath) {
  initListeners()
  ensureStorageInit()

  if (_socketOpen) {
    console.info('[ai-mode] websocket 已连接，跳过重复连接')
    const wssDomain = wx.getStorageSync('wssDomainName') || 'wss://sdk.weixin.senparc.com'
    return Promise.resolve({
      connected: true,
      status: 'WebSocket 已连接',
      hubUrl: wssDomain + hubPath,
    })
  }

  if (_connectPromise) {
    console.info('[ai-mode] websocket 等待进行中的连接')
    return _connectPromise
  }

  const wssDomain = wx.getStorageSync('wssDomainName') || 'wss://sdk.weixin.senparc.com'
  const hubUrl = wssDomain + hubPath
  console.info('[ai-mode] websocket connectSocket', hubUrl)

  _connectPromise = new Promise((resolve, reject) => {
    const timeout = setTimeout(() => {
      _connectPromise = null
      reject(new Error('WebSocket 连接超时'))
    }, 10000)

    const onOpenHandler = function () {
      clearTimeout(timeout)
      wx.offSocketOpen(onOpenHandler)
      resolve({
        connected: true,
        status: 'WebSocket 连接成功',
        hubUrl: hubUrl,
      })
    }

    wx.onSocketOpen(onOpenHandler)

    wx.connectSocket({
      url: hubUrl,
      header: { 'content-type': 'application/json' },
      method: 'GET',
      fail(err) {
        clearTimeout(timeout)
        wx.offSocketOpen(onOpenHandler)
        _connectPromise = null
        reject(err)
      },
    })
  })

  return _connectPromise
}

function sendSocketMessage(message) {
  if (!_socketOpen) {
    return Promise.reject(new Error('WebSocket 未连接，请先连接'))
  }

  const sessionId = getSessionId() || ''
  const submitData = JSON.stringify({
    Message: message,
    SessionId: sessionId,
    FormId: '',
  })

  console.info('[ai-mode] websocket sendSocketMessage', submitData)
  return new Promise((resolve, reject) => {
    wx.sendSocketMessage({
      data: submitData,
      success() {
        resolve({ sent: true, message: message })
      },
      fail(err) {
        reject(err)
      },
    })
  })
}

function getBufferedMessages(limit) {
  const count = limit && limit > 0 ? limit : 10
  return _messageBuffer.slice(0, count)
}

function isSocketOpen() {
  return _socketOpen
}

module.exports = {
  connectWebSocket,
  sendSocketMessage,
  getBufferedMessages,
  isSocketOpen,
}
