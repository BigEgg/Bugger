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

        private SettingDocumentType documentType;
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
            this.documentType = new SettingDocumentType();

            this.settingViewModel = this.container.GetExportedValue<SettingViewModel>();
            this.settingViewModel.SaveCommand = this.saveCommand;

            AddWeakEventListener(this.document, DocumentPropertyChanged);
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
                        if (x.FieldName != "Severity")
                            return string.IsNullOrWhiteSpace(x.PropertyName);
                        else
                            return true;
                    });
        }
        #endregion

        #region Protected Methods
        protected override void OnInitialize()
        {
            if (File.Exists(this.documentType.FilePath))
            {
                try
                {
                    this.document = this.documentType.Open();
                }
                catch
                {
                    this.messageService.ShowError(Resources.CannotOpenFile);
                    this.document = this.documentType.New();
                }
            }
            else
            {
                this.document = this.documentType.New();
            }

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
                string fields = string.Join(", ", this.document.PropertyMappingList.Select(x => x.PropertyName));
                string filter = isFilterCreatedBy ?
                    "([System.AssignedTo] = '" + userName + "' OR [System.CreatedBy] = '" + userName + "')" :
                    "[System.AssignedTo] = '" + userName + "'";
                filter = this.document.BugFilterField + " = '" + this.document.BugFilterValue + "' And " + filter;
                string queryString = "Select " + fields + " From WorkItems Where " + filter;

                Query query = new Query(workItemStore, queryString);
                WorkItemCollection collection = query.RunQuery();

                foreach (WorkItem item in collection)
                {
                    bugs.Add(new Bug()
                    {
                        ID          = (int)item.Fields[this.document.PropertyMappingList.First(
                                            x => x.FieldName == "ID").PropertyName].Value,
                        Title       = item.Fields[this.document.PropertyMappingList.First(
                                            x => x.FieldName == "Title").PropertyName].Value.ToString(),
                        Description = item.Fields[this.document.PropertyMappingList.First(
                                            x => x.FieldName == "Description").PropertyName].Value.ToString(),
                        AssignedTo  = item.Fields[this.document.PropertyMappingList.First(
                                            x => x.FieldName == "AssignedTo").PropertyName].Value.ToString(),
                        State       = item.Fields[this.document.PropertyMappingList.First(
                                            x => x.FieldName == "State").PropertyName].Value.ToString(),
                        ChangedDate = (DateTime)item.Fields[this.document.PropertyMappingList.First(
                                            x => x.FieldName == "ChangedDate").PropertyName].Value,
                        CreatedBy   = item.Fields[this.document.PropertyMappingList.First(
                                            x => x.FieldName == "CreatedBy").PropertyName].Value.ToString(),
                        Priority    = item.Fields[this.document.PropertyMappingList.First(
                                            x => x.FieldName == "Priority").PropertyName].Value.ToString(),
                        Severity    = item.Fields[this.document.PropertyMappingList.First(
                                            x => x.FieldName == "Severity").PropertyName].Value.ToString() 
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
            this.documentType.Save(this.document);
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
