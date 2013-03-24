using Bugger.Proxys.Models;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Net;

namespace Bugger.Proxys.TFS
{
    [Export(typeof(ISourceController)), Export]
    public class TFSSourceController : SourceController
    {
        #region Fields
        private readonly List<string> queryFields;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="TFSSrouceController" /> class.
        /// </summary>
        /// <param name="serverName">The name of the server that is running the application tier for Team Foundation.</param>
        /// <param name="port">The port that Team Foundation uses.</param>
        /// <param name="virtualPath">the virtual path to the Team Foundation application. The default path is tfs.</param>
        public TFSSourceController(string serverName, uint port = 8080, string virtualPath = "tfs")
            : base(serverName, port, virtualPath)
        {
            this.queryFields = new List<string>();
            this.queryFields.Add("[ID]");
            this.queryFields.Add("[Title]");
            this.queryFields.Add("[Description]");
            this.queryFields.Add("[Assigned To]");
            this.queryFields.Add("[State]");
            this.queryFields.Add("[Changed Date]");
            this.queryFields.Add("[Created By]");
        }

        #region Methods
        #region Protected Methods
        /// <summary>
        /// The core functionality query the bugs with the specified user name which should be query.
        /// </summary>
        /// <param name="credential">The credential information.</param>
        /// <param name="userName">The user name which should be query.</param>
        /// <param name="workItemFilter">The string to filter the bugs.</param>
        /// <returns>
        /// The bugs.
        /// </returns>
        protected override List<Bug> QueryCore(NetworkCredential credential, string userName, string workItemFilter)
        {
            TfsTeamProjectCollection tpc = new TfsTeamProjectCollection(this.ConnectUri, credential);
            tpc.EnsureAuthenticated();

            WorkItemStore workItemStore = (WorkItemStore)tpc.GetService(typeof(WorkItemStore));

            string fields = string.Join(", ", this.queryFields);
            string filter = this.IsFilterCreatedByFilter ?
                "[System.AssignedTo] = '" + userName + "' OR [System.CreatedBy] = '" + userName + "'" :
                "[System.AssignedTo] = '" + userName + "'";
            if (!string.IsNullOrWhiteSpace(workItemFilter))
            {
                filter = "[WorkItemType] = '" + workItemFilter + "' And " + filter;
            }
            string queryString = "Select " + fields + " From WorkItems Where " + filter;

            Query query = new Query(workItemStore, queryString);
            WorkItemCollection collection = query.RunQuery();

            List<Bug> result = new List<Bug>();
            foreach (WorkItem item in collection)
            {
                result.Add(new Bug()
                {
                    ID          = (int)item.Fields["ID"].Value,
                    Title       = item.Fields["Title"].Value.ToString(),
                    Description = item.Fields["Description"].Value.ToString(),
                    AssignedTo  = item.Fields["Assigned To"].Value.ToString(),
                    State       = item.Fields["State"].Value.ToString(),
                    ChangedDate = (DateTime)item.Fields["Changed Date"].Value,
                    CreatedBy   = item.Fields["Created By"].Value.ToString(),
                    Priority    = item.Fields["Code Studio Rank"].Value.ToString(),
                    Severity    = item.Fields.Contains("Severity") ?
                                        item.Fields["Severity"].Value.ToString() : string.Empty
                });
            }
            return result;
        }
        #endregion
        #endregion
    }
}
