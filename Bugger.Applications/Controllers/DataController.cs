using BigEgg.Framework.Applications.Commands;
using BigEgg.Framework.Applications.Services;
using BigEgg.Framework.Applications.ViewModels;
using Bugger.Applications.Properties;
using Bugger.Applications.Services;
using Bugger.Applications.ViewModels;
using System;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Bugger.Proxy;
using Bugger.Applications.Models;

namespace Bugger.Applications.Controllers
{
    [Export]
    internal class DataController : Controller
    {
        #region Members
        private readonly CompositionContainer container;
        private readonly IMessageService messageService;
        private readonly DataService dataService;
        private readonly ProxyController proxyController;

        private readonly DelegateCommand refreshBugsCommand;

        private TaskScheduler currentSynchronizationTaskScheduler;
        private Timer autoRefreshTimer = null;
        private bool timerStarted;
        #endregion

        [ImportingConstructor]
        public DataController(CompositionContainer container, IMessageService messageService,
            DataService dataService, ProxyController proxyController)
        {
            this.container = container;
            this.messageService = messageService;
            this.dataService = dataService;
            this.proxyController = proxyController;

            this.refreshBugsCommand = new DelegateCommand(RefreshBugsCommandExecute, CanRefreshBugsCommandExecute);

            this.timerStarted = false;

            AddWeakEventListener(this.proxyController.ProxyService, ProxyServicePropertyChanged);
        }

        #region Implement Controller base class
        protected override void OnInitialize()
        {
            FloatingViewModel floatingViewModel = this.container.GetExportedValue<FloatingViewModel>();
            MainViewModel mainViewModel = this.container.GetExportedValue<MainViewModel>();

            floatingViewModel.RefreshBugsCommand = this.refreshBugsCommand;
            mainViewModel.RefreshBugsCommand = this.refreshBugsCommand;

            currentSynchronizationTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();

            TimerStart();
        }

        public void Shutdown()
        {
            TimerStop();
        }
        #endregion

        #region Properties
        private ITracingSystemProxy ActiveProxy
        {
            get { return this.proxyController.ProxyService.ActiveProxy; }
        }
        #endregion

        #region Methods
        #region Public Methods
        public void TimerStart()
        {
            if (this.timerStarted)
                return;

            if (!CanRefreshBugsCommandExecute())
                return;

            // Create an inferred delegate that invokes methods for the timer.
            TimerCallback tcb = TimerCallbackMethods;
            this.autoRefreshTimer = new Timer(tcb, null, 10000, Settings.Default.AutoQueryMinutes * 1000 * 1000);

            this.dataService.UserBugsQueryState = QueryStatus.NotWorking;
            this.dataService.TeamBugsQueryState = QueryStatus.NotWorking;
            this.dataService.UserBugsProgressValue = 0;
            this.dataService.TeamBugsProgressValue = 0;
            this.timerStarted = true;
        }

        public void TimerStop()
        {
            if (!this.timerStarted) { return; }

            if (this.autoRefreshTimer != null)
            {
                this.autoRefreshTimer.Change(0, Timeout.Infinite);
                this.autoRefreshTimer.Dispose();
                this.autoRefreshTimer = null;
            }

            this.dataService.UserBugsQueryState = QueryStatus.QureyPause;
            this.dataService.TeamBugsQueryState = QueryStatus.QureyPause;
            this.dataService.UserBugsProgressValue = 100;
            this.dataService.TeamBugsProgressValue = 100;
            this.timerStarted = false;
        }
        #endregion

        #region Commands Methods
        private bool CanRefreshBugsCommandExecute()
        {
            return this.ActiveProxy == null ? false : this.ActiveProxy.CanQuery;
        }

        private void RefreshBugsCommandExecute()
        {
            this.dataService.UserBugsQueryState = QueryStatus.NotWorking;
            this.dataService.TeamBugsQueryState = QueryStatus.NotWorking;
            this.dataService.RefreshTime = DateTime.Now;

            if (!string.IsNullOrWhiteSpace(Settings.Default.UserName))
            {
                this.dataService.UserBugsQueryState = QueryStatus.Qureying;
                this.dataService.UserBugsProgressValue = 0;

                Task.Factory.StartNew(() => this.ActiveProxy.Query(
                                            Settings.Default.UserName,
                                            Settings.Default.IsFilterCreatedBy))
                .ContinueWith(task =>
                {
                    this.dataService.UserBugsQueryState = QueryStatus.FillData;
                    this.dataService.UserBugsProgressValue = 50;
                    return task.Result;
                }, CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, currentSynchronizationTaskScheduler)
                .ContinueWith(task =>
                {
                    this.dataService.UserBugs.Clear();
                    if (task.Result.Where(x => Settings.Default.FilterStatusValues.Contains(x.State)).Any())
                    {
                        int interval = 50 / task.Result.Where(x => Settings.Default.FilterStatusValues.Contains(x.State))
                                                       .Count();
                        foreach (var bug in task.Result.Where(x => Settings.Default.FilterStatusValues.Contains(x.State)))
                        {
                            this.dataService.UserBugsProgressValue += interval;
                            this.dataService.UserBugs.Add(bug);
                        }
                    }
                }, CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, currentSynchronizationTaskScheduler)
                .ContinueWith(task =>
                {
                    this.dataService.UserBugsQueryState = QueryStatus.Success;
                    this.dataService.UserBugsProgressValue = 100;
                }, CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, currentSynchronizationTaskScheduler);
            }
            else
            {
                this.dataService.UserBugsQueryState = QueryStatus.Failed;
                this.dataService.UserBugsProgressValue = 100;
            }

            if (!string.IsNullOrWhiteSpace(Settings.Default.TeamMembers))
            {
                this.dataService.TeamBugsQueryState = QueryStatus.Qureying;
                this.dataService.TeamBugsProgressValue = 0;

                Task.Factory.StartNew(() => this.ActiveProxy.Query(
                                            Settings.Default.TeamMembers.Split(';').Select(x => x.Trim()).ToList(),
                                            Settings.Default.IsFilterCreatedBy))
                .ContinueWith(task =>
                {
                    this.dataService.TeamBugsQueryState = QueryStatus.FillData;
                    this.dataService.TeamBugsProgressValue = 50;
                    return task.Result;
                }, CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, currentSynchronizationTaskScheduler)
                .ContinueWith(task =>
                {
                    this.dataService.TeamBugs.Clear();
                    if (task.Result.Where(x => Settings.Default.FilterStatusValues.Contains(x.State)).Any())
                    {
                        int interval = 50 / task.Result.Where(x => Settings.Default.FilterStatusValues.Contains(x.State))
                                                            .Count();
                        foreach (var bug in task.Result.Where(x => Settings.Default.FilterStatusValues.Contains(x.State)))
                        {
                            this.dataService.TeamBugsProgressValue += interval;
                            this.dataService.TeamBugs.Add(bug);
                        }
                    }
                }, CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, currentSynchronizationTaskScheduler)
                .ContinueWith(task =>
                {
                    this.dataService.TeamBugsQueryState = QueryStatus.Success;
                    this.dataService.TeamBugsProgressValue = 100;
                }, CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, currentSynchronizationTaskScheduler);
            }
            else
            {
                this.dataService.UserBugsQueryState = QueryStatus.Failed;
                this.dataService.UserBugsProgressValue = 100;
            }
        }
        #endregion

        #region Private Methods
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
