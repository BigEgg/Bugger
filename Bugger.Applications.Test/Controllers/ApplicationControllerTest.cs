using Bugger.Applications.Controllers;
using Bugger.Applications.Properties;
using Bugger.Applications.Test.Views;
using Bugger.Applications.ViewModels;
using Bugger.Applications.Views;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Bugger.Applications.Test.Controllers
{
    [TestClass]
    public class ApplicationControllerTest : TestClassBase
    {
        [TestMethod]
        public void ControllerLifecycle()
        {
            IApplicationController applicationController = Container.GetExportedValue<IApplicationController>();

            applicationController.Initialize();
            FloatingViewModel floatingViewModel = Container.GetExportedValue<FloatingViewModel>();
            Assert.IsNotNull(floatingViewModel.AboutCommand);
            Assert.IsNotNull(floatingViewModel.SettingCommand);
            Assert.IsNotNull(floatingViewModel.ExitCommand);
            Assert.IsNotNull(floatingViewModel.ChineseCommand);
            Assert.IsNotNull(floatingViewModel.EnglishCommand);
            Assert.IsNotNull(floatingViewModel.RefreshBugsCommand);
            Assert.IsNotNull(floatingViewModel.ShowMainWindowCommand);

            MainViewModel mainViewModel = Container.GetExportedValue<MainViewModel>();
            Assert.IsNotNull(mainViewModel.AboutCommand);
            Assert.IsNotNull(mainViewModel.SettingCommand);
            Assert.IsNotNull(mainViewModel.ExitCommand);
            Assert.IsNotNull(mainViewModel.ChineseCommand);
            Assert.IsNotNull(mainViewModel.EnglishCommand);
            Assert.IsNotNull(mainViewModel.RefreshBugsCommand);

            applicationController.Run();
            MockFloatingView floatingView = (MockFloatingView)Container.GetExportedValue<IFloatingView>();
            Assert.IsTrue(floatingView.IsVisible);

            floatingViewModel.ExitCommand.Execute(null);
            Assert.IsFalse(floatingView.IsVisible);

            applicationController.ShutDown();
        }

        [TestMethod]
        public void LanguageSettingsTest()
        {
            Settings.Default.Culture = "en-US";
            Settings.Default.UICulture = "en-US";

            ApplicationController applicationController = Container.GetExportedValue<IApplicationController>() as ApplicationController;

            Assert.AreEqual(new CultureInfo("en-US"), CultureInfo.CurrentCulture);
            Assert.AreEqual(new CultureInfo("en-US"), CultureInfo.CurrentUICulture);

            applicationController.Initialize();
            applicationController.Run();

            FloatingViewModel floatingViewModel = Container.GetExportedValue<FloatingViewModel>();
            floatingViewModel.ChineseCommand.Execute(null);
            Assert.AreEqual(new CultureInfo("zh-CN"), applicationController.NewLanguage);
            
            bool settingsSaved = false;
            Settings.Default.SettingsSaving += (sender, e) =>
            {
                settingsSaved = true;
            };

            applicationController.ShutDown();
            Assert.AreEqual("zh-CN", Settings.Default.UICulture);
            Assert.IsTrue(settingsSaved);
        }

        [TestMethod]
        public void SelectLanguageTest()
        {
            ApplicationController applicationController = Container.GetExportedValue<IApplicationController>() as ApplicationController;
            applicationController.Initialize();
            applicationController.Run();

            FloatingViewModel floatingViewModel = Container.GetExportedValue<FloatingViewModel>();
            MainViewModel mainViewModel = Container.GetExportedValue<MainViewModel>();

            Assert.IsNull(applicationController.NewLanguage);

            floatingViewModel.ChineseCommand.Execute(null);
            Assert.AreEqual("zh-CN", applicationController.NewLanguage.Name);

            mainViewModel.EnglishCommand.Execute(null);
            Assert.AreEqual("en-US", applicationController.NewLanguage.Name);
        }

        [TestMethod]
        public void ShowMainWindowCommandTest()
        {
            IApplicationController applicationController = Container.GetExportedValue<IApplicationController>();

            applicationController.Initialize();
            FloatingViewModel floatingViewModel = Container.GetExportedValue<FloatingViewModel>();
            MainViewModel mainViewModel = Container.GetExportedValue<MainViewModel>();

            applicationController.Run();

            MockFloatingView floatingView = (MockFloatingView)Container.GetExportedValue<IFloatingView>();
            MockMainView mainView = (MockMainView)Container.GetExportedValue<IMainView>();
            Assert.IsTrue(floatingView.IsVisible);
            Assert.IsFalse(mainView.IsVisible);

            floatingViewModel.ShowMainWindowCommand.Execute(null);
            Assert.IsTrue(mainView.IsVisible);

            applicationController.ShutDown();
        }
    }
}
