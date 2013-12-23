using BigEgg.Framework.Applications.Commands;
using BigEgg.Framework.Applications.Services;
using BigEgg.Framework.Applications.ViewModels;
using Bugger.Applications.Properties;
using Bugger.Applications.Services;
using Bugger.Applications.ViewModels;
using Bugger.Applications.Views;
using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bugger.Applications.Controllers
{
    [Export(typeof(IApplicationController))]
    internal class ApplicationController : Controller, IApplicationController
    {
        #region Fields
        private readonly CompositionContainer container;
        private readonly DataController dataController;
        private readonly ProxyController proxyController;
        private readonly IMessageService messageService;
        private readonly IDataService dataService;

        private readonly FloatingViewModel floatingViewModel;
        private readonly MainViewModel mainViewModel;
        private readonly UserBugsViewModel userBugsViewModel;
        private readonly TeamBugsViewModel teamBugsViewModel;

        private readonly DelegateCommand showMainWindowCommand;
        private readonly DelegateCommand englishCommand;
        private readonly DelegateCommand chineseCommand;
        private readonly DelegateCommand aboutCommand;
        private readonly DelegateCommand settingCommand;
        private readonly DelegateCommand exitCommand;

        private CultureInfo newLanguage;
        #endregion

        [ImportingConstructor]
        public ApplicationController(CompositionContainer container, IPresentationService presentationService,
            IMessageService messageService, ShellService shellService, ProxyController proxyController, DataController dataController)
        {
            InitializeCultures();
            presentationService.InitializeCultures();

            this.container = container;
            this.dataController = dataController;
            this.proxyController = proxyController;
            this.messageService = messageService;

            this.dataService = container.GetExportedValue<IDataService>();

            this.floatingViewModel = container.GetExportedValue<FloatingViewModel>();
            this.mainViewModel = container.GetExportedValue<MainViewModel>();
            this.userBugsViewModel = container.GetExportedValue<UserBugsViewModel>();
            this.teamBugsViewModel = container.GetExportedValue<TeamBugsViewModel>();

            shellService.MainView = mainViewModel.View;
            shellService.UserBugsView = userBugsViewModel.View;
            shellService.TeamBugsView = teamBugsViewModel.View;

            this.floatingViewModel.Closing += FloatingViewModelClosing;

            this.showMainWindowCommand = new DelegateCommand(ShowMainWindowCommandExcute);
            this.englishCommand = new DelegateCommand(() => SelectLanguage(new CultureInfo("en-US")));
            this.chineseCommand = new DelegateCommand(() => SelectLanguage(new CultureInfo("zh-CN")));
            this.settingCommand = new DelegateCommand(SettingDialogCommandExcute, CanSettingDialogCommandExcute);
            this.aboutCommand = new DelegateCommand(AboutDialogCommandExcute);
            this.exitCommand = new DelegateCommand(ExitCommandExcute);
        }

        #region Implement Controller base class
        protected override void OnInitialize()
        {
            this.floatingViewModel.ShowMainWindowCommand = this.showMainWindowCommand;
            this.floatingViewModel.EnglishCommand = this.englishCommand;
            this.floatingViewModel.ChineseCommand = this.chineseCommand;
            this.floatingViewModel.SettingCommand = this.settingCommand;
            this.floatingViewModel.AboutCommand = this.aboutCommand;
            this.floatingViewModel.ExitCommand = this.exitCommand;

            this.mainViewModel.EnglishCommand = this.englishCommand;
            this.mainViewModel.ChineseCommand = this.chineseCommand;
            this.mainViewModel.SettingCommand = this.settingCommand;
            this.mainViewModel.AboutCommand = this.aboutCommand;
            this.mainViewModel.ExitCommand = this.exitCommand;

            this.proxyController.Initialize();
            this.dataController.Initialize();
            this.proxyController.ActiveProxyInitializeTask
                .ContinueWith(task =>
                {
                    dataService.InitializeStatus = Models.InitializeStatus.Done;
                    UpdateCommands();

                    if (task.Result)
                    {
                        this.dataController.TimerStart();
                    }
                }, CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void ShutDown()
        {
            this.dataController.TimerStop();
            this.dataController.Shutdown();

            if (this.newLanguage != null)
            {
                Settings.Default.UICulture = this.newLanguage.Name;
                Settings.Default.Culture = this.newLanguage.Name;
            }
            try
            {
                Settings.Default.Save();
            }
            catch (Exception)
            {
                // When more application instances are closed at the same time then an exception occurs.
            }
        }
        #endregion

        #region Implement IApplicationController interface
        public void Run()
        {
            this.floatingViewModel.Show();
        }
        #endregion

        #region Properties
        internal CultureInfo NewLanguage
        {
            get { return this.newLanguage; }
        }
        #endregion

        #region Methods
        #region Commands Methods
        private void ShowMainWindowCommandExcute()
        {
            this.mainViewModel.Show();
        }

        private void ExitCommandExcute()
        {
            this.mainViewModel.IsShutDown = true;

            this.floatingViewModel.Close();
            this.mainViewModel.Close();
        }

        private void AboutDialogCommandExcute()
        {
            IAboutDialogView view = container.GetExportedValue<IAboutDialogView>();
            AboutDialogViewModel aboutDialog = new AboutDialogViewModel(view);
            aboutDialog.ShowDialog(this.floatingViewModel.View);
        }

        private void SettingDialogCommandExcute()
        {
            var settingDialogView = container.GetExportedValue<ISettingDialogView>();
            var settingsView = container.GetExportedValue<ISettingsView>();
            var proxyService = this.proxyController.ProxyService;

            this.dataController.TimerStop();

            SettingsViewModel settingsViewModel = new SettingsViewModel(settingsView, proxyService, Settings.Default.TeamMembers);
            settingsViewModel.ActiveProxy = proxyService.ActiveProxy.ProxyName;
            settingsViewModel.UserName = Settings.Default.UserName;
            settingsViewModel.RefreshMinutes = Settings.Default.AutoQueryMinutes;
            settingsViewModel.IsFilterCreatedBy = Settings.Default.IsFilterCreatedBy;
            settingsViewModel.FilterStatusValues = Settings.Default.FilterStatusValues;
            settingsViewModel.FloatingWindowOpacity = Settings.Default.FloatingWindowOpacity;

            SettingDialogViewModel settingDialog = new SettingDialogViewModel(settingDialogView, proxyService, this.messageService, settingsViewModel);

            bool? result = settingDialog.ShowDialog(this.floatingViewModel.View);

            if (result == true)
            {
                proxyService.ActiveProxy = proxyService.Proxies.First(x => x.ProxyName == settingsViewModel.ActiveProxy);

                Settings.Default.ActiveProxy = settingsViewModel.ActiveProxy;
                Settings.Default.UserName = settingsViewModel.UserName;
                Settings.Default.AutoQueryMinutes = settingsViewModel.RefreshMinutes;
                Settings.Default.TeamMembers = settingsViewModel.TeamMembersString;
                Settings.Default.IsFilterCreatedBy = settingsViewModel.IsFilterCreatedBy;
                Settings.Default.FilterStatusValues = settingsViewModel.FilterStatusValues;
                Settings.Default.FloatingWindowOpacity = settingsViewModel.FloatingWindowOpacity;
                Settings.Default.Save();
            }

            this.dataController.TimerStart();

            floatingViewModel.Opacity = Settings.Default.FloatingWindowOpacity;
        }

        private bool CanSettingDialogCommandExcute()
        {
            return dataService.InitializeStatus == Models.InitializeStatus.Done;
        }
        #endregion

        #region Private Methods
        private static void InitializeCultures()
        {
            if (!String.IsNullOrEmpty(Settings.Default.Culture))
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Settings.Default.Culture);
            }
            if (!String.IsNullOrEmpty(Settings.Default.UICulture))
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Settings.Default.UICulture);
            }
        }

        private void FloatingViewModelClosing(object sender, CancelEventArgs e)
        {
        }

        private void SelectLanguage(CultureInfo uiCulture)
        {
            if (!uiCulture.Equals(CultureInfo.CurrentUICulture))
            {
                messageService.ShowMessage(this.floatingViewModel.View, Resources.RestartApplication + "\n\n" +
                    Resources.ResourceManager.GetString("RestartApplication", uiCulture));
            }
            this.newLanguage = uiCulture;
        }

        private void UpdateCommands()
        {
            this.settingCommand.RaiseCanExecuteChanged();
        }
        #endregion
        #endregion
    }
}
