using Bugger.Base.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Bugger.Base.PlugIns.Proxies
{
    public interface ITracingSystemProxy
    {
        #region Proxy Methods
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
