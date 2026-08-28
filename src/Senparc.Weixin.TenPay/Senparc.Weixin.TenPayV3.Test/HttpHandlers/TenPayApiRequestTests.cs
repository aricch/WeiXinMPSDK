using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Senparc.Weixin.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Senparc.Weixin.TenPayV3.Test.net6.HttpHandlers
{
    [TestClass]
    public class TenPayApiRequestTests
    {
        [TestMethod]
        public void SetHeaderTest()
        {
            var request = new TenPayApiRequest();
            using HttpClient client = new HttpClient();
            request.SetHeader(client);
            Console.WriteLine(client.DefaultRequestHeaders.Accept.ToString());
            Console.WriteLine(client.DefaultRequestHeaders.UserAgent.ToString());

            UserAgentValues userAgentValues = UserAgentValues.Instance;
            Assert.AreEqual("application/json, */*", client.DefaultRequestHeaders.Accept.ToString());
            Assert.AreEqual($"Senparc.Weixin.TenPayV3-C#/{userAgentValues.TenPayV3Version} (Senparc.Weixin {userAgentValues.SenparcWeixinVersion}) .NET/{userAgentValues.RuntimeVersion} ({userAgentValues.OSVersion})", client.DefaultRequestHeaders.UserAgent.ToString());
        }

        [TestMethod]
        public void ReusesHttpClientForSameSettingTest()
        {
            var setting = new SenparcWeixinSettingItem
            {
                EncryptionType = CertType.RSA
            };
            var firstRequest = new TenPayApiRequest(setting);
            var secondRequest = new TenPayApiRequest(setting);
            var clientField = typeof(TenPayApiRequest).GetField("_client", BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.IsNotNull(clientField);
            Assert.AreSame(clientField.GetValue(firstRequest), clientField.GetValue(secondRequest));
        }
    }
}
