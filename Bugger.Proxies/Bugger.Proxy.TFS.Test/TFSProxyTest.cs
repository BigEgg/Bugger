using BigEgg.Framework.Applications.Services;
using BigEgg.Framework.Applications.Views;
using BigEgg.Framework.UnitTesting;
using Bugger.Domain.Models;
using Bugger.Proxy.TFS.Documents;
using Bugger.Proxy.TFS.Presentation.Fake.Views;
using Bugger.Proxy.TFS.Properties;
using Bugger.Proxy.TFS.Test.Services;
using Bugger.Proxy.TFS.ViewModels;
using Bugger.Proxy.TFS.Views;
using BigEgg.Framework.Applications.Views;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Bugger.Proxy.TFS.Models;
using System.Threading;
using System.Diagnostics;
using System.Windows.Threading;

namespace Bugger.Proxy.TFS.Test
{
    [TestClass]
    public class TFSProxyTest : TestClassBase
    {
        private static TFSProxy proxy;
        private static TFSSettingViewModel viewModel;
        private static int order = 0;

        protected override void OnTestInitialize()
        {
            if (proxy == null)
            {
                proxy = Container.GetExportedValue<ITracingSystemProxy>() as TFSProxy;
            }
        }

        [TestMethod]
        public void GeneralTFSProxyTest()
        {
            if (order++ != 0) { throw new NotSupportedException("This unit test must run as order."); }

            Assert.AreEqual("TFS", proxy.ProxyName);
            Assert.IsTrue(proxy.IsInitialized);
            Assert.IsFalse(proxy.CanQuery);
            Assert.IsNotNull(proxy.StateValues);
            Assert.IsFalse(proxy.StateValues.Any());
        }

        [TestMethod]
        public void InitializeSettingDialogTest()
        {
            if (order++ != 1) { throw new NotSupportedException("This unit test must run as order."); }

            var view = proxy.InitializeSettingDialog();
            Assert.IsNotNull(view);
            Assert.IsInstanceOfType(view, typeof(ITFSSettingView));

            viewModel = (view as ITFSSettingView).GetViewModel<TFSSettingViewModel>();
            Assert.IsNotNull(viewModel);

            Assert.IsNull(viewModel.ConnectUri);
            Assert.AreEqual(string.Empty, viewModel.UserName);
            Assert.AreEqual(string.Empty, viewModel.Password);

            foreach (var mapping in viewModel.PropertyMappingCollection)
            {
                Assert.AreEqual(string.Empty, viewModel.PropertyMappingCollection[mapping.Key]);
            }

            Assert.IsNotNull(viewModel.TFSFields);
            Assert.IsFalse(viewModel.TFSFields.Any());
            Assert.IsNotNull(viewModel.BugFilterFields);
            Assert.IsFalse(viewModel.BugFilterFields.Any());
            Assert.AreEqual(string.Empty, viewModel.BugFilterField);
            Assert.AreEqual(string.Empty, viewModel.BugFilterValue);

            Assert.IsNotNull(viewModel.PriorityValues);
            Assert.IsFalse(viewModel.PriorityValues.Any());
            Assert.AreEqual(string.Empty, viewModel.PriorityRed);

            Assert.AreEqual(ProgressTypes.NotWorking, viewModel.ProgressType);
            Assert.AreEqual(0, viewModel.ProgressValue);
        }

        [TestMethod]
        public void InitializeSettingDialogWithDataTest()
        {
            if (order++ != 10) { throw new NotSupportedException("This unit test must run as order."); }

            var view = proxy.InitializeSettingDialog();
            Assert.IsNotNull(view);
            Assert.IsInstanceOfType(view, typeof(ITFSSettingView));

            viewModel = (view as ITFSSettingView).GetViewModel<TFSSettingViewModel>();
            Assert.IsNotNull(viewModel);

            Assert.AreEqual(new Uri("https://tfs.codeplex.com:443/tfs/TFS12").AbsoluteUri, viewModel.ConnectUri.AbsoluteUri);
            Assert.AreEqual("snd\\BigEgg_cp", viewModel.UserName);
            Assert.AreEqual(ThePassword, viewModel.Password);

            Assert.AreEqual("ID", viewModel.PropertyMappingCollection["ID"]);
            Assert.AreEqual("Title", viewModel.PropertyMappingCollection["Title"]);
            Assert.AreEqual("Description", viewModel.PropertyMappingCollection["Description"]);
            Assert.AreEqual("Assigned To", viewModel.PropertyMappingCollection["AssignedTo"]);
            Assert.AreEqual("State", viewModel.PropertyMappingCollection["State"]);
            Assert.AreEqual("Changed Date", viewModel.PropertyMappingCollection["ChangedDate"]);
            Assert.AreEqual("Created By", viewModel.PropertyMappingCollection["CreatedBy"]);
            Assert.AreEqual("Code Studio Rank", viewModel.PropertyMappingCollection["Priority"]);

            Assert.IsNotNull(viewModel.TFSFields);
            Assert.IsTrue(viewModel.TFSFields.Any());

            Assert.AreEqual("Work Item Type", viewModel.BugFilterField);
            Assert.AreEqual("Work Item", viewModel.BugFilterValue);
            Assert.AreEqual(string.Empty, viewModel.PriorityRed);

            Assert.AreEqual(ProgressTypes.Success, viewModel.ProgressType);
            Assert.AreEqual(100, viewModel.ProgressValue);
        }

