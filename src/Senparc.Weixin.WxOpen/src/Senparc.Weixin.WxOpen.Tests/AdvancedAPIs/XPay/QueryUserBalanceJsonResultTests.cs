#region Apache License Version 2.0
/*----------------------------------------------------------------

Copyright 2026 Jeffrey Su & Suzhou Senparc Network Technology Co.,Ltd.

Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file
except in compliance with the License. You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software distributed under the
License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
either express or implied. See the License for the specific language governing permissions
and limitations under the License.

Detail: https://github.com/JeffreySu/WeiXinMPSDK/blob/master/license.md

----------------------------------------------------------------*/
#endregion Apache License Version 2.0

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Senparc.Weixin.WxOpen.AdvancedAPIs.XPay;

namespace Senparc.Weixin.WxOpen.Tests.AdvancedAPIs.XPay
{
    [TestClass]
    public class QueryUserBalanceJsonResultTests
    {
        [TestMethod]
        public void Deserialize_WithBooleanTrueFirstSaveFlag_ShouldBeTrue()
        {
            var json = "{\"errcode\":0,\"errmsg\":\"ok\",\"balance\":10,\"present_balance\":0,\"sum_save\":0,\"sum_present\":0,\"sum_balance\":0,\"sum_cost\":0,\"first_save_flag\":true}";

            var result = JsonConvert.DeserializeObject<QueryUserBalanceJsonResult>(json);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.first_save_flag);
            Assert.IsTrue(result.FirstSaveFlag);
        }

        [TestMethod]
        public void Deserialize_WithBooleanFalseFirstSaveFlag_ShouldBeFalse()
        {
            var json = "{\"errcode\":0,\"errmsg\":\"ok\",\"balance\":10,\"present_balance\":0,\"sum_save\":0,\"sum_present\":0,\"sum_balance\":0,\"sum_cost\":0,\"first_save_flag\":false}";

            var result = JsonConvert.DeserializeObject<QueryUserBalanceJsonResult>(json);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.first_save_flag);
            Assert.IsFalse(result.FirstSaveFlag);
        }
    }
}
