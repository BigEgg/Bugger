using BigEgg.Framework.Applications.Services;
using BigEgg.Framework.Applications.ViewModels;
using Bugger.Applications.Properties;
using Bugger.Applications.Services;
using System;
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

            this.timerStarted = false;
        }

        #region Implement Controller base class
        protected override void OnInitialize()
        {
            TimerStart();
        }

        public void Shutdown()
        {
            TimerStop();
        }
        #endregion

        #region Methods
        #region Public Methods
        private void TimerStart()
        {
            if (this.timerStarted)
                return;

            if (this.proxyService.ActiveProxy == null)
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
        #endregion

        #region Private Methods
        // This method is called by the timer delegate.
        private void TimerCallbackMethods(Object obj)
        {
            try
            {
                RefreshBugs();
            }
            catch
            {
                throw;
            }
        }

        public void RefreshBugs()
        {
            if (this.proxyService.ActiveProxy == null)
                return;

            Task queryUserTask = null;
            if (!string.IsNullOrWhiteSpace(Settings.Default.UserName))
            {
                queryUserTask = new Task(
                    () => this.proxyService.ActiveProxy.Query(
                        Settings.Default.UserName,
                        Settings.Default.IsFilterCreatedBy));
                queryUserTask.Start();
            }


            this.dataService.RefreshTime = DateTime.Now;
        }
        #endregion
        #endregion
    }
}
