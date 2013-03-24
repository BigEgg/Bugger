using BigEgg.Framework.UnitTesting;
using Bugger.Proxys.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net;

namespace Bugger.Proxys.Test
{
    [TestClass]
    public class SourceControllerTest
    {
        [TestMethod]
        public void ContructorTest()
        {
            AssertHelper.ExpectedException<ArgumentException>(() => new MockSourceController("  ", 123, "VDIR"));
            AssertHelper.ExpectedException<ArgumentException>(() => new MockSourceController(null, 123, "VDIR"));
            AssertHelper.ExpectedException<ArgumentException>(() => new MockSourceController("serverName", 123, "  "));
            AssertHelper.ExpectedException<ArgumentException>(() => new MockSourceController("serverName", 123, null));
            AssertHelper.ExpectedException<ArgumentException>(() => new MockSourceController("!serverName!", 123, "VDIR"));
        }

        [TestMethod]
        public void CheckBaseImplementation()
        {
            MockSourceController controller = new MockSourceController("serverName", 123, "VDIR");

            AssertHelper.ExpectedException<ArgumentNullException>(() => controller.Query(null, "   ", string.Empty));
            AssertHelper.ExpectedException<ArgumentException>(() =>
                controller.Query(new NetworkCredential("BigEggg", "SomePasswork"), null, string.Empty));
            AssertHelper.ExpectedException<NotSupportedException>(() =>
                controller.Query(new NetworkCredential("BigEggg", "SomePasswork"), "BigEgg", string.Empty));

            AssertHelper.ExpectedException<NotSupportedException>(() =>
                controller.CallQueryCore(new NetworkCredential("BigEggg", "SomePasswork"), "BigEgg", string.Empty));
        }

        [TestMethod]
        public void GeneralSourceControllerTest()
        {
            MockSourceController controller = new MockSourceController("serverName", 123, "VDIR");

            Assert.AreEqual("http://servername:123/VDIR", controller.ConnectUri.AbsoluteUri);
            Assert.IsTrue(controller.IsFilterCreatedByFilter);
        }

        [TestMethod]
        public void SetConnectUriTest()
        {
            MockSourceController controller = new MockSourceController("serverName", 123, "VDIR");

            AssertHelper.ExpectedException<ArgumentException>(() => controller.SetConnectUri("  ", 123, "VDIR"));
            AssertHelper.ExpectedException<ArgumentException>(() => controller.SetConnectUri(null, 123, "VDIR"));
            AssertHelper.ExpectedException<ArgumentException>(() => controller.SetConnectUri("serverName", 123, "  "));
            AssertHelper.ExpectedException<ArgumentException>(() => controller.SetConnectUri("serverName", 123, null));
            AssertHelper.ExpectedException<ArgumentException>(() => controller.SetConnectUri("!serverName!", 123, "VDIR"));

            controller.SetConnectUri("https://tfs.codeplex.com", 443, "tfs/TFS12");
            Assert.AreEqual("https://tfs.codeplex.com/tfs/TFS12", controller.ConnectUri.AbsoluteUri);
            controller.SetConnectUri("http://tfs.codeplex.com", 443, "tfs/TFS12");
            Assert.AreEqual("http://tfs.codeplex.com:443/tfs/TFS12", controller.ConnectUri.AbsoluteUri);
        }

        private class MockSourceController : SourceController
        {
            public MockSourceController(string serverName, uint port, string virtualPath)
                : base(serverName, port, virtualPath)
            {
            }


            public List<Bug> CallQueryCore(NetworkCredential credential, string userName, string workItemFilter)
            {
                return base.QueryCore(credential, userName, workItemFilter);
            }
        }
    }
}
