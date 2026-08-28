# Senparc.Weixin CLI AI Harness

`weixin` 是 WeixinCLI / WeChatCLI 的统一后继实现。它把 `Senparc.AI.AgentKernel`（基于 Microsoft Agent Framework）用于智能规划，把文件权限、并发保护、事务写入、构建测试和失败回滚留在本地 Harness 中。

## 核心原则

- 默认 dry-run；只有 `--apply` 才写入工作区。
- Agent 只有 `list_files`、`read_file`、`search_text` 三个只读工具。
- 模型返回结构化文件操作，不能返回或执行任意 shell 命令。
- 修改已有文件时必须携带读取时的 SHA-256。
- 验证只允许 `dotnet build` / `dotnet test`，默认带 `--no-restore`。
- 验证失败自动回滚；脏文件、符号链接和密钥材料默认拒绝修改。

## 构建与安装

```bash
dotnet build tools/Senparc.Weixin.CLI/Senparc.Weixin.CLI.csproj
dotnet pack tools/Senparc.Weixin.CLI/Senparc.Weixin.CLI.csproj -o artifacts/packages
dotnet tool install --global --add-source artifacts/packages Senparc.Weixin.CLI --version 1.0.0-preview.1
```

历史名称 `WeChatCLI` 作为同一工具的别名概念保留，不再维护重复源码；新的命令名统一为 `weixin`。

## 只读审计

```bash
weixin inspect --workspace .
```

结果只包含项目/解决方案路径、源码数量、分支和脏状态，不会上传文件内容。

## AgentKernel / MAF Harness

`appsettings.example.json` 默认使用 NeuCharAI，且只包含安全占位值。可在同一目录创建被 Git 忽略的 `appsettings.Development.json` 保存本地凭据：当 `DOTNET_ENVIRONMENT` 或 `ASPNETCORE_ENVIRONMENT` 为 `Development` 时，该文件会覆盖基础配置；未显式设置环境且文件存在时也按本地开发配置优先。显式设置为 `Production`、`Staging` 等其他环境时不会加载开发配置。

配置仍可用 `WEIXIN_HARNESS_` 前缀环境变量覆盖，嵌套键使用双下划线；环境变量的优先级最高。

先生成并审查计划：

```bash
weixin harness \
  --workspace . \
  --goal "为公众号用户接口增加可取消的实例客户端方法和测试" \
  --ai-config /private/path/appsettings.json \
  --plan-output /tmp/weixin-plan.json
```

确认计划后执行并验证：

```bash
weixin harness \
  --workspace . \
  --goal "为公众号用户接口增加可取消的实例客户端方法和测试" \
  --plan-input /tmp/weixin-plan.json \
  --apply \
  --build src/Senparc.Weixin.MP/Senparc.Weixin.MP/Senparc.Weixin.MP.csproj \
  --test src/Senparc.Weixin.MP/Senparc.Weixin.MP.Test/Senparc.Weixin.MP.Test.csproj \
  --framework net10.0 \
  --report artifacts/weixin-harness/latest.json
```

若目标文件原本已有未提交改动，必须显式增加 `--allow-dirty`，SHA-256 保护仍然有效。删除操作还需要 `--allow-delete`。

## 外部 Agent 协议

不使用 AgentKernel 时，可以接入任意 MAF、Codex 或内部 Agent 进程：

```bash
weixin harness \
  --goal "增加一个接口" \
  --agent-command /path/to/agent \
  --agent-arg --json
```

CLI 向进程标准输入写入单个 UTF-8 JSON `AgentRequest`，进程必须向标准输出写入单个 `HarnessPlan`。协议版本当前为 `1`。标准输出不能夹杂日志；日志应写到标准错误。

外部进程是显式信任边界：CLI 能约束并验证它返回的文件计划，但不能沙箱一个由用户主动启动、且本身拥有工作区权限的可执行文件。需要强隔离时，应在容器或受限账户中运行该进程。

计划示例：

```json
{
  "schemaVersion": 1,
  "goal": "增加一个接口",
  "summary": "增加实现和测试",
  "operations": [
    {
      "kind": "replace",
      "path": "src/Feature.cs",
      "replacements": [
        {
          "oldText": "exact old text",
          "newText": "exact new text",
          "expectedOccurrences": 1
        }
      ],
      "expectedSha256": "sha256-from-read-file"
    },
    {
      "kind": "write",
      "path": "tests/FeatureTests.cs",
      "content": "complete UTF-8 content"
    }
  ]
}
```

## 策略清单

通过 `--manifest harness.json` 设置允许根目录、排除目录、文件大小、操作数量和固定验证步骤。参考 `harness.example.json`。

## 独立验证

```bash
weixin self-test
weixin verify --workspace . --build path/to/project.csproj --framework net10.0
```

`self-test` 在临时目录验证 dry-run、替换、新文件、路径穿越防护、密钥防护、哈希并发保护和失败回滚，不访问网络或真实工作区。

## 当前边界

- AgentKernel 规划需要用户自行配置可用模型，仓库不保存 API Key。
- 本实现不依赖当前仍为骨架的 `Senparc.Weixin.MCP.Server`，避免提前耦合 P2-25；后续可在 `IHarnessPlanner` 后增加 MCP/A2A 规划器。
- 自动验证面向 .NET 项目；前端或微信开发者工具验证可通过未来新增的受控验证类型扩展，不能由 Agent 注入命令。
