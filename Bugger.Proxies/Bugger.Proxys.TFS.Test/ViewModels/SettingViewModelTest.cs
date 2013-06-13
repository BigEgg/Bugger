using BigEgg.Framework.Applications.Views;
using BigEgg.Framework.UnitTesting;
using Bugger.Proxy.TFS.Documents;
using Bugger.Proxy.TFS.Presentation.Fake.Views;
using Bugger.Proxy.TFS.ViewModels;
using Bugger.Proxy.TFS.Views;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Bugger.Proxy.TFS.Test.ViewModels
{
    [TestClass]
    public class TFSSettingViewModelTest : TestClassBase
    {
        private TFSSettingViewModel viewModel;

        protected override void OnTestInitialize()
        {
            if (File.Exists(SettingDocumentType.FilePath))
            {
                File.Delete(SettingDocumentType.FilePath);
            }

            this.viewModel = Container.GetExportedValue<TFSSettingViewModel>();
        }

        [TestMethod]
        public void GeneralSettingViewModelTest()
        {
            Assert.IsFalse(viewModel.CanConnect);
            Assert.AreEqual(0, viewModel.TFSFields.Count);
            Assert.IsNotNull(viewModel.Settings);
            Assert.IsNotNull(viewModel.SaveCommand);
            Assert.IsNotNull(viewModel.UriHelpCommand);
            Assert.IsNotNull(viewModel.TestConnectionCommand);
        }

        [TestMethod]
        public void OpenUriHelpCommandTest()
        {
            MockUriHelpView view = Container.GetExportedValue<IUriHelpView>() as MockUriHelpView;
            view.ShowDialogAction = (x) =>
            {
                UriHelpViewModel uriHelpViewModel = x.GetViewModel<UriHelpViewModel>();
                uriHelpViewModel.CancelCommand.Execute(null);
            };
            this.viewModel.UriHelpCommand.Execute(null);
            Assert.IsNull(this.viewModel.Settings.ConnectUri);

            view.ShowDialogAction = (x) =>
                {
                    UriHelpViewModel uriHelpViewModel = x.GetViewModel<UriHelpViewModel>();
                    uriHelpViewModel.ServerName = "https://tfs.codeplex.com:443/tfs/TFS12";
                    uriHelpViewModel.SubmitCommand.Execute(null);
                };
            this.viewModel.UriHelpCommand.Execute(null);
            Assert.IsNotNull(this.viewModel.Settings.ConnectUri);
            Assert.AreEqual(
                new Uri("https://tfs.codeplex.com:443/tfs/TFS12").AbsoluteUri, 
                this.viewModel.Settings.ConnectUri.AbsoluteUri);
        }

        [TestMethod]
        public void PropertiesWithNotification()
        {
            MockUriHelpView view = Container.GetExportedValue<IUriHelpView>() as MockUriHelpView;
            view.ShowDialogAction = (x) =>
            {
                UriHelpViewModel uriHelpViewModel = x.GetViewModel<UriHelpViewModel>();
                uriHelpViewModel.ServerName = "https://tfs.codeplex.com:443/tfs/TFS12";
                uriHelpViewModel.SubmitCommand.Execute(null);
            };
            this.viewModel.UriHelpCommand.Execute(null);
            this.viewModel.Settings.UserName = "snd\\BigEgg_cp";
            this.viewModel.Settings.Password = password;
            AssertHelper.PropertyChangedEvent(this.viewModel, x => x.TFSFields, () => this.viewModel.TestConnectionCommand.Execute(null));

            this.viewModel.Settings.Password = "password";
            AssertHelper.PropertyChangedEvent(this.viewModel, x => x.CanConnect, () => this.viewModel.TestConnectionCommand.Execute(null));

            this.viewModel.Settings.Password = password;
            AssertHelper.PropertyChangedEvent(this.viewModel, x => x.CanConnect, () => this.viewModel.TestConnectionCommand.Execute(null));
        }
    }
}
