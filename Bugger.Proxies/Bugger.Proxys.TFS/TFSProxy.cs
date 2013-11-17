﻿using BigEgg.Framework.Applications.Commands;
using BigEgg.Framework.Applications.Services;
using Bugger.Domain.Models;
using Bugger.Proxy.TFS.Documents;
using Bugger.Proxy.TFS.Models;
using Bugger.Proxy.TFS.Properties;
using Bugger.Proxy.TFS.ViewModels;
using Bugger.Proxy.TFS.Views;
using Microsoft.TeamFoundation.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bugger.Proxy.TFS
{
    [Export(typeof(ITracingSystemProxy)), Export]
    public class TFSProxy : TracingSystemProxy
    {
        #region Fields
        private readonly CompositionContainer container;
        private readonly IMessageService messageService;
        private readonly TFSHelper tfsHelper;
        private SettingDocument document;
        private TFSSettingViewModel settingViewModel;

        private readonly ObservableCollection<string> stateValues;
        private List<string> ignoreField;
        private readonly List<TFSField> tfsFieldsCache;

        private readonly DelegateCommand testConnectionCommand;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="TFSProxy" /> class.
        /// </summary>
        [ImportingConstructor]
        public TFSProxy(CompositionContainer container, IMessageService messageService, TFSHelper tfsHelper)
            : base(Resources.ProxyName)
        {
            if (container == null) { throw new ArgumentNullException("container"); }
            if (messageService == null) { throw new ArgumentNullException("messageService"); }
            if (tfsHelper == null) { throw new ArgumentNullException("tfsHelper"); }

            this.container = container;
            this.messageService = messageService;
            this.tfsHelper = tfsHelper;

            this.testConnectionCommand = new DelegateCommand(TestConnectionCommandExcute, CanTestConnectionCommandExcute);

            this.tfsFieldsCache = new List<TFSField>();
            this.stateValues = new ObservableCollection<string>();

            this.CanQuery = false;
        }

        #region Properties
        /// <summary>
        /// Gets the status values.
        /// </summary>
        /// <value>
        /// The status values.
        /// </value>
        public override ObservableCollection<string> StateValues { get { return this.stateValues; } }
        #endregion

        #region Methods
        #region Public Methods
        /// <summary>
        /// Initializes the setting dialog.
        /// </summary>
        /// <returns></returns>
        public override ISettingView InitializeSettingDialog()
        {
            if (this.settingViewModel == null)
            {
                ITFSSettingView view = this.container.GetExportedValue<ITFSSettingView>();
                IUriHelpView uriHelpView = this.container.GetExportedValue<IUriHelpView>();
                this.settingViewModel = new TFSSettingViewModel(view, uriHelpView);
                this.settingViewModel.TestConnectionCommand = this.testConnectionCommand;
            }

            this.settingViewModel.ClearData();

            this.settingViewModel.ConnectUri = this.document.ConnectUri;
            this.settingViewModel.UserName = this.document.UserName;
            this.settingViewModel.Password = this.document.Password;

            foreach (var mapping in this.document.PropertyMappingCollection)
            {
                this.settingViewModel.PropertyMappingCollection.Add(mapping);
            }

            foreach (var field in this.tfsFieldsCache)
            {
                this.settingViewModel.TFSFields.Add(field);
                if (field.AllowedValues.Any())
                {
                    this.settingViewModel.BugFilterFields.Add(field);
                }
            }

            this.settingViewModel.BugFilterField = this.document.BugFilterField;
            this.settingViewModel.BugFilterValue = this.document.BugFilterValue;
            this.settingViewModel.PriorityRed = this.document.PriorityRed;

            UpdateSettingDialogPriorityValues();

            if (this.CanQuery)
            {
                if (string.IsNullOrWhiteSpace(this.settingViewModel.BugFilterField)
                    && string.IsNullOrWhiteSpace(this.settingViewModel.BugFilterValue)
                    && this.settingViewModel.PropertyMappingCollection
                            .Where(x => !ignoreField.Contains(x.Key))
                            .Any(x =>
                            {
                                return string.IsNullOrWhiteSpace(x.Value);
                            }))
                {
                    this.settingViewModel.ProgressType = ProgressTypes.SuccessWithError;
                }
                else
                {
                    this.settingViewModel.ProgressType = ProgressTypes.Success;
                }
                this.settingViewModel.ProgressValue = 100;
            }

            AddWeakEventListener(this.settingViewModel, SettingViewModelPropertyChanged);

            return this.settingViewModel.View as ISettingView;
        }

        /// <summary>
        /// Do something afters close setting dialog.
        /// </summary>
        /// <param name="submit">If the dialog is return as submit..</param>
        public override void AfterCloseSettingDialog(bool submit)
        {
            RemoveWeakEventListener(this.settingViewModel, SettingViewModelPropertyChanged);

            if (!submit) { return; }

            this.document.ConnectUri = this.settingViewModel.ConnectUri;
            this.document.UserName = this.settingViewModel.UserName;
            this.document.Password = this.settingViewModel.Password;

            this.document.PropertyMappingCollection.Clear();
            foreach (var mapping in this.settingViewModel.PropertyMappingCollection)
            {
                this.document.PropertyMappingCollection.Add(mapping);
            }

            this.tfsFieldsCache.Clear();
            this.tfsFieldsCache.AddRange(this.settingViewModel.TFSFields);

            this.document.BugFilterField = this.settingViewModel.BugFilterField;
            this.document.BugFilterValue = this.settingViewModel.BugFilterValue;
            this.document.PriorityRed = this.settingViewModel.PriorityRed;

            this.CanQuery = false;

            if ((this.settingViewModel.ProgressType == ProgressTypes.Success
                || this.settingViewModel.ProgressType == ProgressTypes.SuccessWithError)
                &&
                (!string.IsNullOrWhiteSpace(this.settingViewModel.BugFilterField)
                 && !string.IsNullOrWhiteSpace(this.settingViewModel.BugFilterValue)
                 && this.settingViewModel.PropertyMappingCollection
                         .Where(x => !ignoreField.Contains(x.Key))
                         .Any(x =>
                         {
                             return !string.IsNullOrWhiteSpace(x.Value);
                         })))
            {
                this.CanQuery = true;
            }

            SettingDocumentType.Save(this.document);
        }

        /// <summary>
        /// Validate the setting values before close setting dialog.
        /// </summary>
        /// <returns>
        /// The validation result.
        /// </returns>
        public override SettingDialogValidateionResult ValidateBeforeCloseSettingDialog()
        {
            if (this.settingViewModel.ProgressType == ProgressTypes.OnAutoFillMapSettings
                && this.settingViewModel.ProgressType == ProgressTypes.OnConnectProgress
                && this.settingViewModel.ProgressType == ProgressTypes.OnGetFiledsProgress)
            {
                return SettingDialogValidateionResult.Busy;
            }

            if (this.settingViewModel.ConnectUri != null
                && this.settingViewModel.ConnectUri.IsAbsoluteUri
                && !string.IsNullOrWhiteSpace(this.settingViewModel.UserName)
                && this.settingViewModel.ProgressType == ProgressTypes.FailedOnConnect
                && this.settingViewModel.ProgressType == ProgressTypes.FailedOnGetFileds)
            {
                return SettingDialogValidateionResult.ConnectFailed;
            }

            if (this.settingViewModel.ProgressType == ProgressTypes.NotWorking)
            {
                TfsTeamProjectCollection tpc = null;
                if (!tfsHelper.TryConnection(this.settingViewModel.ConnectUri, this.settingViewModel.UserName,
                                             this.settingViewModel.Password, out tpc))
                {
                    return SettingDialogValidateionResult.ConnectFailed;
                }
            }

            if (string.IsNullOrWhiteSpace(this.settingViewModel.BugFilterField)
                && string.IsNullOrWhiteSpace(this.settingViewModel.BugFilterValue)
                && this.settingViewModel.PropertyMappingCollection
                                        .Where(x => !ignoreField.Contains(x.Key))
                                        .Any(x =>
                                         {
                                             return string.IsNullOrWhiteSpace(x.Value);
                                         }))
            {
                return SettingDialogValidateionResult.UnValid;
            }

            return SettingDialogValidateionResult.Valid;
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// The method which will execute when the Controller.Initialize() execute.
        /// </summary>
        protected override void OnInitialize()
        {
            ignoreField = new List<string>();
            ignoreField.Add("Severity");

            //  Open the setting file
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

            //  Validate Connect Information
            if (this.document == null || this.document.ConnectUri == null ||
                string.IsNullOrWhiteSpace(this.document.ConnectUri.AbsoluteUri) ||
                string.IsNullOrWhiteSpace(this.document.UserName))
            {
                return;
            }

            //  Connect to TFS
            TfsTeamProjectCollection tpc = null;
            if (!tfsHelper.TryConnection(this.document.ConnectUri, this.document.UserName,
                                        this.document.Password, out tpc))
            {
                return;
            }

            //  Get Fields
            var fields = tfsHelper.GetFields(tpc);
            if (fields == null || !fields.Any()) { return; }

            this.tfsFieldsCache.AddRange(tfsHelper.GetFields(tpc));
            var stateFieldName = this.document.PropertyMappingCollection["State"];
            if (!string.IsNullOrWhiteSpace(stateFieldName))
            {
                var values = this.tfsFieldsCache.First(x => x.Name == stateFieldName).AllowedValues;
                foreach (var value in values)
                {
                    this.stateValues.Add(value);
                }
            }
            this.CanQuery = true;
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
            List<Bug> bugs = new List<Bug>();

            if (!this.CanQuery) { return new ReadOnlyCollection<Bug>(bugs); }

            if (this.document == null || this.document.ConnectUri == null ||
                string.IsNullOrWhiteSpace(this.document.ConnectUri.AbsoluteUri) ||
                string.IsNullOrWhiteSpace(this.document.UserName))
            {
                this.CanQuery = false;
                return new ReadOnlyCollection<Bug>(bugs);
            }

            TfsTeamProjectCollection tpc = null;
            if (tfsHelper.TryConnection(this.document.ConnectUri, this.document.UserName,
                                        this.document.Password, out tpc))
            {
                List<string> redFilter = string.IsNullOrWhiteSpace(this.document.PriorityRed)
                                           ? new List<string>()
                                           : this.document.PriorityRed.Split(';').Select(x => x.Trim()).ToList();

                foreach (string userName in userNames)
                {
                    var bugCollection = this.tfsHelper.GetBugs(tpc, userName, isFilterCreatedBy,
                                                               this.document.PropertyMappingCollection,
                                                               this.document.BugFilterField, this.document.BugFilterValue,
                                                               redFilter);
                    if (bugCollection == null) { continue; }

                    bugs.AddRange(bugCollection);
                }

                return new ReadOnlyCollection<Bug>(bugs.Distinct().ToList());
            }
            else
            {
                this.CanQuery = false;
                return new ReadOnlyCollection<Bug>(bugs);
            }
        }
        #endregion

        #region Private Methods
        private void UpdateSettingDialogPriorityValues()
        {
            string fieldName = this.settingViewModel.PropertyMappingCollection["Priority"];
            if (string.IsNullOrWhiteSpace(fieldName)) { return; }

            foreach (var checkString in this.settingViewModel.PriorityValues)
            {
                RemoveWeakEventListener(checkString, PriorityValuePropertyChanged);
            }
            this.settingViewModel.PriorityValues.Clear();

            TFSField priorityField = this.settingViewModel.TFSFields.First(x => x.Name == fieldName);
            foreach (var value in priorityField.AllowedValues)
            {
                CheckString checkValue = new CheckString(value);
                checkValue.IsChecked = !string.IsNullOrWhiteSpace(this.settingViewModel.PriorityRed)
                                       && this.settingViewModel.PriorityRed.Contains(value);

                AddWeakEventListener(checkValue, PriorityValuePropertyChanged);

                this.settingViewModel.PriorityValues.Add(checkValue);
            }

            this.settingViewModel.PriorityRed = string.Join("; ",
                                                            this.settingViewModel.PriorityValues
                                                                                 .Where(x => x.IsChecked)
                                                                                 .Select(x => x.Name));
        }

        //private void UpdateStatusValues()
        //{
        //    string fieldName = this.document.PropertyMappingCollection["State"];

        //    if (!this.settingViewModel.CanConnect ||
        //        (string.IsNullOrWhiteSpace(fieldName) &&
        //        stateFieldCache != null &&
        //        stateFieldCache == fieldName))
        //        return;

        //    stateFieldCache = fieldName;
        //    this.StateValues.Clear();

        //    TFSField stateField = this.settingViewModel.TFSFields.First(x => x.Name == stateFieldCache);
        //    foreach (var value in stateField.AllowedValues)
        //    {
        //        this.StateValues.Add(value);
        //    }
        //}

        private void AutoFillMapSettings(List<TFSField> tfsFields)
        {
            if (tfsFields == null) { throw new ArgumentException("tfsFields"); }

            if (tfsFields.Any(x => x.Name == "ID"))
            {
                this.document.PropertyMappingCollection["ID"] = "ID";
            }
            if (tfsFields.Any(x => x.Name == "Title"))
            {
                this.document.PropertyMappingCollection["Title"] = "Title";
            }
            if (tfsFields.Any(x => x.Name == "Description"))
            {
                this.document.PropertyMappingCollection["Description"] = "Description";
            }
            if (tfsFields.Any(x => x.Name == "Assigned To"))
            {
                this.document.PropertyMappingCollection["AssignedTo"] = "Assigned To";
            }
            if (tfsFields.Any(x => x.Name == "State"))
            {
                this.document.PropertyMappingCollection["State"] = "State";
            }
            if (tfsFields.Any(x => x.Name == "Changed Date"))
            {
                this.document.PropertyMappingCollection["ChangedDate"] = "Changed Date";
            }
            if (tfsFields.Any(x => x.Name == "Created By"))
            {
                this.document.PropertyMappingCollection["CreatedBy"] = "Created By";
            }
            if (tfsFields.Any(x => x.Name == "Code Studio Rank"))
            {
                this.document.PropertyMappingCollection["Priority"] = "Code Studio Rank";
            }
            if (tfsFields.Any(x => x.Name == "Severity"))
            {
                this.document.PropertyMappingCollection["Severity"] = "Severity";
            }

            var workItemType = tfsFields.FirstOrDefault(x => x.Name == "Work Item Type");
            if (workItemType != null)
            {
                this.document.BugFilterField = "Work Item Type";
                var value = workItemType.AllowedValues.FirstOrDefault(x => string.Compare(x, "Bugs", true) == 0);

                if (string.IsNullOrEmpty(value))
                {
                    this.document.BugFilterValue = "value";
                }
            }
        }

        private void SettingViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "PropertyMappingList")
            {
                UpdateSettingDialogPriorityValues();
            }
            else if (e.PropertyName == "ConnectUri" || e.PropertyName == "UserName" || e.PropertyName == "Password")
            {
                this.settingViewModel.ClearData();
            }

            UpdateCommands();
        }

        private void PriorityValuePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.settingViewModel.PriorityRed = string.Join("; ",
                                                            this.settingViewModel.PriorityValues
                                                                                 .Where(x => x.IsChecked)
                                                                                 .Select(x => x.Name));
        }
        #endregion

        #region Commands Methods
        private bool CanTestConnectionCommandExcute()
        {
            return this.settingViewModel.ProgressType != ProgressTypes.OnAutoFillMapSettings
                && this.settingViewModel.ProgressType != ProgressTypes.OnConnectProgress
                && this.settingViewModel.ProgressType != ProgressTypes.OnGetFiledsProgress
                && this.settingViewModel.ConnectUri != null
                && this.settingViewModel.ConnectUri.IsAbsoluteUri
                && !string.IsNullOrWhiteSpace(this.settingViewModel.UserName);
        }

        private void TestConnectionCommandExcute()
        {
            this.settingViewModel.ClearData();

            this.settingViewModel.ProgressType = ProgressTypes.OnConnectProgress;
            this.settingViewModel.ProgressValue = 0;
            Task.Factory.StartNew(() =>
            {
                //  Connect
                TfsTeamProjectCollection tpc = null;
                if (!tfsHelper.TryConnection(this.document.ConnectUri, this.document.UserName,
                                             this.document.Password, out tpc))
                {
                    this.settingViewModel.ProgressType = ProgressTypes.FailedOnConnect;
                    this.settingViewModel.ProgressValue = 100;
                    return;
                }


                //  Query TFS Fields
                this.settingViewModel.ProgressType = ProgressTypes.OnGetFiledsProgress;
                this.settingViewModel.ProgressValue = 50;

                var fields = tfsHelper.GetFields(tpc);
                if (fields == null || !fields.Any())
                {
                    this.settingViewModel.ProgressType = ProgressTypes.FailedOnGetFileds;
                    this.settingViewModel.ProgressValue = 100;
                }

                this.settingViewModel.ProgressValue = 80;
                foreach (var field in fields)
                {
                    this.settingViewModel.TFSFields.Add(field);
                    if (field.AllowedValues.Any())
                    {
                        this.settingViewModel.BugFilterFields.Add(field);
                    }
                }

                //  Auto Fill Map Settings
                this.settingViewModel.ProgressValue = 90;
                this.settingViewModel.ProgressType = ProgressTypes.OnAutoFillMapSettings;
                try
                {
                    AutoFillMapSettings(fields);
                }
                catch
                {
                    this.settingViewModel.ProgressType = ProgressTypes.SuccessWithError;
                    this.settingViewModel.ProgressValue = 100;
                    return;
                }

                this.settingViewModel.ProgressValue = 100;
                this.settingViewModel.ProgressType = ProgressTypes.Success;
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void UpdateCommands()
        {
            this.testConnectionCommand.RaiseCanExecuteChanged();
        }
        #endregion
        #endregion
    }
}
