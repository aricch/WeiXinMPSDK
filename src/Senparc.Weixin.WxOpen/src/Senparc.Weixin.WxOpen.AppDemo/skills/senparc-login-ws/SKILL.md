# Senparc 登录与 WebSocket 演示

基于 Senparc.Weixin SDK 体验小程序的登录鉴权与 WebSocket 实时通信演示能力集合。

## 触发场景
用户原话举例（路由命中本技能）：
- "帮我登录一下这个小程序"
- "连上 WebSocket 试试"
- "发一条测试消息到 WebSocket"
- "看看 WebSocket 收到了什么消息"
- "演示一下登录和 WebSocket 功能"
- "帮我跑通 Senparc 的 WebSocket 演示"

## 使用顺序
- 发送 WebSocket 消息前需先完成登录
- 查看消息列表前需先连接 WebSocket 并等待服务端推送
- 若发送时未连接，会自动尝试建立 WebSocket 连接
