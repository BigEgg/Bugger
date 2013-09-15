using BigEgg.Framework.Applications.ViewModels;
using BigEgg.Framework.UnitTesting;
using Bugger.Domain.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Bugger.Proxy.Test
{
    [TestClass]
    public class TracingSystemTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            AssertHelper.ExpectedException<ArgumentException>(() => new MockSourceController("  "));
            AssertHelper.ExpectedException<ArgumentException>(() => new MockSourceController(null));
        }

        [TestMethod]
        public void CheckBaseImplementation()
        {
            MockSourceController controller = new MockSourceController("proxyName");

            AssertHelper.ExpectedException<ArgumentException>(() => controller.Query("   "));
            AssertHelper.ExpectedException<ArgumentException>(() => controller.Query(string.Empty));
            AssertHelper.ExpectedException<NotSupportedException>(() => controller.Query("BigEgg"));

            AssertHelper.ExpectedException<NotSupportedException>(() =>
                controller.Query(new List<string>()));
            AssertHelper.ExpectedException<NotSupportedException>(() =>
                controller.Query(new List<string>() { "BigEggg" }));

            AssertHelper.ExpectedException<NotImplementedException>(() =>
                controller.CallQueryCore(new List<string>() { "BigEggg" }, true));
            AssertHelper.ExpectedException<NotImplementedException>(() =>
                controller.CallQueryCore(new List<string>() { "BigEggg" }, false));
        }

        [TestMethod]
        public void GeneralSourceControllerTest()
        {
            MockSourceController controller = new MockSourceController("proxyName");

            Assert.AreEqual("proxyName", controller.ProxyName);
            Assert.AreEqual(0, controller.StateValues.Count);
            Assert.IsFalse(controller.CanQuery);
        }

        [TestMethod]
        public void PropertiesWithNotification()
        {
            MockSourceController controller = new MockSourceController("proxyName");

            Assert.IsFalse(controller.CanQuery);
            AssertHelper.PropertyChangedEvent(controller, x => x.CanQuery, () => controller.CanQueryValue = true);
            Assert.IsTrue(controller.CanQuery);
        }

        [TestMethod]
        public void QueryTest()
        {
            MockSourceController controller = new MockSourceController("proxyName");

            controller.CanQueryValue = true;

            ReadOnlyCollection<Bug> bugs = controller.Query(new List<string>());
            Assert.AreEqual(0, bugs.Count);
        }

        private class MockSourceController : TracingSystemProxy
        {
            public MockSourceController(string proxyName)
                : base(proxyName)
            {
            }

            public bool CanQueryValue 
            { 
                get { return this.CanQuery; }
                set { this.CanQuery = value; }
            }

            public ReadOnlyCollection<Bug> CallQueryCore(List<string> userNames, bool isFilterCreatedBy)
            {
                return base.QueryCore(userNames, isFilterCreatedBy);
            }
        }
    }
}
