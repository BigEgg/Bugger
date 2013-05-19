using Bugger.Applications.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Bugger.Applications.Test.Controllers
{
    [TestClass]
    public class ProxyControllerTest : TestClassBase
    {
        [TestMethod]
        public void ProxyServiceTest()
        {
            ProxyController controller = Container.GetExportedValue<ProxyController>();
            controller.Initialize();

            Assert.IsNotNull(controller.ProxyService);
            Assert.IsTrue(controller.ProxyService.Proxys.Any());
            Assert.IsNotNull(controller.ProxyService.ActiveProxy);
        }
    }
}
