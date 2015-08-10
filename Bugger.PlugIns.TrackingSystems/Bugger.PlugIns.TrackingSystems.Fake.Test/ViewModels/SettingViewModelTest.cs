using BigEgg.Framework.Applications.UnitTesting;
using Bugger.PlugIns.TrackingSystems.Fake.Properties;
using Bugger.PlugIns.TrackingSystems.Fake.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Bugger.PlugIns.TrackingSystems.Fake.Test.ViewModels
{
    [TestClass]
    public class SettingViewModelTest : TestClassBase
    {
        [TestMethod]
        public void InjectionTest()
        {
            var viewModel = Container.GetExportedValue<SettingViewModel>();
            Assert.IsNotNull(viewModel);
        }

        [TestMethod]
        public void GeneralTest()
        {
            var viewModel = Container.GetExportedValue<SettingViewModel>();
            Assert.AreEqual(PlugInSettingValidationResult.Invalid, viewModel.ValidateSettings());

            viewModel.UsersName = "BigEgg";
            viewModel.BugsCacheMinutes = 10;
            viewModel.BugsCountForEveryone = 5;

            Assert.AreEqual(PlugInSettingValidationResult.Valid, viewModel.ValidateSettings());
        }

        [TestMethod]
        public void UsersNamePropertyChangedTest()
        {
            var viewModel = Container.GetExportedValue<SettingViewModel>();

            viewModel.Validate();
            Assert.IsTrue(viewModel.GetErrors("UsersName").Any());

            AssertHelper.IsRaiseBothErrorChangedEventAndPropertyChangedEvent(viewModel, vm => vm.UsersName, () => viewModel.UsersName = "BigEgg");
            Assert.IsFalse(viewModel.GetErrors("UsersName").Any());

            AssertHelper.IsRaisePropertyChangedEvent(viewModel, vm => vm.UsersName, () => viewModel.UsersName = "Pupil");
            Assert.IsFalse(viewModel.GetErrors("UsersName").Any());

            AssertHelper.IsRaiseBothErrorChangedEventAndPropertyChangedEvent(viewModel, vm => vm.UsersName, () => viewModel.UsersName = "");
            Assert.IsTrue(viewModel.GetErrors("UsersName").Any());
        }

        [TestMethod]
        public void BugsCacheMinutesPropertyChangedTest()
        {
            var viewModel = Container.GetExportedValue<SettingViewModel>();

            viewModel.Validate();
            Assert.IsTrue(viewModel.GetErrors("BugsCacheMinutes").Any());

            AssertHelper.IsRaiseBothErrorChangedEventAndPropertyChangedEvent(viewModel, vm => vm.BugsCacheMinutes, () => viewModel.BugsCacheMinutes = 1);
            Assert.IsFalse(viewModel.GetErrors("BugsCacheMinutes").Any());

            AssertHelper.IsRaisePropertyChangedEvent(viewModel, vm => vm.BugsCacheMinutes, () => viewModel.BugsCacheMinutes = 30);
            Assert.IsFalse(viewModel.GetErrors("BugsCacheMinutes").Any());

            AssertHelper.IsRaiseBothErrorChangedEventAndPropertyChangedEvent(viewModel, vm => vm.BugsCacheMinutes, () => viewModel.BugsCacheMinutes = 31);
            Assert.IsTrue(viewModel.GetErrors("BugsCacheMinutes").Any());
        }

        [TestMethod]
        public void BugsCountForEveryonePropertyChangedTest()
        {
            var viewModel = Container.GetExportedValue<SettingViewModel>();

            viewModel.Validate();
            Assert.IsTrue(viewModel.GetErrors("BugsCountForEveryone").Any());

            AssertHelper.IsRaiseBothErrorChangedEventAndPropertyChangedEvent(viewModel, vm => vm.BugsCountForEveryone, () => viewModel.BugsCountForEveryone = 1);
            Assert.IsFalse(viewModel.GetErrors("BugsCountForEveryone").Any());

            AssertHelper.IsRaisePropertyChangedEvent(viewModel, vm => vm.BugsCountForEveryone, () => viewModel.BugsCountForEveryone = 10);
            Assert.IsFalse(viewModel.GetErrors("BugsCountForEveryone").Any());

            AssertHelper.IsRaiseBothErrorChangedEventAndPropertyChangedEvent(viewModel, vm => vm.BugsCountForEveryone, () => viewModel.BugsCountForEveryone = 11);
            Assert.IsTrue(viewModel.GetErrors("BugsCountForEveryone").Any());
        }

        [TestMethod]
        public void SubmitSettingChangesTest()
        {
            var viewModel = Container.GetExportedValue<SettingViewModel>();

            var originalBugsCacheMinutes = Settings.Default.BugsCacheMinutes;
            var originalBugsForEveryone = Settings.Default.BugsForEveryone;
            var originalUsersName = Settings.Default.UsersName;

            viewModel.UsersName = originalUsersName + ";abc";
            viewModel.BugsCountForEveryone = originalBugsForEveryone + 1;
            viewModel.BugsCacheMinutes = originalBugsCacheMinutes + 1;

            viewModel.SubmitSettingChanges();

            Settings.Default.Reload();
            Assert.AreNotEqual(originalBugsCacheMinutes, Settings.Default.BugsCacheMinutes);
            Assert.AreNotEqual(originalBugsForEveryone, Settings.Default.BugsForEveryone);
            Assert.AreNotEqual(originalUsersName, Settings.Default.UsersName);

            Settings.Default.Reset();
        }
    }
}
