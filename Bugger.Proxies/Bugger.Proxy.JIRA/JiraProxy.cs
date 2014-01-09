using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BigEgg.Framework.Applications.Services;
using Bugger.Domain.Models;
using Bugger.Proxy.Jira.Properties;

namespace Bugger.Proxy.Jira
{
    [Export(typeof(ITracingSystemProxy)), Export]
    public class JiraProxy : TracingSystemProxyBase
    {
        #region Fields
        private readonly CompositionContainer container;
        private readonly IMessageService messageService;
        private readonly JiraHelper jiraHelper;

        private readonly ObservableCollection<string> stateValues;

        #endregion


        [ImportingConstructor]
        public JiraProxy(CompositionContainer container, IMessageService messageService, JiraHelper jiraHelper)
            : base(Resources.ProxyName)
        {
            if (container == null) { throw new ArgumentNullException("container"); }
            if (messageService == null) { throw new ArgumentNullException("messageService"); }
            if (jiraHelper == null) { throw new ArgumentNullException("jiraHelper"); }

            this.container = container;
            this.messageService = messageService;
            this.jiraHelper = jiraHelper;

        }

        #region Properties
        /// <summary>
        /// Gets the status values.
        /// </summary>
        /// <value>
        /// The status values.
        /// </value>
        public override ObservableCollection<string> StateValues { get { return this.stateValues; } }
        #endregion

        #region Methods
        #region Public Methods
        /// <summary>
        /// Initializes the setting dialog.
        /// </summary>
        /// <returns></returns>
        public override ISettingView InitializeSettingDialog()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Do something afters close setting dialog.
        /// </summary>
        /// <param name="submit">If the dialog is return as submit..</param>
        public override void AfterCloseSettingDialog(bool submit)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Validate the setting values before close setting dialog.
        /// </summary>
        /// <returns>
        /// The validation result.
        /// </returns>
        public override SettingDialogValidateionResult ValidateBeforeCloseSettingDialog()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// The method which will execute when the Controller.Initialize() execute.
        /// </summary>
        protected override void OnInitialize()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The core functionality query the bugs with the specified user name which should be query.
        /// </summary>
        /// <param name="userNames">The user names list which the bug assign to.</param>
        /// <param name="isFilterCreatedBy">if set to <c>true</c> indicating whether filter the created by field.</param>
        /// <returns>
        /// The bugs.
        /// </returns>
        protected override ReadOnlyCollection<Bug> QueryCore(List<string> userNames, bool isFilterCreatedBy)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Private Methods
        
        #endregion
        #endregion
    }
}
