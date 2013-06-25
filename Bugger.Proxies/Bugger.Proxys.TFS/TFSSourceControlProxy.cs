using BigEgg.Framework.Applications.Commands;
using BigEgg.Framework.Applications.Services;
using Bugger.Domain.Models;
using Bugger.Proxy.TFS.Documents;
using Bugger.Proxy.TFS.Models;
using Bugger.Proxy.TFS.Properties;
using Bugger.Proxy.TFS.ViewModels;
using Bugger.Proxy.TFS.Views;
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

namespace Bugger.Proxy.TFS
{
    [Export(typeof(ISourceControlProxy)), Export]
    public class TFSSourceControlProxy : SourceControlProxy
    {
        #region Fields
        private readonly CompositionContainer container;
        private readonly IMessageService messageService;
        private readonly DelegateCommand saveCommand;
        private readonly DelegateCommand testConnectionCommand;
        private readonly DelegateCommand uriHelpCommand;

        private SettingDocument document;
        private TFSSettingViewModel settingViewModel;

        private string priorityFieldCache;
        private string stateFieldCache;
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
            this.saveCommand = new DelegateCommand(SaveExcute, CanSaveExcute);
            this.uriHelpCommand = new DelegateCommand(OpenUriHelpExcute);

            this.testConnectionCommand = new DelegateCommand(TestConnectionExcute, CanTestConnectionExcute);
        }

        #region Properties
        public override ISettingView SettingView { get { return this.settingViewModel.View as ISettingView; } }
        #endregion

        #region Methods
        #region Public Methods
        public override void SaveSettings()
        {
            if (this.saveCommand.CanExecute())
                this.saveCommand.Execute();
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

            this.settingViewModel = this.container.GetExportedValue<TFSSettingViewModel>();
            this.settingViewModel.SaveCommand = this.saveCommand;
            this.settingViewModel.UriHelpCommand = this.uriHelpCommand;
            this.settingViewModel.TestConnectionCommand = this.testConnectionCommand;
            this.settingViewModel.Settings = document;

            if (this.testConnectionCommand.CanExecute())
            {
                this.testConnectionCommand.Execute();
                UpdatePriorityValues();
                UpdateStatusValues();
                this.CanQuery = this.saveCommand.CanExecute();
            }
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
            List<string> redFilter = string.IsNullOrWhiteSpace(this.document.PriorityRed)
                                         ? new List<string>()
                                         : this.document.PriorityRed.Split(';').Select(x => x.Trim()).ToList();
            foreach (string userName in userNames)
            {
                string fields = string.Join(", ", this.document.PropertyMappingList
                    .Where(x => !string.IsNullOrWhiteSpace(x.FieldName))
                    .Select(x => "[" + x.FieldName + "]"));
                string filter = "["
                    + this.document.PropertyMappingList.First(x => x.PropertyName == "AssignedTo").FieldName
                    + "] = '" + userName + "'";

                if (isFilterCreatedBy)
                {
                    filter = "( " + filter + " OR ["
                        + this.document.PropertyMappingList.First(x => x.PropertyName == "CreatedBy").FieldName
                        + "] = '" + userName + "' )";
                }

                filter = "[" + this.document.BugFilterField + "] = '" + this.document.BugFilterValue + "' And " + filter;
                string queryString = "SELECT " + fields + " FROM WorkItems WHERE " + filter;

                Query query = new Query(workItemStore, queryString);
                WorkItemCollection collection = query.RunQuery();

                foreach (WorkItem item in collection)
                {
                    Bug bug = new Bug();
                    object value = null;

                    //  ID
                    value =
                        item.Fields[this.document.PropertyMappingList.First(x => x.PropertyName == "ID").FieldName]
                            .Value;
                    bug.ID = value == null ? 0 : (int) value;

                    //  Title
                    value =
                        item.Fields[this.document.PropertyMappingList.First(x => x.PropertyName == "Title").FieldName]
                            .Value;
                    bug.Title = value == null ? string.Empty : value.ToString();

                    //  Description
                    value =
                        item.Fields[
                            this.document.PropertyMappingList.First(x => x.PropertyName == "Description").FieldName]
                            .Value;
                    bug.Description = value == null ? string.Empty : value.ToString();

                    //  AssignedTo
                    value =
                        item.Fields[
                            this.document.PropertyMappingList.First(x => x.PropertyName == "AssignedTo").FieldName]
                            .Value;
                    bug.AssignedTo = value == null ? string.Empty : value.ToString();

                    //  State
                    value =
                        item.Fields[
                            this.document.PropertyMappingList.First(x => x.PropertyName == "State").FieldName]
                            .Value;
                    bug.State = value == null ? string.Empty : value.ToString();

                    //  ChangedDate
                    value =
                        item.Fields[
                            this.document.PropertyMappingList.First(x => x.PropertyName == "ChangedDate").FieldName]
                            .Value;
                    bug.ChangedDate = value == null ? DateTime.Today : (DateTime)value;

                    //  CreatedBy
                    value =
                        item.Fields[
                            this.document.PropertyMappingList.First(x => x.PropertyName == "CreatedBy").FieldName]
                            .Value;
                    bug.CreatedBy = value == null ? string.Empty : value.ToString();

                    //  Priority
                    value =
                        item.Fields[
                            this.document.PropertyMappingList.First(x => x.PropertyName == "Priority").FieldName]
                            .Value;
                    bug.Priority = value == null ? string.Empty : value.ToString();

                    //  Severity
                    if (string.IsNullOrWhiteSpace(
                            this.document.PropertyMappingList.First(x => x.PropertyName == "Severity").FieldName))
                    {
                        bug.Severity = string.Empty;
                    }
                    else
                    {
                        value =
                            item.Fields[
                                this.document.PropertyMappingList.First(x => x.PropertyName == "Severity").FieldName]
                                .Value;
                        bug.Severity = value == null ? string.Empty : value.ToString();
                    }

                    bug.Type = string.IsNullOrWhiteSpace(bug.Priority)
                                   ? BugType.Yellow
                                   : (redFilter.Contains(bug.Priority) ? BugType.Red : BugType.Yellow);

                    bugs.Add(bug);
                }
            }

            return new ReadOnlyCollection<Bug>(bugs.Distinct().ToList());
        }
        #endregion

