using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Bugger.Proxy.FakeProxy.Test
{
    [TestClass]
    public class FakeProxyTest
    {
        [TestMethod]
        public void GeneralFakeProxyTest()
        {
            FakeProxy proxy = new FakeProxy();
            Assert.IsTrue(proxy.CanQuery());
            Assert.AreEqual("Fake", proxy.ProxyName);
        }

        [TestMethod]
        public void QueryTest()
        {
            FakeProxy proxy = new FakeProxy();
            proxy.Initialize();

            var bugs = proxy.Query("bigegg");
            Assert.IsNotNull(bugs);
            Assert.AreEqual(7, bugs.Count);

            bugs = proxy.Query(new List<string> { "bigegg", "pupil" });
            Assert.IsNotNull(bugs);
            Assert.AreEqual(12, bugs.Count);
        }
    }
}
