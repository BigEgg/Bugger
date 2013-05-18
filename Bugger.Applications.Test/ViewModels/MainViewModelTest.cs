using BigEgg.Framework.Applications.Commands;
using BigEgg.Framework.UnitTesting;
using Bugger.Applications.Properties;
using Bugger.Applications.Services;
using Bugger.Applications.Test.Services;
using Bugger.Applications.Test.Views;
using Bugger.Applications.ViewModels;
using Bugger.Applications.Views;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Input;

namespace Bugger.Applications.Test.ViewModels
{
    [TestClass]
    public class MainViewModelTest : TestClassBase
    {
        [TestMethod]
        public void PropertiesWithNotification()
        {
            MainViewModel viewModel = Container.GetExportedValue<MainViewModel>();

            ICommand exitCommand = new DelegateCommand(() => { });
            AssertHelper.PropertyChangedEvent(viewModel, x => x.ExitCommand, () => viewModel.ExitCommand = exitCommand);
            Assert.AreEqual(exitCommand, viewModel.ExitCommand);

            ICommand settingCommand = new DelegateCommand(() => { });
            AssertHelper.PropertyChangedEvent(viewModel, x => x.SettingCommand, () => viewModel.SettingCommand = settingCommand);
            Assert.AreEqual(settingCommand, viewModel.SettingCommand);

            ICommand aboutCommand = new DelegateCommand(() => { });
            AssertHelper.PropertyChangedEvent(viewModel, x => x.AboutCommand, () => viewModel.AboutCommand = aboutCommand);
            Assert.AreEqual(aboutCommand, viewModel.AboutCommand);

            ICommand refreshBugsCommand = new DelegateCommand(() => { });
            AssertHelper.PropertyChangedEvent(viewModel, x => x.RefreshBugsCommand, () => viewModel.RefreshBugsCommand = refreshBugsCommand);
            Assert.AreEqual(refreshBugsCommand, viewModel.RefreshBugsCommand);

            ICommand englishCommand = new DelegateCommand(() => { });
            AssertHelper.PropertyChangedEvent(viewModel, x => x.EnglishCommand, () => viewModel.EnglishCommand = englishCommand);
            Assert.AreEqual(englishCommand, viewModel.EnglishCommand);

            ICommand chineseCommand = new DelegateCommand(() => { });
            AssertHelper.PropertyChangedEvent(viewModel, x => x.ChineseCommand, () => viewModel.ChineseCommand = chineseCommand);
            Assert.AreEqual(chineseCommand, viewModel.ChineseCommand);
        }

        [TestMethod]
        public void ShowAndClose()
        {
            MockMainView view = (MockMainView)Container.GetExportedValue<IMainView>();
            MainViewModel viewModel = Container.GetExportedValue<MainViewModel>();

            // Show the MainView
            Assert.IsFalse(view.IsVisible);
            viewModel.Show();
            Assert.IsTrue(view.IsVisible);

            Assert.AreNotEqual("", viewModel.Title);

            // Try to close the ShellView but cancel this operation through the closing event
            bool cancelClosing = true;
            viewModel.Closing += (sender, e) =>
            {
                e.Cancel = cancelClosing;
            };
            viewModel.Close();
            Assert.IsTrue(view.IsVisible);

            // Close the ShellView via the ExitCommand
            cancelClosing = false;
            AssertHelper.PropertyChangedEvent(viewModel, x => x.ExitCommand, () =>
                viewModel.ExitCommand = new DelegateCommand(() => viewModel.Close()));
            viewModel.ExitCommand.Execute(null);
            Assert.IsFalse(view.IsVisible);
        }


        [TestMethod]
        public void RestoreWindowLocationAndSize()
        {
            MockPresentationService presentationService = (MockPresentationService)Container.GetExportedValue<IPresentationService>();
            presentationService.VirtualScreenWidth = 1000;
            presentationService.VirtualScreenHeight = 700;

            SetSettingsValues(20, 10, 400, 300);

            MainViewModel viewModel = Container.GetExportedValue<MainViewModel>();
            MockMainView view = (MockMainView)Container.GetExportedValue<IMainView>();
            Assert.AreEqual(20, view.Left);
            Assert.AreEqual(10, view.Top);
            Assert.AreEqual(400, view.Width);
            Assert.AreEqual(300, view.Height);

            view.Left = 25;
            view.Top = 15;
            view.Width = 450;
            view.Height = 350;

            view.Close();
            AssertSettingsValues(25, 15, 450, 350);
        }

        [TestMethod]
        public void RestoreWindowLocationAndSizeSpecial()
        {
            DataService dataService = new DataService();
            MockPresentationService presentationService = (MockPresentationService)Container.GetExportedValue<IPresentationService>();
            presentationService.VirtualScreenWidth = 1000;
            presentationService.VirtualScreenHeight = 700;

            MainViewModel viewModel = Container.GetExportedValue<MainViewModel>();
            MockMainView view = (MockMainView)Container.GetExportedValue<IMainView>();
            view.SetNAForLocationAndSize();

            SetSettingsValues();
            new MainViewModel(view, dataService, presentationService).Close();
            AssertSettingsValues(double.NaN, double.NaN, double.NaN, double.NaN);

            // Height is 0 => don't apply the Settings values
            SetSettingsValues(0, 0, 1, 0);
            new MainViewModel(view, dataService, presentationService).Close();
            AssertSettingsValues(double.NaN, double.NaN, double.NaN, double.NaN);

            // Left = 100 + Width = 901 > VirtualScreenWidth = 1000 => don't apply the Settings values
            SetSettingsValues(100, 100, 901, 100);
            new MainViewModel(view, dataService, presentationService).Close();
            AssertSettingsValues(double.NaN, double.NaN, double.NaN, double.NaN);

            // Top = 100 + Height = 601 > VirtualScreenWidth = 600 => don't apply the Settings values
            SetSettingsValues(100, 100, 100, 601);
            new MainViewModel(view, dataService, presentationService).Close();
            AssertSettingsValues(double.NaN, double.NaN, double.NaN, double.NaN);

            // Use the limit values => apply the Settings values
            SetSettingsValues(0, 0, 1000, 700);
            new MainViewModel(view, dataService, presentationService).Close();
            AssertSettingsValues(0, 0, 1000, 700);
        }


        private void SetSettingsValues(double left = 0, double top = 0, double width = 0, double height = 0)
        {
            Settings.Default.MainWindowLeft = left;
            Settings.Default.MainWindowTop = top;
            Settings.Default.MainWindowWidth = width;
            Settings.Default.MainWindowHeight = height;
        }

        private void AssertSettingsValues(double left, double top, double width, double height)
        {
            Assert.AreEqual(left, Settings.Default.MainWindowLeft);
            Assert.AreEqual(top, Settings.Default.MainWindowTop);
            Assert.AreEqual(width, Settings.Default.MainWindowWidth);
            Assert.AreEqual(height, Settings.Default.MainWindowHeight);
        }
    }
}
