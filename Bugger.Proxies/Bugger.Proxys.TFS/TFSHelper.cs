using Bugger.Proxy.TFS.Models;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;

namespace Bugger.Proxy.TFS
{
    public static class TFSHelper
    {
        private static TfsTeamProjectCollection tfsProjectCache;

        public static bool TestConnection(Uri connectUri, string userName, string password)
        {
            try
            {
                TfsTeamProjectCollection tpc = new TfsTeamProjectCollection(connectUri, new NetworkCredential(userName, password));
                tpc.EnsureAuthenticated();
                tfsProjectCache = tpc;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsConnected()
        {
            return tfsProjectCache == null;
        }

        public static FieldDefinitionCollection GetFields(Uri connectUri, string userName, string password)
        {
            if (!IsConnected()) { throw new NotSupportedException(); }

            try
            {
                WorkItemStore workItemStore = (WorkItemStore)tfsProjectCache.GetService(typeof(WorkItemStore));
                return workItemStore.FieldDefinitions;
            }
            catch
            {
                return null;
            }
        }

        public static WorkItemCollection GetBugs(string userName, bool isFilterCreatedBy, ObservableDictionary<string, string> propertyMappingList, string bugFilterField, string bugFilterValue)
        {
            if (!IsConnected()) { throw new NotSupportedException(); }

            try
            {
                WorkItemStore workItemStore = (WorkItemStore)tfsProjectCache.GetService(typeof(WorkItemStore));

                string fields = string.Join(", ", propertyMappingList.Where(x => !string.IsNullOrWhiteSpace(x.Value))
                                                                     .Select(x => "[" + x.Value + "]"));
                string filter = "[" + propertyMappingList["AssignedTo"] + "] = '" + userName + "'";

                if (isFilterCreatedBy)
                {
                    filter = "( " + filter + " OR [" + propertyMappingList["CreatedBy"] + "] = '" + userName + "' )";
                }

                filter = "[" + bugFilterField + "] = '" + bugFilterValue + "' And " + filter;
                string queryString = "SELECT " + fields + " FROM WorkItems WHERE " + filter;

                Query query = new Query(workItemStore, queryString);
                return query.RunQuery();
            }
            catch
            {
                return null;
            }
        }
    }
}
