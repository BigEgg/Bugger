using Bugger.Base.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Bugger.Base.PlugIns.TrackingSystem
{
    public interface ITrackingSystemPlugIn
    {
        /// <summary>
        /// Get the status of this tracking system.
        /// </summary>
        /// <returns>The status of this tracking system Plug-in.</returns>
        TrackingSystemStatus GetStatus();

        /// <summary>
        /// Query the bugs with the specified user name which the bug assign to.
        /// </summary>
        /// <param name="userName">The user name which the bug assign to.</param>
        /// <param name="isFilterCreatedBy">if set to <c>true</c> indicating also filter the createdBy field, otherwise <c>false</c>.</param>
        /// <returns>
        /// The bugs.
        /// </returns>
        ReadOnlyCollection<Bug> Query(string userName, bool isFilterCreatedBy = true);

        /// <summary>
        /// Query the bugs with the specified team members name list which the bug assign to.
        /// </summary>
        /// <param name="teamMembers">The team members name list which the bug assign to.</param>
        /// <returns>
        /// The bugs.
        /// </returns>
        ReadOnlyCollection<Bug> Query(List<string> teamMembers);
    }
}
