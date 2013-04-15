using BigEgg.Framework.Applications.Commands;
using BigEgg.Framework.Applications.Services;
using BigEgg.Framework.Applications.ViewModels;
using Bugger.Applications.Properties;
using Bugger.Applications.Services;
using Bugger.Applications.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Bugger.Applications.Controllers
{
    [Export]
    internal class DataController : Controller
    {
        #region Members
        private readonly CompositionContainer container;
        private readonly IMessageService messageService;
        private readonly DataService dataService;
        private readonly ProxyService proxyService;

        private readonly DelegateCommand refreshBugsCommand;

        private Timer autoRefreshTimer = null;
        private bool timerStarted;
        #endregion

        [ImportingConstructor]
        public DataController(CompositionContainer container, IMessageService messageService,
            DataService dataService, ProxyService proxyService)
        {
            this.container = container;
            this.messageService = messageService;
            this.dataService = dataService;
            this.proxyService = proxyService;

            this.refreshBugsCommand = new DelegateCommand(RefreshBugsCommandExecute, CanRefreshBugsCommandExecute);

            this.timerStarted = false;

            AddWeakEventListener(this.proxyService, ProxyServicePropertyChanged);
        }

        #region Implement Controller base class
        protected override void OnInitialize()
        {
            FloatingViewModel floatingViewModel = this.container.GetExportedValue<FloatingViewModel>();
            MainViewModel mainViewModel = this.container.GetExportedValue<MainViewModel>();

            floatingViewModel.RefreshBugsCommand = this.refreshBugsCommand;
            mainViewModel.RefreshBugsCommand = this.refreshBugsCommand;

            TimerStart();
        }

        public void Shutdown()
        {
            TimerStop();
        }
        #endregion

        #region Methods
        #region Private Methods
        #region Commands Methods
        private bool CanRefreshBugsCommandExecute()
        {
            return this.proxyService.ActiveProxy != null || this.proxyService.ActiveProxy.CanQuery();
        }

        private void RefreshBugsCommandExecute()
        {
            if (!string.IsNullOrWhiteSpace(Settings.Default.UserName))
            {
                Task.Factory.StartNew(
                    () => this.proxyService.ActiveProxy.Query(
                        Settings.Default.UserName,
                        Settings.Default.IsFilterCreatedBy))
                .ContinueWith((result) =>
                {
                    this.dataService.UserBugs.Clear();
                    foreach (var bug in result.Result)
                        this.dataService.UserBugs.Add(bug);
                }, TaskContinuationOptions.OnlyOnRanToCompletion);
            }

            if (!string.IsNullOrWhiteSpace(Settings.Default.TeamMembers))
            {
                Task.Factory.StartNew(
                    () => this.proxyService.ActiveProxy.Query(
                        Settings.Default.TeamMembers,
                        Settings.Default.IsFilterCreatedBy))
                .ContinueWith((result) =>
                {
                    this.dataService.TeamBugs.Clear();
                    foreach (var bug in result.Result)
                        this.dataService.TeamBugs.Add(bug);
                }, TaskContinuationOptions.OnlyOnRanToCompletion);
            }

            Task.WaitAll();

            this.dataService.RefreshTime = DateTime.Now;
        }
        #endregion

        private void TimerStart()
        {
            if (this.timerStarted)
                return;

            if (!CanRefreshBugsCommandExecute())
                return;

            // Create an inferred delegate that invokes methods for the timer.
            TimerCallback tcb = TimerCallbackMethods;
            this.autoRefreshTimer = new Timer(tcb, null, 0, Settings.Default.AutoQueryMinutes * 1000);

            this.timerStarted = true;
        }

        private void TimerStop()
        {
            if (!this.timerStarted)
                return;

            if (this.autoRefreshTimer != null)
            {
                this.autoRefreshTimer.Change(0, Timeout.Infinite);
                this.autoRefreshTimer.Dispose();
                this.autoRefreshTimer = null;
            }

            this.timerStarted = false;
        }

        // This method is called by the timer delegate.
        private void TimerCallbackMethods(Object obj)
        {
            try
            {
                if (this.refreshBugsCommand.CanExecute())
                {
                    this.refreshBugsCommand.Execute();
                }
            }
            catch
            {
                throw;
            }
        }

        private void ProxyServicePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ActiveProxy")
            {
                UpdateCommands();

                TimerStop();
                if (CanRefreshBugsCommandExecute() && !this.timerStarted)
                {
                    TimerStart();
                }
            }
        }

        private void UpdateCommands()
        {
            this.refreshBugsCommand.RaiseCanExecuteChanged();
        }
        #endregion
        #endregion
    }
}
