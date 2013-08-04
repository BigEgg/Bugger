using Bugger.Domain.Models;
using Bugger.Proxy.TFS.Models;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;

namespace Bugger.Proxy.TFS
{
    [Export]
    public class TFSHelper
    {
        private TfsTeamProjectCollection tfsProjectCache;

        public bool TestConnection(Uri connectUri, string userName, string password)
        {
            if (connectUri == null) { throw new ArgumentNullException("connectUri"); }
            if (string.IsNullOrWhiteSpace(userName)) { throw new ArgumentException("userName"); }

            try
            {
                this.tfsProjectCache = null;
                TfsTeamProjectCollection tpc = new TfsTeamProjectCollection(
                    connectUri, new NetworkCredential(userName, password));
                tpc.EnsureAuthenticated();
                this.tfsProjectCache = tpc;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool IsConnected()
        {
            return this.tfsProjectCache != null;
        }

        public List<TFSField> GetFields()
        {
            if (!IsConnected())
            {
                throw new InvalidOperationException("The method cannot be executed because the IsConnected returned false.");
            }

            try
            {
                WorkItemStore workItemStore = (WorkItemStore)this.tfsProjectCache.GetService(typeof(WorkItemStore));
                var collection = workItemStore.FieldDefinitions;

                var fields = new List<TFSField>();
                foreach (FieldDefinition field in collection)
                {
                    TFSField tfsField = new TFSField(field.Name);
                    foreach (var value in field.AllowedValues)
                    {
                        tfsField.AllowedValues.Add(value.ToString());
                    }
                    fields.Add(tfsField);
                }

                return fields;
            }
            catch
            {
                return null;
            }
        }

        public List<Bug> GetBugs(
            string userName, bool isFilterCreatedBy, IDictionary<string, string> propertyMappingList,
            string bugFilterField, string bugFilterValue, IEnumerable<string> redFilter)
        {
            if (string.IsNullOrWhiteSpace(userName)) { throw new ArgumentException("userName"); }
            if (propertyMappingList == null) { throw new ArgumentNullException("propertyMappingList"); }
            if (string.IsNullOrWhiteSpace(bugFilterField)) { throw new ArgumentException("usebugFilterFieldrName"); }
            if (string.IsNullOrWhiteSpace(bugFilterValue)) { throw new ArgumentException("bugFilterValue"); }
            if (redFilter == null) { throw new ArgumentNullException("redFilter"); }

            if (!IsConnected())
            {
                throw new InvalidOperationException("The method cannot be executed because the IsConnected returned false.");
            }

            try
            {
                WorkItemStore workItemStore = (WorkItemStore)this.tfsProjectCache.GetService(typeof(WorkItemStore));

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
                var collection = query.RunQuery();

                if (collection == null)
                    return null;

                var bugs = new List<Bug>();
                foreach (WorkItem item in collection)
                {
                    bugs.Add(Map(item, propertyMappingList, redFilter));
                }

                return bugs;
            }
            catch
            {
                return null;
            }
        }


        private Bug Map(WorkItem workitem, IDictionary<string, string> propertyMappingList, IEnumerable<string> redFilter)
        {
            Bug bug = new Bug();
            object value = null;

            //  ID
            value = workitem.Fields[propertyMappingList["ID"]].Value;
            bug.ID = value == null ? 0 : (int)value;

            //  Title
            value = workitem.Fields[propertyMappingList["Title"]].Value;
            bug.Title = value == null ? string.Empty : value.ToString();

            //  Description
            value = workitem.Fields[propertyMappingList["Description"]].Value;
            bug.Description = value == null ? string.Empty : value.ToString();

            //  AssignedTo
            value = workitem.Fields[propertyMappingList["AssignedTo"]].Value;
            bug.AssignedTo = value == null ? string.Empty : value.ToString();

            //  State
            value = workitem.Fields[propertyMappingList["State"]].Value;
            bug.State = value == null ? string.Empty : value.ToString();

            //  ChangedDate
            value = workitem.Fields[propertyMappingList["ChangedDate"]].Value;
            bug.ChangedDate = value == null ? DateTime.Today : (DateTime)value;

            //  CreatedBy
            value = workitem.Fields[propertyMappingList["CreatedBy"]].Value;
            bug.CreatedBy = value == null ? string.Empty : value.ToString();

            //  Priority
            value = workitem.Fields[propertyMappingList["Priority"]].Value;
            bug.Priority = value == null ? string.Empty : value.ToString();

            //  Severity
            if (string.IsNullOrWhiteSpace(propertyMappingList["Severity"]))
            {
                bug.Severity = string.Empty;
            }
            else
            {
                value = workitem.Fields[propertyMappingList["Severity"]].Value;
                bug.Severity = value == null ? string.Empty : value.ToString();
            }

            bug.Type = string.IsNullOrWhiteSpace(bug.Priority)
                           ? BugType.Yellow
                           : (redFilter.Contains(bug.Priority) ? BugType.Red : BugType.Yellow);

            return bug;
        }
    }
}
