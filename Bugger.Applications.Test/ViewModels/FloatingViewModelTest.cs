using BigEgg.Framework.Applications.Commands;
using BigEgg.Framework.UnitTesting;
using Bugger.Applications.Properties;
using Bugger.Applications.Services;
using Bugger.Applications.Test.Services;
using Bugger.Applications.Test.Views;
using Bugger.Applications.ViewModels;
using Bugger.Applications.Views;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Bugger.Applications.Test.ViewModels
{
    [TestClass]
    public class FloatingViewModelTest : TestClassBase
    {
        [TestMethod]
        public void PropertiesWithNotification()
        {
            FloatingViewModel viewModel = Container.GetExportedValue<FloatingViewModel>();

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

            ICommand showMainWindowCommand = new DelegateCommand(() => { });
            AssertHelper.PropertyChangedEvent(viewModel, x => x.ShowMainWindowCommand, () => viewModel.ShowMainWindowCommand = showMainWindowCommand);
            Assert.AreEqual(showMainWindowCommand, viewModel.ShowMainWindowCommand);
        }

        [TestMethod]
        public void ShowAndClose()
        {
            MockFloatingView view = (MockFloatingView)Container.GetExportedValue<IFloatingView>();
            FloatingViewModel viewModel = Container.GetExportedValue<FloatingViewModel>();

            // Show the MainView
            Assert.IsFalse(view.IsVisible);
            viewModel.Show();
            Assert.IsTrue(view.IsVisible);

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

            SetSettingsValues(20, 10, 100, 30);

            FloatingViewModel viewModel = Container.GetExportedValue<FloatingViewModel>();
            MockFloatingView view = (MockFloatingView)Container.GetExportedValue<IFloatingView>();
            Assert.AreEqual(20, view.Left);
            Assert.AreEqual(10, view.Top);
            Assert.AreEqual(100, view.Width);
            Assert.AreEqual(30, view.Height);

            view.Left = 25;
            view.Top = 15;
            view.Width = 150;
            view.Height = 35;

            view.Close();
            AssertSettingsValues(25, 15, 150, 35);
        }

        [TestMethod]
        public void RestoreWindowLocationAndSizeSpecial()
        {
            DataService dataService = new DataService();
            MockPresentationService presentationService = (MockPresentationService)Container.GetExportedValue<IPresentationService>();
            presentationService.VirtualScreenWidth = 1000;
            presentationService.VirtualScreenHeight = 700;

            FloatingViewModel viewModel = Container.GetExportedValue<FloatingViewModel>();
            MockFloatingView view = (MockFloatingView)Container.GetExportedValue<IFloatingView>();
            view.SetNAForLocationAndSize();

            SetSettingsValues();
            new FloatingViewModel(view, dataService, presentationService).Close();
            AssertSettingsValues(double.NaN, double.NaN, double.NaN, double.NaN);

            // Height is 0 => don't apply the Settings values
            SetSettingsValues(0, 0, 1, 0);
            new FloatingViewModel(view, dataService, presentationService).Close();
            AssertSettingsValues(double.NaN, double.NaN, double.NaN, double.NaN);

            // Left = 100 + Width = 901 > VirtualScreenWidth = 1000 => don't apply the Settings values
            SetSettingsValues(100, 100, 901, 100);
            new FloatingViewModel(view, dataService, presentationService).Close();
            AssertSettingsValues(double.NaN, double.NaN, double.NaN, double.NaN);

            // Top = 100 + Height = 601 > VirtualScreenWidth = 600 => don't apply the Settings values
            SetSettingsValues(100, 100, 100, 601);
            new FloatingViewModel(view, dataService, presentationService).Close();
            AssertSettingsValues(double.NaN, double.NaN, double.NaN, double.NaN);

            // Use the limit values => apply the Settings values
            SetSettingsValues(0, 0, 1000, 700);
            new FloatingViewModel(view, dataService, presentationService).Close();
            AssertSettingsValues(0, 0, 1000, 700);
        }


        private void SetSettingsValues(double left = 0, double top = 0, double width = 0, double height = 0)
        {
            Settings.Default.FloatingWindowLeft = left;
            Settings.Default.FloatingWindowTop = top;
            Settings.Default.FloatingWindowWidth = width;
            Settings.Default.FloatingWindowHeight = height;
        }

        private void AssertSettingsValues(double left, double top, double width, double height)
        {
            Assert.AreEqual(left, Settings.Default.FloatingWindowLeft);
            Assert.AreEqual(top, Settings.Default.FloatingWindowTop);
            Assert.AreEqual(width, Settings.Default.FloatingWindowWidth);
            Assert.AreEqual(height, Settings.Default.FloatingWindowHeight);
        }
    }
}
