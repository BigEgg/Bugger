﻿using BigEgg.Framework.UnitTesting;
using Bugger.Applications.Services;
using Bugger.Proxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Bugger.Applications.Test.Services
{
    [TestClass]
    public class ProxyServiceTest : TestClassBase
    {
        private IEnumerable<ITracingSystemProxy> proxys;
        private IProxyService proxyService;

        protected override void OnTestInitialize()
        {
            this.proxys = Container.GetExportedValues<ITracingSystemProxy>();
            this.proxyService = new ProxyService(proxys);
        }

        [TestMethod]
        public void GeneralProxyServiceTest()
        {
            Assert.AreEqual(this.proxys.Count(), this.proxyService.Proxies.Count());
            Assert.IsNull(this.proxyService.ActiveProxy);
        }

        [TestMethod]
        public void PropertiesWithNotification()
        {
            AssertHelper.PropertyChangedEvent(this.proxyService, x => x.ActiveProxy, () =>
                this.proxyService.ActiveProxy = this.proxys.First()
            );
            Assert.AreEqual(this.proxys.First(), this.proxyService.ActiveProxy);
        }
    }
}
