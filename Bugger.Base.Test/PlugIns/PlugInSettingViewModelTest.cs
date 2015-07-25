using Bugger.Base.PlugIns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Bugger.Domain.Test.PlugIns
{
    [TestClass]
    public class PlugInSettingViewModelTest
    {
        [TestMethod]
        public void ValidateSettingsTest()
        {
            var view = new MockPlugInSettingDialogView();
            var viewModel = new MockPlugInSettingDialogViewModel(view);

            Assert.AreEqual(PlugInSettingDialogValidationResult.Valid, viewModel.ValidateSettings());
        }

        [TestMethod]
        public void SubmitSettingChangesTest()
        {
            var view = new MockPlugInSettingDialogView();
            var viewModel = new MockPlugInSettingDialogViewModel(view);

            Assert.IsFalse(viewModel.SubmitSettingChangesCoreCalled);
            viewModel.SubmitSettingChanges();
            Assert.IsTrue(viewModel.SubmitSettingChangesCoreCalled);
        }
        
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SubmitSettingChangesTest_Busy()
        {
            var view = new MockPlugInSettingDialogView();
            var viewModel = new MockPlugInSettingDialogViewModel(view);
            viewModel.ValidationResult = PlugInSettingDialogValidationResult.Busy;

            Assert.IsFalse(viewModel.SubmitSettingChangesCoreCalled);
            viewModel.SubmitSettingChanges();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SubmitSettingChangesTest_ConnectFailed()
        {
            var view = new MockPlugInSettingDialogView();
            var viewModel = new MockPlugInSettingDialogViewModel(view);
            viewModel.ValidationResult = PlugInSettingDialogValidationResult.ConnectFailed;

            Assert.IsFalse(viewModel.SubmitSettingChangesCoreCalled);
            viewModel.SubmitSettingChanges();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SubmitSettingChangesTest_UnValid()
        {
            var view = new MockPlugInSettingDialogView();
            var viewModel = new MockPlugInSettingDialogViewModel(view);
            viewModel.ValidationResult = PlugInSettingDialogValidationResult.UnValid;

            Assert.IsFalse(viewModel.SubmitSettingChangesCoreCalled);
            viewModel.SubmitSettingChanges();
        }
    }
}