        #region Private Methods
        #region Commands Mehods
        private void SaveExcute()
        {
            SettingDocumentType.Save(this.document);
        }

        private bool CanSaveExcute()
        {
            return this.testConnectionCommand.CanExecute()
                && !string.IsNullOrWhiteSpace(this.document.BugFilterField)
                && !string.IsNullOrWhiteSpace(this.document.BugFilterValue)
                && this.document.PropertyMappingList
                        .Where(x => x.PropertyName != "Severity")
                        .Any(x =>
                        {
                            return !string.IsNullOrWhiteSpace(x.FieldName);
                        });
        }

        private void OpenUriHelpExcute()
        {
            IUriHelpView view = this.container.GetExportedValue<IUriHelpView>();
            UriHelpViewModel viewModel = new UriHelpViewModel(view);
            viewModel.ServerName = this.document.ConnectUri.AbsoluteUri;

            var result = viewModel.ShowDialog(this);

            if (result.HasValue && result.Value)
            {
                this.document.ConnectUri = viewModel.UriPreview == Resources.InvalidUrl
                                               ? null
                                               : new Uri(viewModel.UriPreview);
            }
        }

        private bool CanTestConnectionExcute()
        {
            return this.document != null && this.document.ConnectUri != null && this.document.ConnectUri.IsAbsoluteUri
                && !string.IsNullOrWhiteSpace(this.document.UserName);
        }

        private void TestConnectionExcute()
        {
            this.settingViewModel.CanConnect = false;

            TfsTeamProjectCollection tpc = null;

            try
            {
                tpc = new TfsTeamProjectCollection(
                    this.document.ConnectUri,
                    new NetworkCredential(this.document.UserName, this.document.Password));
                tpc.EnsureAuthenticated();
            }
            catch
            {
                messageService.ShowMessage(Resources.CannotConnect);
                return;
            }

            try
            {
                WorkItemStore workItemStore = (WorkItemStore)tpc.GetService(typeof(WorkItemStore));
                FieldDefinitionCollection collection = workItemStore.FieldDefinitions;
                this.settingViewModel.TFSFields.Clear();
                foreach (FieldDefinition field in collection)
                {
                    TFSField tfsField = new TFSField(field.Name);
                    foreach (var value in field.AllowedValues)
                    {
                        tfsField.AllowedValues.Add(value.ToString());
                    }

                    this.settingViewModel.TFSFields.Add(tfsField);
                    if (tfsField.AllowedValues.Any())
                    {
                        this.settingViewModel.BugFilterFields.Add(tfsField);
                    }
                }

                this.settingViewModel.CanConnect = true;
            }
            catch
            {
                messageService.ShowMessage(Resources.CannotQueryFields);
            }
        }
        #endregion

        private void DocumentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "PropertyMappingList")
            {
                UpdatePriorityValues();
                UpdateStatusValues();
            }

            UpdateCommands();
            this.CanQuery = this.saveCommand.CanExecute();
        }

        private void PriorityValuePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.document.PriorityRed = string.Join("; ", 
                this.settingViewModel.PriorityValues.Where(x => x.IsChecked).Select(x=>x.Name));
        }

        private void UpdateCommands()
        {
            this.saveCommand.RaiseCanExecuteChanged();
            this.testConnectionCommand.RaiseCanExecuteChanged();
        }

        private void UpdatePriorityValues()
        {
            string fieldName = this.document.PropertyMappingList.First(x => x.PropertyName == "Priority").FieldName;

            if (!this.settingViewModel.CanConnect ||
                (string.IsNullOrWhiteSpace(fieldName) &&
                priorityFieldCache != null &&
                priorityFieldCache == fieldName))
                return;

            priorityFieldCache = fieldName;
            this.settingViewModel.PriorityValues.Clear();

            TFSField priorityField = this.settingViewModel.TFSFields.First(x => x.Name == priorityFieldCache);
            foreach (var value in priorityField.AllowedValues)
            {
                CheckString checkValue = new CheckString(value);
                checkValue.IsChecked = !string.IsNullOrWhiteSpace(this.document.PriorityRed) &&
                                       this.document.PriorityRed.Contains(value);

                AddWeakEventListener(checkValue, PriorityValuePropertyChanged);

                this.settingViewModel.PriorityValues.Add(checkValue);
            }

            this.document.PriorityRed = string.Join("; ",
                this.settingViewModel.PriorityValues.Where(x => x.IsChecked).Select(x => x.Name));
        }

        private void UpdateStatusValues()
        {
            string fieldName = this.document.PropertyMappingList.First(x => x.PropertyName == "State").FieldName;

            if (!this.settingViewModel.CanConnect ||
                (string.IsNullOrWhiteSpace(fieldName) &&
                stateFieldCache != null &&
                stateFieldCache == fieldName))
                return;

            stateFieldCache = fieldName;
            this.StateValues.Clear();

            TFSField stateField = this.settingViewModel.TFSFields.First(x => x.Name == stateFieldCache);
            foreach (var value in stateField.AllowedValues)
            {
                this.StateValues.Add(value);
            }
        }
        #endregion
        #endregion
    }
}