        [TestMethod]
        public void ValidateBeforeCloseSettingDialogBusyTest()
        {
            if (order++ != 2) { throw new NotSupportedException("This unit test must run as order."); }

            Assert.IsNotNull(viewModel);
            Assert.AreEqual(ProgressTypes.NotWorking, viewModel.ProgressType);

            viewModel.ProgressType = ProgressTypes.OnAutoFillMapSettings;
            Assert.AreEqual(SettingDialogValidateionResult.Busy, proxy.ValidateBeforeCloseSettingDialog());
            viewModel.ProgressType = ProgressTypes.OnConnectProgress;
            Assert.AreEqual(SettingDialogValidateionResult.Busy, proxy.ValidateBeforeCloseSettingDialog());
            viewModel.ProgressType = ProgressTypes.OnGetFiledsProgress;
            Assert.AreEqual(SettingDialogValidateionResult.Busy, proxy.ValidateBeforeCloseSettingDialog());

            viewModel.ProgressType = ProgressTypes.NotWorking;
        }

        [TestMethod]
        public void ValidateBeforeCloseSettingDialogConnectFailedTest()
        {
            if (order++ != 3) { throw new NotSupportedException("This unit test must run as order."); }

            Assert.IsNull(viewModel.ConnectUri);
            Assert.AreEqual(string.Empty, viewModel.UserName);
            Assert.AreEqual(string.Empty, viewModel.Password);
            Assert.AreEqual(SettingDialogValidateionResult.ConnectFailed, proxy.ValidateBeforeCloseSettingDialog());
            viewModel.ConnectUri = new Uri("https://tfs.codeplex.com:443/tfs/TFS12");
            Assert.AreEqual(SettingDialogValidateionResult.ConnectFailed, proxy.ValidateBeforeCloseSettingDialog());
            viewModel.UserName = "snd\\BigEgg_cp";
            viewModel.ProgressType = ProgressTypes.FailedOnConnect;
            Assert.AreEqual(SettingDialogValidateionResult.ConnectFailed, proxy.ValidateBeforeCloseSettingDialog());
            viewModel.ProgressType = ProgressTypes.FailedOnGetFileds;
            Assert.AreEqual(SettingDialogValidateionResult.ConnectFailed, proxy.ValidateBeforeCloseSettingDialog());

            viewModel.ProgressType = ProgressTypes.NotWorking;
            Assert.AreEqual(SettingDialogValidateionResult.ConnectFailed, proxy.ValidateBeforeCloseSettingDialog());
        }

        [TestMethod]
        public void ValidateBeforeCloseSettingDialogUnvalidTest()
        {
            if (order++ != 11) { throw new NotSupportedException("This unit test must run as order."); }

            var idMapping = viewModel.PropertyMappingCollection["ID"];
            viewModel.PropertyMappingCollection["ID"] = string.Empty;
            Assert.AreEqual(SettingDialogValidateionResult.UnValid, proxy.ValidateBeforeCloseSettingDialog());
            viewModel.PropertyMappingCollection["ID"] = idMapping;

            var bugFilterField = viewModel.BugFilterField;
            viewModel.BugFilterField = string.Empty;
            Assert.AreEqual(SettingDialogValidateionResult.UnValid, proxy.ValidateBeforeCloseSettingDialog());
            viewModel.BugFilterField = bugFilterField;

            var bugFilterValue = viewModel.BugFilterValue;
            viewModel.BugFilterValue = string.Empty;
            Assert.AreEqual(SettingDialogValidateionResult.UnValid, proxy.ValidateBeforeCloseSettingDialog());
            viewModel.BugFilterValue = bugFilterValue;
        }

