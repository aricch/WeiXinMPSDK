# wxa-skills-validate 校验报告

**项目**：Senparc.Weixin.WxOpen.AppDemo  
**校验时间**：2026-06-09（第 2 次重跑）  
**Skill 分包**：`skills/senparc-login-ws`  
**AppID**：`wx12b4f63276b14d4c`

---

## 阶段 1 — 静态校验 + 编译校验

| 项 | 结果 |
|---|---|
| 静态规则 (V001~V016) | ✅ **通过** — 141/141，0 errors，0 warnings |
| 编译校验 (cli preview) | ❌ **失败** — IDE 服务端口未开启 |

### 静态规则明细

```
V001:36  V002:4  V003:16  V005:40  V006:24  V007:1  V008:1
V009:4   V011:4  V012:4  V013:1  V014:1  V015:4  V016:1
```

报告文件：`cli-agent-run/validate-report.json`

### 编译失败原因

```
IDE service port disabled.
工具的服务端口已关闭。要使用命令行调用工具，请手动打开工具 -> 设置 -> 安全设置，将服务端口开启。
```

**本机配置确认**（微信开发者工具 `WeappLocalData`）：

```json
"security": { "enableServicePort": false, "port": null }
```

服务端口仍为关闭状态，CLI 所有命令（`islogin` / `preview` / `agent`）均无法初始化。

---

## 阶段 2 — CLI 准备

| 项 | 结果 |
|---|---|
| CLI 可执行 | ✅ `/Applications/wechatwebdevtools.app/Contents/MacOS/cli -h` 正常 |
| 服务端口 | ❌ 未开启，阻塞后续所有 CLI 调用 |

---

## 阶段 3 — 执行计划

按依赖顺序排列的原子接口：

| 顺序 | 接口 | 组件 | 入参来源 |
|------|------|------|---------|
| 1 | `performLogin` | login-status-card | 无参 |
| 2 | `connectWebSocket` | connection-status-card | 可选 `hubPath` |
| 3 | `sendWebSocketMessage` | send-result-card | `message`（必填） |
| 4 | `getWebSocketMessages` | message-list-card | 可选 `limit` |

---

## 阶段 4 — execute / render

| 接口 | execute | render | 备注 |
|------|---------|--------|------|
| performLogin | ⏸ 阻塞 | ⏸ 阻塞 | CLI 服务端口未开启 |
| connectWebSocket | ⏸ 阻塞 | ⏸ 阻塞 | 同上 |
| sendWebSocketMessage | ⏸ 阻塞 | ⏸ 阻塞 | 同上 |
| getWebSocketMessages | ⏸ 阻塞 | ⏸ 阻塞 | 同上 |

---

## 阶段 5 — 交付

❌ **未完成** — 真机闭环未执行，无法生成 `DELIVERY.md`。

---

## 解除阻塞后重跑命令

### 1. 开启服务端口

打开微信开发者工具 → **设置** → **安全设置** → 开启 **服务端口**。

### 2. 重新校验

```bash
cd /Volumes/DevelopAndData/SenparcProjects/WeiXinMPSDK/src/Senparc.Weixin.WxOpen/src/Senparc.Weixin.WxOpen.AppDemo

# 静态 + 编译
node .cursor/skills/wxa-skills-validate/scripts/validate.mjs .

# 逐个 execute
node .cursor/skills/wxa-skills-validate/scripts/execute.mjs \
  --project . --name performLogin \
  --output ./cli-agent-run/execute-result.performLogin.json

node .cursor/skills/wxa-skills-validate/scripts/execute.mjs \
  --project . --name connectWebSocket \
  --output ./cli-agent-run/execute-result.connectWebSocket.json

node .cursor/skills/wxa-skills-validate/scripts/execute.mjs \
  --project . --name sendWebSocketMessage \
  --args '{"message":"TEST"}' \
  --output ./cli-agent-run/execute-result.sendWebSocketMessage.json

node .cursor/skills/wxa-skills-validate/scripts/execute.mjs \
  --project . --name getWebSocketMessages \
  --args '{"limit":10}' \
  --output ./cli-agent-run/execute-result.getWebSocketMessages.json

# 逐个 render（从 execute 产物继承）
node .cursor/skills/wxa-skills-validate/scripts/render.mjs \
  --project . \
  --from-execute ./cli-agent-run/execute-result.performLogin.json \
  --output ./cli-agent-run/render-result.performLogin.json
# ... 对其余 3 个接口重复
```

---

## 总结

- **代码静态质量**：✅ 全部通过，skill 产物结构合规
- **编译与真机**：❌ 被环境阻塞（微信开发者工具服务端口未开启）
- **建议**：在开发者工具中开启服务端口后，重新执行 `@wxa-skills-validate 校验 ./skills 目录`
