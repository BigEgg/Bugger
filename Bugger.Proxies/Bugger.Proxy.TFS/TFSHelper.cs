using Bugger.Models;
using Bugger.Proxy.Models;
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
        /// <summary>
        /// Try to connect the TFS with the specific authentication information
        /// </summary>
        /// <param name="connectUri">The connect URI.</param>
        /// <param name="userName">The user name.</param>
        /// <param name="password">The password.</param>
        /// <param name="tpc">The TFS Team Project Collection.</param>
        /// <returns>
        ///   <c>True</c>, if the TFS can be connected with the specific authentication information;
        ///   otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// connectUri
        /// or
        /// userName
        /// </exception>
        public bool TryConnection(Uri connectUri, string userName, string password, out TfsTeamProjectCollection tpc)
        {
            if (connectUri == null) { throw new ArgumentNullException("connectUri"); }
            if (string.IsNullOrWhiteSpace(userName)) { throw new ArgumentNullException("userName"); }
            if (password == null) { throw new ArgumentNullException("password"); }

            try
            {
                TfsTeamProjectCollection tempTPC = new TfsTeamProjectCollection(
                    connectUri, new NetworkCredential(userName, password));
                tempTPC.EnsureAuthenticated();

                tpc = tempTPC;
                return true;
            }
            catch
            {
                tpc = null;
                return false;
            }
        }

        /// <summary>
        /// Gets the fields' informations from the TFS.
        /// </summary>
        /// <param name="tpc">The TFS Team Project Collection.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">The Team Project Collection class cannot be null.</exception>
        public List<TFSField> GetFields(TfsTeamProjectCollection tpc)
        {
            if (tpc == null) { throw new ArgumentNullException("The Team Project Collection class cannot be null."); }

            try
            {
                WorkItemStore workItemStore = (WorkItemStore)tpc.GetService(typeof(WorkItemStore));
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

        /// <summary>
        /// Gets the bugs from TFS.
        /// </summary>
        /// <param name="tpc">The TFS Team Project Collection.</param>
        /// <param name="userName">The user name.</param>
        /// <param name="isFilterCreatedBy">if set to <c>true</c> [is filter created by].</param>
        /// <param name="propertyMappingList">The property mapping list.</param>
        /// <param name="bugFilterField">The bug filter field.</param>
        /// <param name="bugFilterValue">The bug filter value.</param>
        /// <param name="redFilter">The red filter.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// userName
        /// or
        /// propertyMappingList
        /// or
        /// usebugFilterFieldrName
        /// or
        /// bugFilterValue
        /// or
        /// redFilter
        /// or
        /// The Team Project Collection class cannot be null.
        /// </exception>
        public List<IBug> GetBugs(
            TfsTeamProjectCollection tpc,
            string userName, bool isFilterCreatedBy, PropertyMappingDictionary propertyMappingList,
            string bugFilterField, string bugFilterValue, IEnumerable<string> redFilter)
        {
            if (tpc == null) { throw new ArgumentNullException("The Team Project Collection class cannot be null."); }
            if (string.IsNullOrWhiteSpace(userName)) { throw new ArgumentNullException("userName"); }
            if (propertyMappingList == null) { throw new ArgumentNullException("propertyMappingList"); }
            if (string.IsNullOrWhiteSpace(bugFilterField)) { throw new ArgumentNullException("usebugFilterFieldrName"); }
            if (string.IsNullOrWhiteSpace(bugFilterValue)) { throw new ArgumentNullException("bugFilterValue"); }
            if (redFilter == null) { throw new ArgumentNullException("redFilter"); }

            try
            {
                WorkItemStore workItemStore = (WorkItemStore)tpc.GetService(typeof(WorkItemStore));

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

                if (collection == null) { return null; }

                var bugs = new List<IBug>();
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


        /// <summary>
        /// Maps the specified work item to the bug model.
        /// </summary>
        /// <param name="workItem">The work item.</param>
        /// <param name="propertyMappingList">The property mapping list.</param>
        /// <param name="redFilter">The red bug filter.</param>
        /// <returns></returns>
        private IBug Map(WorkItem workItem, PropertyMappingDictionary propertyMappingList,
                        IEnumerable<string> redFilter)
        {
            string id = MapProperty(workItem, propertyMappingList["ID"]);
            string title = MapProperty(workItem, propertyMappingList["Title"]);
            string description = MapProperty(workItem, propertyMappingList["Description"]);
            string assignedTo = MapProperty(workItem, propertyMappingList["AssignedTo"]);
            string state = MapProperty(workItem, propertyMappingList["State"]);
            DateTime changedDate = MapProperty<DateTime>(workItem, propertyMappingList["ChangedDate"]);
            string createdBy = MapProperty(workItem, propertyMappingList["CreatedBy"]);
            string priority = MapProperty(workItem, propertyMappingList["Priority"]);
            string severity = MapProperty(workItem, propertyMappingList["Severity"], string.Empty);

            BugType bugType = string.IsNullOrWhiteSpace(priority)
                           ? BugType.Yellow
                           : (redFilter.Contains(priority) ? BugType.Red : BugType.Yellow);

            var bug = new Bug(id, title, description, assignedTo, state, changedDate, createdBy, priority, severity);
            bug.Type = bugType;
            return bug;
        }

        /// <summary>
        /// Map the property.
        /// </summary>
        /// <param name="workItem">The work item.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">propertyName cannot be null or empty.</exception>
        /// <exception cref="Bugger.Proxy.TFS.UnableMapBugException">Cannot get the this property.</exception>
        private string MapProperty(WorkItem workItem, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName)) { throw new ArgumentNullException("propertyName cannot be null or empty."); }

            object value = workItem.Fields[propertyName].Value;

            if (value == null) { throw new UnableMapBugException("Cannot get the this property."); }
            else { return value.ToString(); }
        }

        /// <summary>
        /// Map the property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="workItem">The work item.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">propertyName cannot be null or empty.</exception>
        /// <exception cref="Bugger.Proxy.TFS.UnableMapBugException">Cannot get the this property.</exception>
        private T MapProperty<T>(WorkItem workItem, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName)) { throw new ArgumentNullException("propertyName cannot be null or empty."); }

            object value = workItem.Fields[propertyName].Value;

            if (value == null) { throw new UnableMapBugException("Cannot get the this property."); }
            else { return (T)value; }
        }

        /// <summary>
        /// Map the property with default value.
        /// </summary>
        /// <param name="workItem">The work item.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        private string MapProperty(WorkItem workItem, string propertyName, string defaultValue)
        {
            if (string.IsNullOrWhiteSpace(propertyName)) { return defaultValue; }

            object value = workItem.Fields[propertyName].Value;

            if (value == null) { return defaultValue; }
            else { return value.ToString(); }
        }

        /// <summary>
        /// Map the property with default value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="workItem">The work item.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        private T MapProperty<T>(WorkItem workItem, string propertyName, T defaultValue)
            {
            if (string.IsNullOrWhiteSpace(propertyName)) { return defaultValue; }

            object value = workItem.Fields[propertyName].Value;

            if (value == null) { return defaultValue; }
            else { return (T)value; }
        }
    }
}
