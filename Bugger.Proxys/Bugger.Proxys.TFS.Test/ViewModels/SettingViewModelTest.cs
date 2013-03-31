using BigEgg.Framework.Applications.Services;
using BigEgg.Framework.Applications.Views;
using Bugger.Proxys.TFS.Properties;
using Bugger.Proxys.TFS.Test.Services;
using Bugger.Proxys.TFS.Test.Views;
using Bugger.Proxys.TFS.ViewModels;
using Bugger.Proxys.TFS.Views;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Bugger.Proxys.TFS.Test.ViewModels
{
    [TestClass]
    public class SettingViewModelTest : TestClassBase
    {
        private SettingViewModel viewModel;
        private MockMessageService messageService;

        protected override void OnTestInitialize()
        {
            this.viewModel = Container.GetExportedValue<SettingViewModel>();
            this.messageService = Container.GetExportedValue<IMessageService>() as MockMessageService;
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
        public void CanTestConnectionCommandChangeTest()
        {
            MockUriHelpView view = Container.GetExportedValue<IUriHelpView>() as MockUriHelpView;
            view.ShowDialogAction = (x) =>
            {
                UriHelpViewModel uriHelpViewModel = x.GetViewModel<UriHelpViewModel>();
                uriHelpViewModel.CancelCommand.Execute(null);
            };
            this.viewModel.UriHelpCommand.Execute(null);
            Assert.IsFalse(viewModel.TestConnectionCommand.CanExecute(null));

            view.ShowDialogAction = (x) =>
            {
                UriHelpViewModel uriHelpViewModel = x.GetViewModel<UriHelpViewModel>();
                uriHelpViewModel.ServerName = "https://tfs.codeplex.com:443/tfs/TFS12";
                uriHelpViewModel.SubmitCommand.Execute(null);
            };
            this.viewModel.UriHelpCommand.Execute(null);
            Assert.IsTrue(viewModel.TestConnectionCommand.CanExecute(null));
        }

        [TestMethod]
        public void TestConnectionCommandTest()
        {
            MockUriHelpView view = Container.GetExportedValue<IUriHelpView>() as MockUriHelpView;
            view.ShowDialogAction = (x) =>
            {
                UriHelpViewModel uriHelpViewModel = x.GetViewModel<UriHelpViewModel>();
                uriHelpViewModel.ServerName = "https://tfs.codeplex.com:443/tfs/TFS12";
                uriHelpViewModel.SubmitCommand.Execute(null);
            };
            this.viewModel.UriHelpCommand.Execute(null);

            messageService.Clear();
            Assert.IsNull(messageService.Message);
            this.viewModel.TestConnectionCommand.Execute(null);
            Assert.AreEqual(Resources.CannotConnect, messageService.Message);
            Assert.AreEqual(MessageType.Message, messageService.MessageType);

            this.viewModel.Settings.UserName = "snd\\BigEgg_cp";
            //  Replace the [Password] with real password.
            this.viewModel.Settings.Password = "[Password]";
            this.viewModel.TestConnectionCommand.Execute(null);
            Assert.IsTrue(this.viewModel.CanConnect);
            Assert.IsTrue(this.viewModel.TFSFields.Any());
        }
    }
}
