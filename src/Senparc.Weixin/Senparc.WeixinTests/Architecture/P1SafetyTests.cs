using Microsoft.VisualStudio.TestTools.UnitTesting;
using Senparc.Weixin;
using Senparc.Weixin.Containers;
using Senparc.Weixin.Containers.Tests;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.MessageContexts;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Senparc.WeixinTests.Architecture
{
    [TestClass]
    public class P1SafetyTests
    {
        [TestMethod]
        public void TraceRedactorRedactsAndTruncatesSecrets()
        {
            var input = "https://api.weixin.qq.com/path?access_token=token123&openid=user123 " +
                        "{\"appsecret\":\"secret123\",\"mobile\":\"13800138000\",\"safe\":\"ok\"}";

            var result = WeixinTraceRedactor.RedactAndTruncate(input, 200);

            Assert.IsFalse(result.Contains("token123"));
            Assert.IsFalse(result.Contains("user123"));
            Assert.IsFalse(result.Contains("secret123"));
            Assert.IsFalse(result.Contains("13800138000"));
            StringAssert.Contains(result, "\"safe\":\"ok\"");

            var truncated = WeixinTraceRedactor.RedactAndTruncate(new string('a', 100), 10);
            StringAssert.StartsWith(truncated, "aaaaaaaaaa…[truncated");
        }

        [TestMethod]
        public void RegistrationCollectionEnforcesCapacityAndAllowsReplacement()
        {
            var registrations = new BaseContainerRegisterFuncCollection<TestContainerBag1>
            {
                MaximumCount = 1
            };

            registrations["APP"] = () => Task.FromResult(new TestContainerBag1());
            registrations["app"] = () => Task.FromResult(new TestContainerBag1());

            Assert.AreEqual(1, registrations.Count);
            Assert.ThrowsException<InvalidOperationException>(() =>
                registrations["other"] = () => Task.FromResult(new TestContainerBag1()));
        }

        [TestMethod]
        public void PublishedContainerMetadataSignaturesRemainCompatible()
        {
            var registrationProperty = typeof(BaseContainer<TestContainerBag1>).GetProperty(
                "RegisterFuncCollection",
                BindingFlags.Static | BindingFlags.NonPublic);

            Assert.IsNotNull(registrationProperty);
            Assert.AreEqual(
                typeof(ConcurrentDictionary<string, Func<Task<TestContainerBag1>>>),
                registrationProperty.PropertyType);
            Assert.AreEqual(
                typeof(Dictionary<string, Func<Task<TestContainerBag1>>>),
                typeof(BaseContainerRegisterFuncCollection<TestContainerBag1>).BaseType);
        }

        [TestMethod]
        public async Task UnifiedErrorModelRethrowsCancellation()
        {
            using var cancellation = new CancellationTokenSource();
            cancellation.Cancel();

            await Assert.ThrowsExceptionAsync<OperationCanceledException>(() =>
                WeixinApiExecutor.ExecuteAsync<int>(token => Task.FromResult(1), cancellation.Token));
        }

        [TestMethod]
        public void MessageContextCompatibilityAndSafeDefaultAreExplicit()
        {
            var originalDefault = MessageContextSafetyOptions.DefaultMaxRecordCount;
            var originalMaximum = MessageContextSafetyOptions.MaximumRecordCount;
            try
            {
                MessageContextSafetyOptions.MaximumRecordCount = 100;
                MessageContextSafetyOptions.DefaultMaxRecordCount = 25;

                Assert.AreEqual(0, MessageContextSafetyOptions.ResolveMaxRecordCount(0));
                Assert.AreEqual(25, MessageContextSafetyOptions.ResolveMaxRecordCount(MessageContextSafetyOptions.DefaultRecordCount));
                Assert.AreEqual(1000, MessageContextSafetyOptions.ResolveMaxRecordCount(1000));
                Assert.AreEqual(0, MessageContextSafetyOptions.ResolveMaxRecordCount(MessageContextSafetyOptions.UnlimitedRecordCount));
            }
            finally
            {
                MessageContextSafetyOptions.MaximumRecordCount = Math.Max(originalMaximum, originalDefault);
                MessageContextSafetyOptions.DefaultMaxRecordCount = originalDefault;
                MessageContextSafetyOptions.MaximumRecordCount = originalMaximum;
            }
        }
    }
}
