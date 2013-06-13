﻿using BigEgg.Framework.Applications.Services;
using BigEgg.Framework.Applications.Views;
using BigEgg.Framework.UnitTesting;
using Bugger.Domain.Models;
using Bugger.Proxy.TFS.Documents;
using Bugger.Proxy.TFS.Presentation.Fake.Views;
using Bugger.Proxy.TFS.Properties;
using Bugger.Proxy.TFS.Test.Services;
using Bugger.Proxy.TFS.ViewModels;
using Bugger.Proxy.TFS.Views;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Bugger.Proxy.TFS.Test
{
    [TestClass]
    public class TFSSourceControllerTest : TestClassBase
    {
        private TFSSourceControlProxy proxy;
        private TFSSettingViewModel viewModel;

        protected override void OnTestInitialize()
        {
            if (File.Exists(SettingDocumentType.FilePath))
            {
                File.Delete(SettingDocumentType.FilePath);
            }

            this.proxy = Container.GetExportedValue<ISourceControlProxy>() as TFSSourceControlProxy;
            this.viewModel = Container.GetExportedValue<TFSSettingViewModel>();
        }

        [TestMethod]
        public void GeneralTFSSourceControllerTest()
        {
            Assert.IsTrue(this.proxy.IsInitialized);
            Assert.IsFalse(this.proxy.CanQuery());
            Assert.IsNotNull(this.proxy.SettingView);

            Assert.IsNotNull(this.viewModel.SaveCommand);
            Assert.IsNotNull(this.viewModel.Settings);
        }

        [TestMethod]
        public void SaveCommandTest()
        {
            this.viewModel.Settings.ConnectUri = new Uri("https://tfs.codeplex.com:443/tfs/TFS12");
            this.viewModel.Settings.BugFilterField = "WorkItemType";
            this.viewModel.Settings.BugFilterValue = "Issue";
            this.viewModel.Settings.UserName = "snd\\BigEgg_cp";
            this.viewModel.Settings.Password = password;
            this.viewModel.Settings.PropertyMappingList.First(x => x.PropertyName == "ID").FieldName = "ID";
            this.viewModel.Settings.PropertyMappingList.First(x => x.PropertyName == "Title").FieldName = "Title";
            this.viewModel.Settings.PropertyMappingList.First(x => x.PropertyName == "Description").FieldName = "Description";
            this.viewModel.Settings.PropertyMappingList.First(x => x.PropertyName == "AssignedTo").FieldName = "Assigned To";
            this.viewModel.Settings.PropertyMappingList.First(x => x.PropertyName == "State").FieldName = "State";
            this.viewModel.Settings.PropertyMappingList.First(x => x.PropertyName == "ChangedDate").FieldName = "Changed Date";
            this.viewModel.Settings.PropertyMappingList.First(x => x.PropertyName == "CreatedBy").FieldName = "Created By";
            this.viewModel.Settings.PropertyMappingList.First(x => x.PropertyName == "Priority").FieldName = "Code Studio Rank";

            Assert.IsTrue(this.viewModel.SaveCommand.CanExecute(null));

            this.viewModel.SaveCommand.Execute(null);
            Assert.IsTrue(File.Exists(SettingDocumentType.FilePath));
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
            this.viewModel.Settings.UserName = "snd\\BigEgg_cp";
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

            MockMessageService messageService = Container.GetExportedValue<IMessageService>() as MockMessageService;
            messageService.Clear();
            Assert.IsNull(messageService.Message);
            this.viewModel.TestConnectionCommand.Execute(null);
            Assert.AreEqual(Resources.CannotConnect, messageService.Message);
            Assert.AreEqual(MessageType.Message, messageService.MessageType);

            this.viewModel.Settings.UserName = "snd\\BigEgg_cp";
            this.viewModel.Settings.Password = password;
            this.viewModel.TestConnectionCommand.Execute(null);
            Assert.IsTrue(this.viewModel.CanConnect);
            Assert.IsTrue(this.viewModel.TFSFields.Any());
        }

        [TestMethod]
        public void QueryTest()
        {
            AssertHelper.ExpectedException<NotSupportedException>(() => this.proxy.Query("snd\\BigEgg_cp"));

            this.viewModel.Settings.ConnectUri = new Uri("https://tfs.codeplex.com:443/tfs/TFS12");
            this.viewModel.Settings.BugFilterField = "Work Item Type";
            this.viewModel.Settings.BugFilterValue = "Work Item";
            this.viewModel.Settings.UserName = "snd\\BigEgg_cp";
            this.viewModel.Settings.Password = password;
            this.viewModel.Settings.PropertyMappingList.First(x => x.PropertyName == "ID").FieldName = "ID";
            this.viewModel.Settings.PropertyMappingList.First(x => x.PropertyName == "Title").FieldName = "Title";
            this.viewModel.Settings.PropertyMappingList.First(x => x.PropertyName == "Description").FieldName = "Description";
            this.viewModel.Settings.PropertyMappingList.First(x => x.PropertyName == "AssignedTo").FieldName = "Assigned To";
            this.viewModel.Settings.PropertyMappingList.First(x => x.PropertyName == "State").FieldName = "State";
            this.viewModel.Settings.PropertyMappingList.First(x => x.PropertyName == "ChangedDate").FieldName = "Changed Date";
            this.viewModel.Settings.PropertyMappingList.First(x => x.PropertyName == "CreatedBy").FieldName = "Created By";
            this.viewModel.Settings.PropertyMappingList.First(x => x.PropertyName == "Priority").FieldName = "Code Studio Rank";

            Assert.IsTrue(this.proxy.CanQuery());
            ReadOnlyCollection<Bug> bugs = this.proxy.Query("BigEgg_cp");
            Assert.IsNotNull(bugs);
            Assert.IsTrue(bugs.Any());

            bugs = null;
            bugs = this.proxy.Query("BigEgg_cp", false);
            Assert.IsNotNull(bugs);
            Assert.IsTrue(bugs.Any());
        }
    }
}