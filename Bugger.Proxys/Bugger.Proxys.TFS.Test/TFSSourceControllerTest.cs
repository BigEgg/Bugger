using BigEgg.Framework.UnitTesting;
using Bugger.Domain.Models;
using Bugger.Proxys.TFS.Documents;
using Bugger.Proxys.TFS.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Bugger.Proxys.TFS.Test
{
    [TestClass]
    public class TFSSourceControllerTest : TestClassBase
    {
        private TFSSourceControlProxy proxy;
        private SettingViewModel viewModel;

        protected override void OnTestInitialize()
        {
            if (File.Exists(SettingDocumentType.FilePath))
            {
                File.Delete(SettingDocumentType.FilePath);
            }

            this.proxy = Container.GetExportedValue<ISourceControlProxy>() as TFSSourceControlProxy;
            this.viewModel = Container.GetExportedValue<SettingViewModel>();
        }

        [TestMethod]
        public void GeneralTFSSourceControllerTest()
        {
            Assert.IsTrue(this.proxy.IsInitialized);
            Assert.IsFalse(this.proxy.CanQuery());
            Assert.IsNotNull(this.proxy.SettingViewModel);

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
