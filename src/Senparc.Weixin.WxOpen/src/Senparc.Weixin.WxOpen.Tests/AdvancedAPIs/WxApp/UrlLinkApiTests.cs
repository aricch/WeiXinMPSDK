/*----------------------------------------------------------------
    Copyright (C) 2026 Senparc

    文件名：UrlLinkApiTests.cs
    文件功能描述：

    创建标识：Senparc - 20220106

    修改标识：Senparc - 20260523
    修改描述：补充更新日志，完善文件头修改记录

----------------------------------------------------------------*/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Senparc.CO2NET.Extensions;
using Senparc.Weixin.WxOpen.Tests;
using Senparc.Weixin.WxOpen.AdvancedAPIs.WxApp.UrlLinkJson;
using System;

namespace Senparc.Weixin.WxOpen.AdvancedAPIs.WxApp.Tests
{
    [TestClass()]
    public class UrlLinkApiTests : WxOpenBaseTest
    {
        [TestMethod()]
        public void GenerateTest()
        {
            var result = UrlLinkApi.Generate(base._wxOpenAppId);
            Console.WriteLine(result.ToJson(true));
            Assert.IsTrue(result.errcode == ReturnCode.请求成功);
            Assert.IsTrue(result.url_link != null);
            Assert.IsTrue(result.url_link.StartsWith("http"));
        }

        [TestMethod()]
        public void GenerateWithSafetyNoticeTest()
        {
            // Test with safety notice configuration
            var safetyNotice = new Generate_SafetyNotice
            {
                is_show_safety_notice = true,
                safe_url = "https://example.com/safety"
            };

            var result = UrlLinkApi.Generate(
                base._wxOpenAppId,
                path: "/pages/index",
                query: "id=123",
                safetyNotice: safetyNotice
            );

            Console.WriteLine(result.ToJson(true));
            Assert.IsTrue(result.errcode == ReturnCode.请求成功);
            Assert.IsTrue(result.url_link != null);
            Assert.IsTrue(result.url_link.StartsWith("http"));
        }

        [TestMethod()]
        public void GenerateWithCloudBaseTest()
        {
            // Test with cloud base configuration
            var cloudBase = new Generate_CloudBase
            {
                env = "test-env",
                domain = "test.example.com",
                path = "/h5/index",
                query = "from=test"
            };

            var result = UrlLinkApi.Generate(
                base._wxOpenAppId,
                cloudBase: cloudBase
            );

            Console.WriteLine(result.ToJson(true));
            Assert.IsTrue(result.errcode == ReturnCode.请求成功);
            Assert.IsTrue(result.url_link != null);
        }

        [TestMethod()]
        public void GenerateAsyncTest()
        {
            var result = UrlLinkApi.GenerateAsync(base._wxOpenAppId).GetAwaiter().GetResult();
            Console.WriteLine(result.ToJson(true));
            Assert.IsTrue(result.errcode == ReturnCode.请求成功);
            Assert.IsTrue(result.url_link != null);
            Assert.IsTrue(result.url_link.StartsWith("http"));
        }
    }
}