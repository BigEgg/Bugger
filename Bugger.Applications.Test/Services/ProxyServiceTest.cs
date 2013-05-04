using BigEgg.Framework.UnitTesting;
using Bugger.Applications.Services;
using Bugger.Proxys;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bugger.Applications.Test.Services
{
    [TestClass]
    public class ProxyServiceTest : TestClassBase
    {
        [TestMethod]
        public void GeneralProxyServiceTest()
        {
            IEnumerable<ISourceControlProxy> proxys = Container.GetExportedValues<ISourceControlProxy>();
            IProxyService proxyService = new ProxyService(proxys);

            Assert.AreEqual(proxys.Count(), proxyService.Proxys.Count());
            Assert.IsNull(proxyService.ActiveProxy);
        }

        [TestMethod]
        public void PropertiesWithNotification()
        {
            IEnumerable<ISourceControlProxy> proxys = Container.GetExportedValues<ISourceControlProxy>();
            IProxyService proxyService = new ProxyService(proxys);

            AssertHelper.PropertyChangedEvent(proxyService, x => x.ActiveProxy, () =>
                proxyService.ActiveProxy = proxys.First()
            );
            Assert.AreEqual(proxys.First(), proxyService.ActiveProxy);
        }
    }
}
