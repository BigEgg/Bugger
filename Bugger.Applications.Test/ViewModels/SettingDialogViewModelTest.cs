using Bugger.Applications.Services;
using Bugger.Applications.ViewModels;
using Bugger.Applications.Views;
using Bugger.Proxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Bugger.Applications.Test.ViewModels
{
    [TestClass]
    public class SettingDialogViewModelTest : TestClassBase
    {
        private IProxyService proxyService;
        private SettingDialogViewModel dialogViewModel;
        private SettingsViewModel settingsViewModel;

        private string activeProxy = "Fake";
        private string teamMembersString = "BigEgg; Pupil";
        private string userName = "BigEgg";
        private int refreshMinutes = 60;
        private bool isFilterCreatedBy = false;

        protected override void OnTestInitialize()
        {
            this.proxyService = new ProxyService(Container.GetExportedValues<ISourceControlProxy>());
            this.proxyService.ActiveProxy = this.proxyService.Proxys.First(x => x.ProxyName == activeProxy);
            foreach (var proxy in this.proxyService.Proxys)
            {
                proxy.Initialize();
            }

            ISettingsView settingsView = Container.GetExportedValue<ISettingsView>();
            this.settingsViewModel = new SettingsViewModel(settingsView, proxyService, teamMembersString);

            this.settingsViewModel.UserName = this.userName;
            this.settingsViewModel.RefreshMinutes = this.refreshMinutes;
            this.settingsViewModel.IsFilterCreatedBy = this.isFilterCreatedBy;

            ISettingDialogView dialogView = Container.GetExportedValue<ISettingDialogView>();
            this.dialogViewModel = new SettingDialogViewModel(dialogView, proxyService, settingsViewModel);
        }

        [TestMethod]
        public void SettingDialogViewModelGeneralTest()
        {
            Assert.AreNotEqual(string.Empty, this.dialogViewModel.Title);
            Assert.IsNotNull(this.dialogViewModel.SubmitCommand);
            Assert.IsNotNull(this.dialogViewModel.CancelCommand);
            Assert.AreEqual(1, this.dialogViewModel.Views.Count);

            Assert.IsTrue(this.dialogViewModel.SubmitCommand.CanExecute(null));
        }

        [TestMethod]
        public void SettingsViewModelValidationTest()
        {
            Assert.IsTrue(this.dialogViewModel.SubmitCommand.CanExecute(null));
            
            this.settingsViewModel.UserName = string.Empty;
            Assert.IsFalse(this.dialogViewModel.SubmitCommand.CanExecute(null));
        }

        [TestMethod]
        public void ViewsTest()
        {
            Assert.AreEqual(1, this.dialogViewModel.Views.Count);

            this.settingsViewModel.ActiveProxy = "TFS";
            Assert.AreEqual(2, this.dialogViewModel.Views.Count);

            this.settingsViewModel.ActiveProxy = this.activeProxy;
            Assert.AreEqual(1, this.dialogViewModel.Views.Count);
        }

        [TestMethod]
        public void SubmitCommandTest()
        {
            Assert.IsTrue(this.dialogViewModel.SubmitCommand.CanExecute(null));

            this.settingsViewModel.UserName = string.Empty;
            Assert.IsFalse(this.dialogViewModel.SubmitCommand.CanExecute(null));
            this.settingsViewModel.UserName = this.userName;
            Assert.IsTrue(this.dialogViewModel.SubmitCommand.CanExecute(null));

            this.settingsViewModel.ActiveProxy = "TFS";
            Assert.IsFalse(this.dialogViewModel.SubmitCommand.CanExecute(null));
            this.settingsViewModel.ActiveProxy = this.activeProxy;
            Assert.IsTrue(this.dialogViewModel.SubmitCommand.CanExecute(null));
        }
    }
}
