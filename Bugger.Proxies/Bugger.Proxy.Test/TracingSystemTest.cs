using BigEgg.Framework.UnitTesting;
using Bugger.Domain.Models;
using Bugger.Proxy.Models;
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
            AssertHelper.ExpectedException<ArgumentNullException>(() => new MockTracingSystemProxy("  "));
            AssertHelper.ExpectedException<ArgumentNullException>(() => new MockTracingSystemProxy(null));
        }

        [TestMethod]
        public void CheckBaseImplementation()
        {
            MockTracingSystemProxy proxy = new MockTracingSystemProxy("proxyName");

            AssertHelper.ExpectedException<ArgumentException>(() => proxy.Query("   "));
            AssertHelper.ExpectedException<ArgumentException>(() => proxy.Query(string.Empty));
            AssertHelper.ExpectedException<NotSupportedException>(() => proxy.Query("BigEgg"));

            AssertHelper.ExpectedException<NotSupportedException>(() =>
                proxy.Query(new List<string>()));
            AssertHelper.ExpectedException<NotSupportedException>(() =>
                proxy.Query(new List<string>() { "BigEggg" }));

            AssertHelper.ExpectedException<NotImplementedException>(() =>
                proxy.CallQueryCore(new List<string>() { "BigEggg" }, true));
            AssertHelper.ExpectedException<NotImplementedException>(() =>
                proxy.CallQueryCore(new List<string>() { "BigEggg" }, false));
        }

        [TestMethod]
        public void GeneralSourceControllerTest()
        {
            MockTracingSystemProxy proxy = new MockTracingSystemProxy("proxyName");

            Assert.AreEqual("proxyName", proxy.ProxyName);
            Assert.AreEqual("proxyNameBugViewTemplateName", proxy.BugViewTemplateName);

            Assert.AreEqual(0, proxy.StateValues.Count);
            Assert.IsFalse(proxy.CanQuery);
        }

        [TestMethod]
        public void PropertiesWithNotification()
        {
            MockTracingSystemProxy proxy = new MockTracingSystemProxy("proxyName");

            Assert.IsFalse(proxy.CanQuery);
            AssertHelper.PropertyChangedEvent(proxy, x => x.CanQuery, () => proxy.CanQueryValue = true);
            Assert.IsTrue(proxy.CanQuery);
        }

        [TestMethod]
        public void QueryTest()
        {
            MockTracingSystemProxy proxy = new MockTracingSystemProxy("proxyName");

            proxy.CanQueryValue = true;

            ReadOnlyCollection<IBug> bugs = proxy.Query(new List<string>());
            Assert.AreEqual(0, bugs.Count);
        }

        [TestMethod]
        public void ProxyInitializeTest()
        {
            MockTracingSystemProxy proxy = new MockTracingSystemProxy("proxyName");

            Assert.IsFalse(proxy.IsInitialized);
            proxy.Initialize();
            Assert.IsTrue(proxy.IsInitialized);
        }

        [TestMethod]
        public void ProxyValidateSettingDialogTest()
        {
            MockTracingSystemProxy proxy = new MockTracingSystemProxy("proxyName");

            var result = proxy.ValidateBeforeCloseSettingDialog();
            Assert.AreEqual(SettingDialogValidateionResult.Valid, result);
        }

        private class MockTracingSystemProxy : TracingSystemProxyBase
        {
            #region Fields
            private ObservableCollection<string> statsValues;
            #endregion

            public MockTracingSystemProxy(string proxyName)
                : base(proxyName)
            {
                statsValues = new ObservableCollection<string>();
            }

            #region Properties
            public bool CanQueryValue
            {
                get { return this.CanQuery; }
                set { this.CanQuery = value; }
            }

            public Action OnInitializeAction { get; set; }

            public override ObservableCollection<string> StateValues { get { return this.statsValues; } }
            #endregion

            #region Methods
            protected override void OnInitialize()
            {
                if (OnInitializeAction != null)
                {
                    OnInitializeAction.Invoke();
                }
            }

            public ReadOnlyCollection<IBug> CallQueryCore(List<string> userNames, bool isFilterCreatedBy)
            {
                return base.QueryCore(userNames, isFilterCreatedBy);
            }
            #endregion
        }
    }
}
