using Bugger.Domain.Models;
using Bugger.Domain.ViewModels;
using Bugger.Proxy.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Bugger.Proxy
{
    /// <summary>
    /// The interface of the application proxy for source control system.
    /// </summary>
    public interface ITracingSystemProxy : INotifyPropertyChanged
    {
        #region Properties
        /// <summary>
        /// Gets the name of the proxy.
        /// </summary>
        /// <value>
        /// The name of the proxy.
        /// </value>
        string ProxyName { get; }

        /// <summary>
        /// Gets the status values.
        /// </summary>
        /// <value>
        /// The status values.
        /// </value>
        ObservableCollection<string> StateValues { get; }

        /// <summary>
        /// Determines whether this source control proxy can query the bugs.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this source control proxy can query the bugs.; otherwise, <c>false</c>.
        /// </value>
        bool CanQuery { get; }

        /// <summary>
        /// Gets the name of the bug's view template.
        /// </summary>
        /// <value>
        /// The name of the bug's view template.
        /// </value>
        string BugViewTemplateName { get; }
        #endregion

        #region Methods
        void Initialize();

        /// <summary>
        /// Query the bugs with the specified user name which the bug assign to.
        /// </summary>
        /// <param name="userName">The user name which the bug assign to.</param>
        /// <param name="isFilterCreatedBy">if set to <c>true</c> indicating whether filter the created by field.</param>
        /// <returns>
        /// The bugs.
        /// </returns>
        ReadOnlyCollection<IBug> Query(string userName, bool isFilterCreatedBy = true);

        /// <summary>
        /// Query the bugs with the specified team members name list which the bug assign to.
        /// </summary>
        /// <param name="teamMembers">The team members name list which the bug assign to.</param>
        /// <param name="isFilterCreatedBy">if set to <c>true</c> indicating whether filter the created by field.</param>
        /// <returns>
        /// The bugs.
        /// </returns>
        ReadOnlyCollection<IBug> Query(List<string> teamMembers, bool isFilterCreatedBy = false);


        #region SettingDialog
        /// <summary>
        /// Initializes the values before open the setting dialog.
        /// </summary>
        /// <returns></returns>
        ISettingView InitializeSettingDialog();

        /// <summary>
        /// Do something afters close setting dialog.
        /// </summary>
        /// <param name="submit">if set to <c>true</c> [submit].</param>
        void AfterCloseSettingDialog(bool submit);

        /// <summary>
        /// Validate the setting values before close setting dialog.
        /// </summary>
        /// <returns>The validation result.</returns>
        SettingDialogValidateionResult ValidateBeforeCloseSettingDialog();
        #endregion
        #endregion
    }
}
