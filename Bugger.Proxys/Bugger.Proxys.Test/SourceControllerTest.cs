using BigEgg.Framework.Applications.ViewModels;
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
        }

        [TestMethod]
        public void QueryTest()
        {
            MockSourceController controller = new MockSourceController("proxyName");

            controller.CanQueryValue = true;

            List<Bug> bugs = controller.Query(new List<string>());
            Assert.AreEqual(0, bugs.Count);
        }

        private class MockSourceController : SourceControlProxy
        {
            private ViewModel settingViewModel;

            public MockSourceController(string proxyName)
                : base(proxyName)
            {
                this.CanQueryValue = false;

                settingViewModel = null; 
            }

            public override ViewModel SettingViewModel { get { return this.settingViewModel; } }

            public bool CanQueryValue { get; set; }

            public override bool CanQuery() { return CanQueryValue; }

            public List<Bug> CallQueryCore(List<string> userNames, bool isFilterCreatedBy)
            {
                return base.QueryCore(userNames, isFilterCreatedBy);
            }
        }
    }
}