        [TestMethod]
        public void ValidateBeforeCloseSettingDialogValidTest()
        {
            if (order++ != 8) { throw new NotSupportedException("This unit test must run as order."); }

            Assert.AreEqual(SettingDialogValidateionResult.Valid, proxy.ValidateBeforeCloseSettingDialog());
        }

        [TestMethod]
        public void AfterCloseSettingDialogSubmitTest()
        {
            if (order++ != 9) { throw new NotSupportedException("This unit test must run as order."); }

            Assert.IsFalse(File.Exists(SettingDocumentType.FilePath));
            proxy.AfterCloseSettingDialog(true);
            Assert.IsTrue(File.Exists(SettingDocumentType.FilePath));

            var doc = SettingDocumentType.Open();
            Assert.AreEqual(viewModel.ConnectUri.AbsoluteUri, doc.ConnectUri.AbsoluteUri);
            Assert.AreEqual(viewModel.UserName, doc.UserName);
            Assert.AreEqual(viewModel.Password, doc.Password);

            Assert.AreEqual(viewModel.PropertyMappingCollection["ID"], doc.PropertyMappingCollection["ID"]);
            Assert.AreEqual(viewModel.PropertyMappingCollection["Title"], doc.PropertyMappingCollection["Title"]);
            Assert.AreEqual(viewModel.PropertyMappingCollection["Description"], doc.PropertyMappingCollection["Description"]);
            Assert.AreEqual(viewModel.PropertyMappingCollection["AssignedTo"], doc.PropertyMappingCollection["AssignedTo"]);
            Assert.AreEqual(viewModel.PropertyMappingCollection["State"], doc.PropertyMappingCollection["State"]);
            Assert.AreEqual(viewModel.PropertyMappingCollection["ChangedDate"], doc.PropertyMappingCollection["ChangedDate"]);
            Assert.AreEqual(viewModel.PropertyMappingCollection["CreatedBy"], doc.PropertyMappingCollection["CreatedBy"]);
            Assert.AreEqual(viewModel.PropertyMappingCollection["Priority"], doc.PropertyMappingCollection["Priority"]);

            Assert.AreEqual(viewModel.BugFilterField, doc.BugFilterField);
            Assert.AreEqual(viewModel.BugFilterValue, doc.BugFilterValue);
            Assert.AreEqual(viewModel.PriorityRed, doc.PriorityRed);

            Assert.IsTrue(proxy.CanQuery);
        }

        [TestMethod]
        public void AfterCloseSettingDialogCancelTest()
        {
            if (order++ != 12) { throw new NotSupportedException("This unit test must run as order."); }

            proxy.AfterCloseSettingDialog(true);
            Assert.IsTrue(File.Exists(SettingDocumentType.FilePath));

            var idMapping = viewModel.PropertyMappingCollection["ID"];
            var bugFilterField = viewModel.BugFilterField;
            var bugFilterValue = viewModel.BugFilterValue;

            viewModel.PropertyMappingCollection["ID"] = string.Empty;
            viewModel.BugFilterField = string.Empty;
            viewModel.BugFilterValue = string.Empty;
            Assert.AreEqual(SettingDialogValidateionResult.UnValid, proxy.ValidateBeforeCloseSettingDialog());

            proxy.AfterCloseSettingDialog(false);

            var doc = SettingDocumentType.Open();
            Assert.AreEqual(idMapping, doc.PropertyMappingCollection["ID"]);
            Assert.AreEqual(bugFilterField, doc.BugFilterField);
            Assert.AreEqual(bugFilterValue, doc.BugFilterValue);

            Assert.IsTrue(proxy.CanQuery);
        }

        [TestMethod]
        public void CanTestConnectionCommandTest()
        {
            if (order++ != 4) { throw new NotSupportedException("This unit test must run as order."); }

            viewModel.ConnectUri = null;
            viewModel.UserName = string.Empty;
            Assert.IsFalse(viewModel.TestConnectionCommand.CanExecute(null));
            viewModel.ConnectUri = new Uri("https://tfs.codeplex.com:443/tfs/TFS12");
            Assert.IsFalse(viewModel.TestConnectionCommand.CanExecute(null));
            viewModel.UserName = "snd\\BigEgg_cp";
            Assert.IsTrue(viewModel.TestConnectionCommand.CanExecute(null));
        }

