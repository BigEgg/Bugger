using BigEgg.Framework.Applications.Commands;
using BigEgg.Framework.Applications.Services;
using BigEgg.Framework.Applications.ViewModels;
using Bugger.Proxys.Models;
using Bugger.Proxys.TFS.Documents;
using Bugger.Proxys.TFS.Properties;
using Bugger.Proxys.TFS.ViewModels;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Net;

namespace Bugger.Proxys.TFS
{
    [Export(typeof(ISourceControlProxy)), Export]
    public class TFSSourceControlProxy : SourceControlProxy
    {
        #region Fields
        private readonly CompositionContainer container;
        private readonly IMessageService messageService;
        private readonly DelegateCommand saveCommand;

        private SettingDocument document;
        private SettingViewModel settingViewModel;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="TFSSourceControlProxy" /> class.
        /// </summary>
        [ImportingConstructor]
        public TFSSourceControlProxy(CompositionContainer container, IMessageService messageService)
            : base(Resources.ProxyName)
        {
            this.container = container;
            this.messageService = messageService;
            this.saveCommand = new DelegateCommand(SaveExcute, CanQuery);

            this.settingViewModel = this.container.GetExportedValue<SettingViewModel>();
            this.settingViewModel.SaveCommand = this.saveCommand;
        }

        #region Properties
        public override ViewModel SettingViewModel { get { return this.settingViewModel; } }
        #endregion

        #region Methods
        #region Public Methods
        public override bool CanQuery()
        {
            return this.settingViewModel.TestConnectionCommand.CanExecute(null)
                && string.IsNullOrWhiteSpace(this.document.BugFilterField)
                && string.IsNullOrWhiteSpace(this.document.BugFilterValue)
                && !this.document.PropertyMappingList.Any(x =>
                    {
                        if (x.PropertyName != "Severity")
                            return string.IsNullOrWhiteSpace(x.FieldName);
                        else
                            return true;
                    });
        }
        #endregion

        #region Protected Methods
        protected override void OnInitialize()
        {
            if (File.Exists(SettingDocumentType.FilePath))
            {
                try
                {
                    this.document = SettingDocumentType.Open();
                }
                catch
                {
                    this.messageService.ShowError(Resources.CannotOpenFile);
                    this.document = SettingDocumentType.New();
                }
            }
            else
            {
                this.document = SettingDocumentType.New();
            }

            AddWeakEventListener(this.document, DocumentPropertyChanged);
            this.settingViewModel.Settings = document;
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
            TfsTeamProjectCollection tpc = new TfsTeamProjectCollection(
                this.document.ConnectUri,
                new NetworkCredential(this.document.UserName, this.document.Password));
            tpc.EnsureAuthenticated();

            WorkItemStore workItemStore = (WorkItemStore)tpc.GetService(typeof(WorkItemStore));

            List<Bug> bugs = new List<Bug>();
            foreach (string userName in userNames)
            {
                string fields = string.Join(", ", this.document.PropertyMappingList.Select(x => "[" + x.FieldName + "]"));
                string filter = "["
                    + this.document.PropertyMappingList.First(x => x.PropertyName == "AssignedTo").FieldName
                    + "] = '" + userName + "'";

                if (isFilterCreatedBy)
                {
                    filter = "(" + filter + "' OR ["
                        + this.document.PropertyMappingList.First(x => x.PropertyName == "CreatedBy").FieldName
                        + "] = '" + userName + "')";
                }

                filter = this.document.BugFilterField + " = '" + this.document.BugFilterValue + "' And " + filter;
                string queryString = "Select " + fields + " From WorkItems Where " + filter;

                Query query = new Query(workItemStore, queryString);
                WorkItemCollection collection = query.RunQuery();

                foreach (WorkItem item in collection)
                {
                    bugs.Add(new Bug()
                    {
                        ID          = (int)item.Fields[this.document.PropertyMappingList.First(
                                            x => x.PropertyName == "ID").FieldName].Value,
                        Title       = item.Fields[this.document.PropertyMappingList.First(
                                            x => x.PropertyName == "Title").FieldName].Value.ToString(),
                        Description = item.Fields[this.document.PropertyMappingList.First(
                                            x => x.PropertyName == "Description").FieldName].Value.ToString(),
                        AssignedTo  = item.Fields[this.document.PropertyMappingList.First(
                                            x => x.PropertyName == "AssignedTo").FieldName].Value.ToString(),
                        State       = item.Fields[this.document.PropertyMappingList.First(
                                            x => x.PropertyName == "State").FieldName].Value.ToString(),
                        ChangedDate = (DateTime)item.Fields[this.document.PropertyMappingList.First(
                                            x => x.PropertyName == "ChangedDate").FieldName].Value,
                        CreatedBy   = item.Fields[this.document.PropertyMappingList.First(
                                            x => x.PropertyName == "CreatedBy").FieldName].Value.ToString(),
                        Priority    = item.Fields[this.document.PropertyMappingList.First(
                                            x => x.PropertyName == "Priority").FieldName].Value.ToString(),
                        Severity    = item.Fields[this.document.PropertyMappingList.First(
                                            x => x.PropertyName == "Severity").FieldName].Value.ToString()
                                            ?? string.Empty
                    });
                }
            }
            return new ReadOnlyCollection<Bug>(bugs);
        }
        #endregion

        #region Private Methods
        #region Commands Mehods
        private void SaveExcute()
        {
            SettingDocumentType.Save(this.document);
        }
        #endregion

        private void DocumentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateCommands();
        }

        private void UpdateCommands()
        {
            this.saveCommand.RaiseCanExecuteChanged();
        }
        #endregion
        #endregion
    }
}
