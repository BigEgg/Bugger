using BigEgg.Framework.Applications.ViewModels;
using Bugger.Domain.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Bugger.Proxy
{
    /// <summary>
    /// The interface of the application proxy for source control system.
    /// </summary>
    public interface ISourceControlProxy
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
        /// Gets the setting view.
        /// </summary>
        /// <value>
        /// The setting view.
        /// </value>
        object SettingView { get; }
        #endregion

        #region Methods
        void Initialize();

        /// <summary>
        /// Determines whether this source control proxy can query the bugs.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this source control proxy can query the bugs.; otherwise, <c>false</c>.
        /// </returns>
        bool CanQuery();

        /// <summary>
        /// Query the bugs with the specified user name which the bug assign to.
        /// </summary>
        /// <param name="userName">The user name which the bug assign to.</param>
        /// <param name="isFilterCreatedBy">if set to <c>true</c> indicating whether filter the created by field.</param>
        /// <returns>
        /// The bugs.
        /// </returns>
        ReadOnlyCollection<Bug> Query(string userName, bool isFilterCreatedBy = true);

        /// <summary>
        /// Query the bugs with the specified team members name list which the bug assign to.
        /// </summary>
        /// <param name="teamMembers">The team members name list which the bug assign to.</param>
        /// <param name="isFilterCreatedBy">if set to <c>true</c> indicating whether filter the created by field.</param>
        /// <returns>
        /// The bugs.
        /// </returns>
        ReadOnlyCollection<Bug> Query(List<string> teamMembers, bool isFilterCreatedBy = false);
        #endregion
    }
}