        [TestMethod]
        public void TestConnectionCommandFailedOnConnectTest()
        {
            if (order++ != 5) { throw new NotSupportedException("This unit test must run as order."); }

            proxy.TestConnectionCommandExcuteCore();

            Assert.AreEqual(ProgressTypes.FailedOnConnect, viewModel.ProgressType);
            Assert.AreEqual(100, viewModel.ProgressValue);
        }

        [TestMethod]
        public void TestConnectionCommandSuccessTest()
        {
            if (order++ != 6) { throw new NotSupportedException("This unit test must run as order."); }

            viewModel.Password = ThePassword;

            proxy.TestConnectionCommandExcuteCore();

            Assert.AreEqual(ProgressTypes.Success, viewModel.ProgressType);
            Assert.AreEqual(100, viewModel.ProgressValue);
        }

        [TestMethod]
        public void AutoMappingTest()
        {
            if (order++ != 7) { throw new NotSupportedException("This unit test must run as order."); }

            Assert.AreEqual("ID", viewModel.PropertyMappingCollection["ID"]);
            Assert.AreEqual("Title", viewModel.PropertyMappingCollection["Title"]);
            Assert.AreEqual("Description", viewModel.PropertyMappingCollection["Description"]);
            Assert.AreEqual("Assigned To", viewModel.PropertyMappingCollection["AssignedTo"]);
            Assert.AreEqual("State", viewModel.PropertyMappingCollection["State"]);
            Assert.AreEqual("Changed Date", viewModel.PropertyMappingCollection["ChangedDate"]);
            Assert.AreEqual("Created By", viewModel.PropertyMappingCollection["CreatedBy"]);
            Assert.AreEqual("Code Studio Rank", viewModel.PropertyMappingCollection["Priority"]);

            Assert.AreEqual("Work Item Type", viewModel.BugFilterField);
            //  Code Plex don't have a type named "Bugs"
            Assert.AreEqual(string.Empty, viewModel.BugFilterValue);

            Assert.AreEqual(string.Empty, viewModel.PriorityRed);

            viewModel.BugFilterValue = "Work Item";
        }

        [TestMethod]
        public void SettingViewModelConnectionPropertyChangedTest()
        {
            if (order++ != 13) { throw new NotSupportedException("This unit test must run as order."); }

        }

        [TestMethod]
        public void SettingViewModelPriorityMappingChangedTest()
        {
            if (order++ != 14) { throw new NotSupportedException("This unit test must run as order."); }

        }

        [TestMethod]
        public void SettingViewModelPriorityValuePropertyChangedTest()
        {
            if (order++ != 15) { throw new NotSupportedException("This unit test must run as order."); }

        }

        [TestMethod]
        public void QueryTest()
        {
            if (order++ != 16) { throw new NotSupportedException("This unit test must run as order."); }

        }

        //[TestMethod]
        //public void QueryTest()
        //{
        //    AssertHelper.ExpectedException<NotSupportedException>(() => proxy.Query("snd\\BigEgg_cp"));

        //    viewModel.Settings.ConnectUri = new Uri("https://tfs.codeplex.com:443/tfs/TFS12");
        //    viewModel.Settings.BugFilterField = "Work Item Type";
        //    viewModel.Settings.BugFilterValue = "Work Item";
        //    viewModel.Settings.UserName = "snd\\BigEgg_cp";
        //    viewModel.Settings.Password = ThePassword;
        //    viewModel.Settings.PropertyMappingCollection["ID"] = "ID";
        //    viewModel.Settings.PropertyMappingCollection["Title"] = "Title";
        //    viewModel.Settings.PropertyMappingCollection["Description"] = "Description";
        //    viewModel.Settings.PropertyMappingCollection["AssignedTo"] = "Assigned To";
        //    viewModel.Settings.PropertyMappingCollection["State"] = "State";
        //    viewModel.Settings.PropertyMappingCollection["ChangedDate"] = "Changed Date";
        //    viewModel.Settings.PropertyMappingCollection["CreatedBy"] = "Created By";
        //    viewModel.Settings.PropertyMappingCollection["Priority"] = "Code Studio Rank";

        //    Assert.IsTrue(proxy.CanQuery);
        //    ReadOnlyCollection<Bug> bugs = proxy.Query("BigEgg_cp");
        //    Assert.IsNotNull(bugs);
        //    Assert.IsTrue(bugs.Any());

        //    bugs = proxy.Query("BigEgg_cp", false);
        //    Assert.IsNotNull(bugs);
        //    Assert.IsTrue(bugs.Any());
        //}
    }
}
