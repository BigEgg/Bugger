using BigEgg.Framework.Applications.UnitTesting.Views;
using Bugger.PlugIns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Bugger.Domain.Test.PlugIns
{
    [TestClass]
    public class PlugInSettingDialogViewModelTest
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
        public void SubmitSettingChangesTest_UnValid()
        {
            var view = new MockPlugInSettingDialogView();
            var viewModel = new MockPlugInSettingDialogViewModel(view);
            viewModel.ValidationResult = PlugInSettingDialogValidationResult.UnValid;

            Assert.IsFalse(viewModel.SubmitSettingChangesCoreCalled);
            viewModel.SubmitSettingChanges();
        }
    }

    public class MockPlugInSettingDialogView : MockView, IPlugInSettingDialogView
    {
    }

    public class MockPlugInSettingDialogViewModel : PlugInSettingDialogViewModel<IPlugInSettingDialogView>
    {
        public MockPlugInSettingDialogViewModel(IPlugInSettingDialogView view)
            : base(view)
        {
            CleanUp();
        }


        public PlugInSettingDialogValidationResult ValidationResult { get; set; }

        public bool SubmitSettingChangesCoreCalled { get; set; }


        public override PlugInSettingDialogValidationResult ValidateSettings()
        {
            return ValidationResult;
        }

        protected override void SubmitSettingChangesCore()
        {
            SubmitSettingChangesCoreCalled = true;
        }

        public void CleanUp()
        {
            ValidationResult = PlugInSettingDialogValidationResult.Valid;
            SubmitSettingChangesCoreCalled = false;
        }
    }
}
