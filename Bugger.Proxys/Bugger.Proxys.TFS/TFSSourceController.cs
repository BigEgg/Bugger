using Bugger.Proxys.Models;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System.Collections.Generic;
using System.ComponentModel.Composition;
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
            this.queryFields.Add("[System.Id]");
            this.queryFields.Add("[System.WorkItemType]");
            this.queryFields.Add("[System.Title]");
            this.queryFields.Add("[System.Description]");
            this.queryFields.Add("[System.AssignedTo]");
            this.queryFields.Add("[System.State]");
            this.queryFields.Add("[System.ChangedDate]");
            this.queryFields.Add("[System.CreatedBy]");
        }

        #region Methods
        #region Protected Methods
        /// <summary>
        /// The core function of query the work items with the specified user name.
        /// </summary>
        /// <param name="userName">The user name.</param>
        protected override List<Bug> QueryCore(NetworkCredential credential, string userName)
        {
            TfsTeamProjectCollection tpc = new TfsTeamProjectCollection(this.ConnectUri, credential);
            tpc.EnsureAuthenticated();

            WorkItemStore workItemStore = (WorkItemStore)tpc.GetService(typeof(WorkItemStore));

            string fields = string.Join(", ", this.queryFields);
            string filter = this.IsFilterCreatedByFilter ?
                "[System.AssignedTo] = '" + userName + "' OR [System.CreatedBy] = '" + userName + "'" :
                "[System.AssignedTo] = '" + userName + "'";
            string queryString = "Select " + fields + " From WorkItems Where " + filter;

            Query query = new Query(workItemStore, queryString);
            WorkItemCollection collection = query.RunQuery();

            List<Bug> result = new List<Bug>();
            foreach (WorkItem item in collection)
            {
                result.Add(new Bug()
                {
                    ID           = item.Id,
                    WorkItemType = item.Type == null ? string.Empty : item.Type.Name,
                    Title        = item.Title,
                    Description  = item.Description,
                    AssignedTo   = item.Fields["Assigned To"].Value.ToString(),
                    State        = item.State,
                    ChangedDate  = item.ChangedDate,
                    CreatedBy    = item.CreatedBy,
                    Priority     = item.Fields["Priority"].Value.ToString(),
                    Severity     = item.Fields.Contains("Severity") ? 
                                        item.Fields["Severity"].Value.ToString() : string.Empty
                });
            }
            return result;
        }
        #endregion
        #endregion
    }
}
