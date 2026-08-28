using Microsoft.VisualStudio.TestTools.UnitTesting;
using Senparc.Weixin.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Senparc.Weixin.TenPayV3.Test
{
    [TestClass]
    public class TenPayV3UtilTests
    {
        [TestMethod]
        public void BuildRandomStrReturnsRequestedNumberOfDigitsTest()
        {
            foreach (var length in new[] { 0, 1, 6, 100 })
            {
                var value = TenPayV3Util.BuildRandomStr(length);

                Assert.AreEqual(length, value.Length);
                Assert.IsTrue(value.All(char.IsDigit));
            }

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => TenPayV3Util.BuildRandomStr(-1));
        }

        [TestMethod]
        public void TenPayV3InfoCollectionSupportsConcurrentWritesTest()
        {
            var collection = new TenPayV3InfoCollection();

            Parallel.For(0, 500, index =>
            {
                collection[index.ToString()] = new TenPayV3Info(new SenparcWeixinSettingItem
                {
                    EncryptionType = CertType.RSA
                });
            });

            Assert.AreEqual(500, collection.Count);
        }
    }
}
