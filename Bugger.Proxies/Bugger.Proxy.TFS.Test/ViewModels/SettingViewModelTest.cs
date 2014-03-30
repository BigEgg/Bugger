using BigEgg.Framework.Applications.Views;
using BigEgg.Framework.UnitTesting;
using Bugger.Domain.Models;
using Bugger.Proxy.TFS.Documents;
using Bugger.Proxy.TFS.Models;
using Bugger.Proxy.TFS.Presentation.Fake.Views;
using Bugger.Proxy.TFS.ViewModels;
using Bugger.Proxy.TFS.Views;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel;
using System.IO;

namespace Bugger.Proxy.TFS.Test.ViewModels
{
    [TestClass]
    public class TFSSettingViewModelTest : TestClassBase
    {
        private TFSSettingViewModel viewModel;

        protected override void OnTestInitialize()
        {
            ITFSSettingView view = Container.GetExportedValue<ITFSSettingView>();
            IUriHelperDialogView uriHelpView = Container.GetExportedValue<IUriHelperDialogView>();

            this.viewModel = new TFSSettingViewModel(view, uriHelpView);
        }

        [TestMethod]
        public void GeneralSettingViewModelTest()
        {
            PropertyDescriptorCollection propertyDescriptorCollection = TypeDescriptor.GetProperties(typeof(IBug));

            Assert.AreEqual(0, this.viewModel.TFSFields.Count);
            Assert.AreEqual(0, this.viewModel.BugFilterFields.Count);
            Assert.AreEqual(0, this.viewModel.PriorityValues.Count);
            Assert.AreEqual(propertyDescriptorCollection.Count - 1, this.viewModel.PropertyMappingCollection.Count);

            foreach (var mappingModel in this.viewModel.PropertyMappingCollection)
            {
                Assert.AreEqual(string.Empty, mappingModel.Value);
            }

            Assert.IsNull(this.viewModel.ConnectUri);
            Assert.IsNull(this.viewModel.UserName);
            Assert.IsNull(this.viewModel.Password);
            Assert.IsTrue(string.IsNullOrEmpty(this.viewModel.BugFilterField));
            Assert.IsTrue(string.IsNullOrEmpty(this.viewModel.BugFilterValue));
            Assert.IsTrue(string.IsNullOrEmpty(this.viewModel.PriorityRed));

            Assert.IsNotNull(this.viewModel.OpenUriHelperDialogCommand);
            Assert.IsNull(this.viewModel.TestConnectionCommand);

            Assert.AreEqual(ProgressType.NotWorking, this.viewModel.ProgressType);
            Assert.AreEqual(0, this.viewModel.ProgressValue);
        }

        [TestMethod]
        public void OpenUriHelpCommandTest()
        {
            Assert.IsNull(this.viewModel.ConnectUri);

            MockUriHelperDialogView view = Container.GetExportedValue<IUriHelperDialogView>() as MockUriHelperDialogView;
            view.ShowDialogAction = (x) =>
            {
                UriHelperDialogViewModel uriHelpViewModel = x.GetViewModel<UriHelperDialogViewModel>();
                uriHelpViewModel.CancelCommand.Execute(null);
            };
            this.viewModel.OpenUriHelperDialogCommand.Execute(null);
            Assert.IsNull(this.viewModel.ConnectUri);

            view.ShowDialogAction = (x) =>
                {
                    UriHelperDialogViewModel uriHelpViewModel = x.GetViewModel<UriHelperDialogViewModel>();
                    uriHelpViewModel.ServerName = TheCodePlexUri;
                    uriHelpViewModel.SubmitCommand.Execute(null);
                };
            this.viewModel.OpenUriHelperDialogCommand.Execute(null);
            Assert.IsNotNull(this.viewModel.ConnectUri);
            Assert.AreEqual(
                new Uri(TheCodePlexUri).AbsoluteUri,
                this.viewModel.ConnectUri.AbsoluteUri);
        }

        [TestMethod]
        public void PropertiesWithNotification()
        {
            AssertHelper.PropertyChangedEvent(this.viewModel, x => x.ConnectUri, () => this.viewModel.ConnectUri = new Uri(TheCodePlexUri));
            AssertHelper.PropertyChangedEvent(this.viewModel, x => x.UserName, () => this.viewModel.UserName = "BigEgg");
            AssertHelper.PropertyChangedEvent(this.viewModel, x => x.Password, () => this.viewModel.Password = "Password");
            AssertHelper.PropertyChangedEvent(this.viewModel, x => x.BugFilterField, () => this.viewModel.BugFilterField = "Work Item Type");
            AssertHelper.PropertyChangedEvent(this.viewModel, x => x.BugFilterValue, () => this.viewModel.BugFilterValue = "Bug");
            AssertHelper.PropertyChangedEvent(this.viewModel, x => x.PriorityRed, () => this.viewModel.PriorityRed = "1;2");
            AssertHelper.PropertyChangedEvent(this.viewModel, x => x.ProgressType, () => this.viewModel.ProgressType = ProgressType.Success);
            AssertHelper.PropertyChangedEvent(this.viewModel, x => x.ProgressValue, () => this.viewModel.ProgressValue = 100);

            AssertHelper.PropertyChangedEvent(this.viewModel, x => x.PropertyMappingCollection, () => this.viewModel.PropertyMappingCollection["ID"] = "ID");
        }
    }
}
