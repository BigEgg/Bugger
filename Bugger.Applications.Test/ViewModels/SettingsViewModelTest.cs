using BigEgg.Framework.Foundation;
using BigEgg.Framework.UnitTesting;
using Bugger.Applications.Services;
using Bugger.Applications.ViewModels;
using Bugger.Applications.Views;
using Bugger.Proxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Bugger.Applications.Test.ViewModels
{
    [TestClass]
    public class SettingsViewModelTest : TestClassBase
    {
        private IProxyService proxyService;
        private SettingsViewModel viewModel;

        private string activeProxy = "Fake";
        private string teamMembersString = "BigEgg; Pupil";
        private string userName = "BigEgg";
        private string filterStatusValues = "High; Low";
        private int refreshMinutes = 60;
        private bool isFilterCreatedBy = false;

        protected override void OnTestInitialize()
        {
            this.proxyService = new ProxyService(Container.GetExportedValues<ITracingSystemProxy>());
            this.proxyService.ActiveProxy = this.proxyService.Proxys.First(x => x.ProxyName == activeProxy);
            foreach (var proxy in this.proxyService.Proxys)
            {
                proxy.Initialize();
            }

            ISettingsView view = Container.GetExportedValue<ISettingsView>();
            this.viewModel = new SettingsViewModel(view, proxyService, teamMembersString);

            this.viewModel.UserName = this.userName;
            this.viewModel.RefreshMinutes = this.refreshMinutes;
            this.viewModel.IsFilterCreatedBy = this.isFilterCreatedBy;
            this.viewModel.FilterStatusValues = this.filterStatusValues;
        }

        [TestMethod]
        public void SettingsViewModelGeneralTest()
        {
            Assert.AreEqual(proxyService.ActiveProxy.ProxyName, this.viewModel.ActiveProxy);
            Assert.AreEqual(proxyService.Proxys.Count(), this.viewModel.Proxys.Count);
            Assert.AreEqual(2, this.viewModel.TeamMembers.Count);
            Assert.AreEqual(this.teamMembersString, this.viewModel.TeamMembersString);
            Assert.IsNotNull(this.viewModel.AddNewTeamMemberCommand);
            Assert.IsNotNull(this.viewModel.RemoveTeamMemberCommand);
            Assert.AreEqual(this.userName, this.viewModel.UserName);
            Assert.AreEqual(this.refreshMinutes, this.viewModel.RefreshMinutes);
            Assert.IsFalse(this.viewModel.IsFilterCreatedBy);
            Assert.AreEqual(this.filterStatusValues, this.viewModel.FilterStatusValues);

            Assert.AreEqual(string.Empty, viewModel.Validate());

            Assert.AreEqual(string.Empty, this.viewModel.NewTeamMember);
            Assert.AreNotEqual(string.Empty, this.viewModel.SelectedTeamMember);
            Assert.AreEqual(1, this.viewModel.SelectedTeamMembers.Count);
            Assert.IsTrue(this.viewModel.RemoveTeamMemberCommand.CanExecute(null));
            Assert.IsFalse(this.viewModel.AddNewTeamMemberCommand.CanExecute(null));
        }

        [TestMethod]
        public void PropertiesWithNotification()
        {
            AssertHelper.PropertyChangedEvent(this.viewModel, x => x.SelectedTeamMember, () =>
                this.viewModel.SelectedTeamMember = this.viewModel.TeamMembers.First(x => x != "BigEgg")
            );
            Assert.AreEqual("Pupil", this.viewModel.SelectedTeamMember);

            AssertHelper.PropertyChangedEvent(this.viewModel, x => x.ActiveProxy, () =>
                this.viewModel.ActiveProxy = this.proxyService.Proxys.First(x => x.ProxyName != activeProxy).ProxyName
            );
            Assert.AreEqual(this.proxyService.Proxys.First(x => x.ProxyName != activeProxy).ProxyName, this.viewModel.ActiveProxy);

            AssertHelper.PropertyChangedEvent(this.viewModel, x => x.UserName, () =>
                this.viewModel.UserName = "Pupil"
            );
            Assert.AreEqual("Pupil", this.viewModel.UserName);

            AssertHelper.PropertyChangedEvent(this.viewModel, x => x.NewTeamMember, () =>
                this.viewModel.NewTeamMember = "Senior"
            );
            Assert.AreEqual("Senior", this.viewModel.NewTeamMember);

            AssertHelper.PropertyChangedEvent(this.viewModel, x => x.RefreshMinutes, () =>
                this.viewModel.RefreshMinutes = 30
            );
            Assert.AreEqual(30, this.viewModel.RefreshMinutes);

            AssertHelper.PropertyChangedEvent(this.viewModel, x => x.IsFilterCreatedBy, () =>
                this.viewModel.IsFilterCreatedBy = true
            );
            Assert.IsTrue(this.viewModel.IsFilterCreatedBy);
        }

        [TestMethod]
        public void ActiveProxyValidationTest()
        {
            Assert.AreEqual(this.activeProxy, this.viewModel.ActiveProxy);
            Assert.AreEqual(string.Empty, this.viewModel.Validate("ActiveProxy"));

            this.viewModel.ActiveProxy = "TFS";
            Assert.AreEqual("TFS", this.viewModel.ActiveProxy);
            Assert.AreEqual(string.Empty, this.viewModel.Validate("ActiveProxy"));

            this.viewModel.ActiveProxy = string.Empty;
            Assert.AreEqual(string.Empty, this.viewModel.ActiveProxy);
            Assert.AreNotEqual(string.Empty, this.viewModel.Validate("ActiveProxy"));

            this.viewModel.ActiveProxy = null;
            Assert.AreEqual(null, this.viewModel.ActiveProxy);
            Assert.AreNotEqual(string.Empty, this.viewModel.Validate("ActiveProxy"));
        }

        [TestMethod]
        public void UserNameValidationTest()
        {
            Assert.AreEqual(this.userName, this.viewModel.UserName);
            Assert.AreEqual(string.Empty, this.viewModel.Validate("UserName"));

            this.viewModel.UserName = "Senior";
            Assert.AreEqual("Senior", this.viewModel.UserName);
            Assert.AreEqual(string.Empty, this.viewModel.Validate("UserName"));

            this.viewModel.UserName = string.Empty;
            Assert.AreEqual(string.Empty, this.viewModel.UserName);
            Assert.AreNotEqual(string.Empty, this.viewModel.Validate("UserName"));

            this.viewModel.UserName = null;
            Assert.AreEqual(null, this.viewModel.UserName);
            Assert.AreNotEqual(string.Empty, this.viewModel.Validate("UserName"));
        }

        [TestMethod]
        public void RefreshMinutesValidationTest()
        {
            Assert.AreEqual(this.refreshMinutes, this.viewModel.RefreshMinutes);
            Assert.AreEqual(string.Empty, viewModel.Validate("RefreshMinutes"));

            this.viewModel.RefreshMinutes = 1;
            Assert.AreEqual(1, this.viewModel.RefreshMinutes);
            Assert.AreEqual(string.Empty, this.viewModel.Validate("RefreshMinutes"));

            this.viewModel.RefreshMinutes = 100;
            Assert.AreEqual(100, this.viewModel.RefreshMinutes);
            Assert.AreEqual(string.Empty, this.viewModel.Validate("RefreshMinutes"));

            this.viewModel.RefreshMinutes = 720;
            Assert.AreEqual(720, this.viewModel.RefreshMinutes);
            Assert.AreEqual(string.Empty, this.viewModel.Validate("RefreshMinutes"));

            this.viewModel.RefreshMinutes = 0;
            Assert.AreEqual(0, this.viewModel.RefreshMinutes);
            Assert.AreNotEqual(string.Empty, this.viewModel.Validate("RefreshMinutes"));

            this.viewModel.RefreshMinutes = 721;
            Assert.AreEqual(721, this.viewModel.RefreshMinutes);
            Assert.AreNotEqual(string.Empty, this.viewModel.Validate("RefreshMinutes"));
        }

        [TestMethod]
        public void AddNewTeamMemberCommandTest()
        {
            Assert.AreEqual(2, this.viewModel.TeamMembers.Count);
            Assert.AreEqual(1, this.viewModel.SelectedTeamMembers.Count);
            Assert.AreEqual("BigEgg", this.viewModel.SelectedTeamMembers[0]);
            Assert.AreEqual("BigEgg", this.viewModel.SelectedTeamMember);

            Assert.AreEqual("", this.viewModel.NewTeamMember);
            Assert.IsFalse(this.viewModel.AddNewTeamMemberCommand.CanExecute(null));

            this.viewModel.NewTeamMember = "NewMember";
            Assert.IsTrue(this.viewModel.AddNewTeamMemberCommand.CanExecute(null));

            this.viewModel.AddNewTeamMemberCommand.Execute(null);
            Assert.AreEqual("", this.viewModel.NewTeamMember);
            Assert.AreEqual(3, this.viewModel.TeamMembers.Count);
            Assert.AreEqual(1, this.viewModel.SelectedTeamMembers.Count);
            Assert.AreEqual("NewMember", this.viewModel.SelectedTeamMembers[0]);
            Assert.AreEqual("NewMember", this.viewModel.SelectedTeamMember);
        }

        [TestMethod]
        public void SettingDialogViewModelRemoveBranchCommandsTest()
        {
            Assert.AreEqual(2, this.viewModel.TeamMembers.Count);
            Assert.AreEqual(1, this.viewModel.SelectedTeamMembers.Count);
            Assert.AreEqual("BigEgg", this.viewModel.SelectedTeamMembers[0]);
            Assert.AreEqual("BigEgg", this.viewModel.SelectedTeamMember);

            Assert.IsTrue(this.viewModel.RemoveTeamMemberCommand.CanExecute(null));

            this.viewModel.RemoveTeamMemberCommand.Execute(null);
            Assert.AreEqual(1, this.viewModel.TeamMembers.Count);
            Assert.AreEqual(1, this.viewModel.SelectedTeamMembers.Count);
            Assert.AreEqual("Pupil", this.viewModel.SelectedTeamMembers[0]);
            Assert.AreEqual("Pupil", this.viewModel.SelectedTeamMember);

            this.viewModel.NewTeamMember = "NewMember";
            this.viewModel.AddNewTeamMemberCommand.Execute(null);

            viewModel.SelectedTeamMembers.Add("Pupil");
            this.viewModel.RemoveTeamMemberCommand.Execute(null);
            Assert.AreEqual(0, this.viewModel.TeamMembers.Count);
            Assert.AreEqual(0, this.viewModel.SelectedTeamMembers.Count);

            Assert.IsFalse(this.viewModel.RemoveTeamMemberCommand.CanExecute(null));
        }
    }
}
