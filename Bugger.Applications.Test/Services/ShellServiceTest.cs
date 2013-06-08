using BigEgg.Framework.UnitTesting;
using Bugger.Applications.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bugger.Applications.Test.Services
{
    [TestClass]
    public class ShellServiceTest
    {
        [TestMethod]
        public void SetViewTest()
        {
            ShellService shellService = new ShellService();
            object mockView = new object();

            AssertHelper.PropertyChangedEvent(shellService, x => x.MainView, () =>
                shellService.MainView = mockView);
            Assert.AreEqual(mockView, shellService.MainView);

            AssertHelper.PropertyChangedEvent(shellService, x => x.UserBugsView, () =>
                shellService.UserBugsView = mockView);
            Assert.AreEqual(mockView, shellService.UserBugsView);

            AssertHelper.PropertyChangedEvent(shellService, x => x.TeamBugsView, () =>
                shellService.TeamBugsView = mockView);
            Assert.AreEqual(mockView, shellService.TeamBugsView);
        }
    }
}
