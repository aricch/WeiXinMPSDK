/*----------------------------------------------------------------
    Copyright (C) 2026 Senparc

    文件名：WeixinTraceSecurity.cs
    文件功能描述：微信日志敏感信息脱敏与正文限长工具


    创建标识：Senparc - 20260717

    修改标识：Senparc - 20260718
    修改描述：v6.24.0 新增微信日志敏感信息集中脱敏工具

----------------------------------------------------------------*/

using System;
using System.Text.RegularExpressions;

namespace Senparc.Weixin
{
    /// <summary>
    /// 微信日志安全选项。
    /// </summary>
    public static class WeixinTraceSecurityOptions
    {
        private static int _maximumBodyLength = 4096;

        /// <summary>
        /// 请求或响应正文写入日志的最大字符数，默认为 4096。
        /// </summary>
        public static int MaximumBodyLength
        {
            get => _maximumBodyLength;
            set => _maximumBodyLength = value > 0
                ? value
                : throw new ArgumentOutOfRangeException(nameof(value));
        }
    }

    /// <summary>
    /// 对 URL、JSON、表单和 Authorization 文本进行集中脱敏。
    /// </summary>
    public static class WeixinTraceRedactor
    {
        private const string SensitiveNames =
            "access_token|refresh_token|authorizer_access_token|component_access_token|suite_access_token|" +
            "secret|appsecret|app_secret|corpsecret|corp_secret|mchkey|api_key|private_key|password|" +
            "openid|unionid|phone|mobile|id_card|identity_card|bank_card|account_number";

        private static readonly Regex QueryOrFormRegex = new Regex(
            "(?i)([?&;](?:" + SensitiveNames + ")=)[^&#;\\s]*",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private static readonly Regex JsonRegex = new Regex(
            "(?i)(\\\"(?:" + SensitiveNames + ")\\\"\\s*:\\s*)(\\\"(?:\\\\.|[^\\\"\\\\])*\\\"|[^,}\\s]+)",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private static readonly Regex KeyValueRegex = new Regex(
            "(?i)(\\b(?:" + SensitiveNames + ")\\s*[:=]\\s*)[^,;\\s]+",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private static readonly Regex AuthorizationRegex = new Regex(
            "(?i)(authorization\\s*[:=]\\s*)[^\\r\\n]+",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private static readonly Regex PemRegex = new Regex(
            "-----BEGIN [^-]+-----[\\s\\S]*?-----END [^-]+-----",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public static string Redact(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            var redacted = QueryOrFormRegex.Replace(value, "$1***");
            redacted = JsonRegex.Replace(redacted, "$1\"***\"");
            redacted = KeyValueRegex.Replace(redacted, "$1***");
            redacted = AuthorizationRegex.Replace(redacted, "$1***");
            return PemRegex.Replace(redacted, "***PEM REDACTED***");
        }

        public static string RedactAndTruncate(string value, int? maximumLength = null)
        {
            var redacted = Redact(value);
            if (redacted == null)
            {
                return null;
            }

            var limit = maximumLength ?? WeixinTraceSecurityOptions.MaximumBodyLength;
            return redacted.Length <= limit
                ? redacted
                : redacted.Substring(0, limit) + $"…[truncated, original length: {redacted.Length}]";
        }
    }
}
