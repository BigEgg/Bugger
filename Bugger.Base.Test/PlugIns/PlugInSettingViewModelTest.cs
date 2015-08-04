using BigEgg.Framework.Applications.UnitTesting.Views;
using Bugger.PlugIns;
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
            var view = new MockPlugInSettingView();
            var viewModel = new MockPlugInSettingViewModel(view);

            Assert.AreEqual(PlugInSettingValidationResult.Valid, viewModel.ValidateSettings());
        }

        [TestMethod]
        public void SubmitSettingChangesTest()
        {
            var view = new MockPlugInSettingView();
            var viewModel = new MockPlugInSettingViewModel(view);

            Assert.IsFalse(viewModel.SubmitSettingChangesCoreCalled);
            viewModel.SubmitSettingChanges();
            Assert.IsTrue(viewModel.SubmitSettingChangesCoreCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SubmitSettingChangesTest_Busy()
        {
            var view = new MockPlugInSettingView();
            var viewModel = new MockPlugInSettingViewModel(view);
            viewModel.ValidationResult = PlugInSettingValidationResult.Busy;

            Assert.IsFalse(viewModel.SubmitSettingChangesCoreCalled);
            viewModel.SubmitSettingChanges();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SubmitSettingChangesTest_UnValid()
        {
            var view = new MockPlugInSettingView();
            var viewModel = new MockPlugInSettingViewModel(view);
            viewModel.ValidationResult = PlugInSettingValidationResult.UnValid;

            Assert.IsFalse(viewModel.SubmitSettingChangesCoreCalled);
            viewModel.SubmitSettingChanges();
        }
    }

    public class MockPlugInSettingView : MockView, IPlugInSettingView
    {
    }

    public class MockPlugInSettingViewModel : PlugInSettingViewModel<IPlugInSettingView>
    {
        public MockPlugInSettingViewModel(IPlugInSettingView view)
            : base(view)
        {
            CleanUp();
        }


        public PlugInSettingValidationResult ValidationResult { get; set; }

        public bool SubmitSettingChangesCoreCalled { get; set; }


        public override PlugInSettingValidationResult ValidateSettings()
        {
            return ValidationResult;
        }

        protected override void SubmitSettingChangesCore()
        {
            SubmitSettingChangesCoreCalled = true;
        }

        public void CleanUp()
        {
            ValidationResult = PlugInSettingValidationResult.Valid;
            SubmitSettingChangesCoreCalled = false;
        }
    }
}
