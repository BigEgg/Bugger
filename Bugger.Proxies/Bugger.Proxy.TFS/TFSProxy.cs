using BigEgg.Framework.Applications.Commands;
using BigEgg.Framework.Applications.Services;
using Bugger.Domain.Models;
using Bugger.Domain.ViewModels;
using Bugger.Proxy.TFS.Documents;
using Bugger.Proxy.TFS.Models;
using Bugger.Proxy.TFS.Properties;
using Bugger.Proxy.TFS.ViewModels;
using Bugger.Proxy.TFS.Views;
using Bugger.Proxy.Views;
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
    public class TFSProxy : TracingSystemProxyBase
    {
        #region Fields
        private readonly CompositionContainer container;
        private readonly IMessageService messageService;
        private readonly TFSHelper tfsHelper;
        private SettingDocument document;
        private TFSSettingViewModel settingViewModel;

        private string stateColumn = string.Empty;
        private readonly ObservableCollection<string> stateValues;
        private List<string> ignoreField;
        private const string PriorityRedSeparator = ";";
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

        /// <summary>
        /// Gets the type of the bug view model.
        /// </summary>
        /// <value>
        /// The type of the bug view model.
        /// </value>
        public override Type BugViewModelType { get { return typeof(TFSBugViewModel); } }
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

            RemoveWeakEventListener(this.settingViewModel, SettingViewModelPropertyChanged);
            this.settingViewModel.ClearMappingData();

            this.settingViewModel.ConnectUri = this.document.ConnectUri;
            this.settingViewModel.UserName = this.document.UserName;
            this.settingViewModel.Password = this.document.Password;

            foreach (var mapping in this.document.PropertyMappingCollection)
            {
                this.settingViewModel.PropertyMappingCollection[mapping.Key] = mapping.Value;
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

            if (!submit)
            {
                UpdateStateValues(this.document.PropertyMappingCollection["State"], this.tfsFieldsCache);
                return;
            }

            this.document.ConnectUri = this.settingViewModel.ConnectUri;
            this.document.UserName = this.settingViewModel.UserName;
            this.document.Password = this.settingViewModel.Password;

            this.document.PropertyMappingCollection.Clear();
            foreach (var mapping in this.settingViewModel.PropertyMappingCollection)
            {
                this.document.PropertyMappingCollection.Add(mapping.Key, mapping.Value);
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
                || this.settingViewModel.ProgressType == ProgressTypes.OnConnectProgress
                || this.settingViewModel.ProgressType == ProgressTypes.OnGetFiledsProgress)
            {
                return SettingDialogValidateionResult.Busy;
            }

            if (this.settingViewModel.ConnectUri == null
                || !this.settingViewModel.ConnectUri.IsAbsoluteUri
                || string.IsNullOrWhiteSpace(this.settingViewModel.UserName)
                || this.settingViewModel.ProgressType == ProgressTypes.FailedOnConnect
                || this.settingViewModel.ProgressType == ProgressTypes.FailedOnGetFileds)
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
                || string.IsNullOrWhiteSpace(this.settingViewModel.BugFilterValue)
                || this.settingViewModel.PropertyMappingCollection
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
            UpdateStateValues(this.document.PropertyMappingCollection["State"], this.tfsFieldsCache);
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
        protected override ReadOnlyCollection<IBugViewModel> QueryCore(List<string> userNames, bool isFilterCreatedBy)
        {
            List<IBugViewModel> bugs = new List<IBugViewModel>();

            if (!this.CanQuery) { return new ReadOnlyCollection<IBugViewModel>(bugs); }

            if (this.document == null || this.document.ConnectUri == null ||
                string.IsNullOrWhiteSpace(this.document.ConnectUri.AbsoluteUri) ||
                string.IsNullOrWhiteSpace(this.document.UserName))
            {
                this.CanQuery = false;
                return new ReadOnlyCollection<IBugViewModel>(bugs);
            }

            TfsTeamProjectCollection tpc = null;
            if (tfsHelper.TryConnection(this.document.ConnectUri, this.document.UserName,
                                        this.document.Password, out tpc))
            {
                List<string> redFilter = string.IsNullOrWhiteSpace(this.document.PriorityRed)
                                           ? new List<string>()
                                           : this.document.PriorityRed
                                                          .Split(new string[] { PriorityRedSeparator }, StringSplitOptions.RemoveEmptyEntries)
                                                          .Select(x => x.Trim())
                                                          .ToList();

                foreach (string userName in userNames)
                {
                    var bugCollection = this.tfsHelper.GetBugs(tpc, userName, isFilterCreatedBy,
                                                               this.document.PropertyMappingCollection,
                                                               this.document.BugFilterField, this.document.BugFilterValue,
                                                               redFilter);
                    if (bugCollection == null) { continue; }

                    bugs.AddRange(bugCollection);
                }

                return new ReadOnlyCollection<IBugViewModel>(bugs.Distinct().ToList());
            }
            else
            {
                this.CanQuery = false;
                return new ReadOnlyCollection<IBugViewModel>(bugs);
            }
        }
        #endregion

        #region Private Methods
        private void UpdateSettingDialogPriorityValues()
        {
            foreach (var checkString in this.settingViewModel.PriorityValues)
            {
                RemoveWeakEventListener(checkString, PriorityValuePropertyChanged);
            }
            this.settingViewModel.PriorityValues.Clear();

            string fieldName = this.settingViewModel.PropertyMappingCollection["Priority"];
            if (string.IsNullOrWhiteSpace(fieldName))
            {
                this.settingViewModel.PriorityRed = string.Empty;
                return;
            }

            TFSField priorityField = this.settingViewModel.TFSFields.FirstOrDefault(x => x.Name == fieldName);
            if (priorityField == null) { return; }

            foreach (var value in priorityField.AllowedValues)
            {
                CheckString checkValue = new CheckString(value);
                checkValue.IsChecked = !string.IsNullOrWhiteSpace(this.document.PriorityRed)
                                       && this.document.PriorityRed.Contains(value);

                AddWeakEventListener(checkValue, PriorityValuePropertyChanged);

                this.settingViewModel.PriorityValues.Add(checkValue);
            }

            this.settingViewModel.PriorityRed = string.Join(PriorityRedSeparator,
                                                            this.settingViewModel.PriorityValues
                                                                                 .Where(x => x.IsChecked)
                                                                                 .Select(x => x.Name));
        }

        private void UpdateStateValues(string stateFieldName, ICollection<TFSField> tfsFields)
        {
            var field = tfsFields.FirstOrDefault(x => x.Name == stateFieldName);
            if (field != null && field.Name != stateColumn)
            {
                stateColumn = field.Name;
                StateValues.Clear();
                foreach (var value in field.AllowedValues)
                {
                    StateValues.Add(value);
                }
            }
        }

        private void AutoFillMapSettings(List<TFSField> tfsFields)
        {
            if (tfsFields == null) { throw new ArgumentException("tfsFields"); }

            if (tfsFields.Any(x => x.Name == "ID"))
            {
                this.settingViewModel.PropertyMappingCollection["ID"] = "ID";
            }
            if (tfsFields.Any(x => x.Name == "Title"))
            {
                this.settingViewModel.PropertyMappingCollection["Title"] = "Title";
            }
            if (tfsFields.Any(x => x.Name == "Description"))
            {
                this.settingViewModel.PropertyMappingCollection["Description"] = "Description";
            }
            if (tfsFields.Any(x => x.Name == "Assigned To"))
            {
                this.settingViewModel.PropertyMappingCollection["AssignedTo"] = "Assigned To";
            }
            if (tfsFields.Any(x => x.Name == "State"))
            {
                this.settingViewModel.PropertyMappingCollection["State"] = "State";
            }
            if (tfsFields.Any(x => x.Name == "Changed Date"))
            {
                this.settingViewModel.PropertyMappingCollection["ChangedDate"] = "Changed Date";
            }
            if (tfsFields.Any(x => x.Name == "Created By"))
            {
                this.settingViewModel.PropertyMappingCollection["CreatedBy"] = "Created By";
            }
            if (tfsFields.Any(x => x.Name == "Code Studio Rank"))
            {
                this.settingViewModel.PropertyMappingCollection["Priority"] = "Code Studio Rank";
            }
            if (tfsFields.Any(x => x.Name == "Severity"))
            {
                this.settingViewModel.PropertyMappingCollection["Severity"] = "Severity";
            }

            var workItemType = tfsFields.FirstOrDefault(x => x.Name == "Work Item Type");
            if (workItemType != null)
            {
                this.settingViewModel.BugFilterField = "Work Item Type";
                var value = workItemType.AllowedValues.FirstOrDefault(x => string.Compare(x, "Bugs", true) == 0);
                this.settingViewModel.BugFilterValue = value ?? string.Empty;
            }

            UpdateSettingDialogPriorityValues();
        }

        private void SettingViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "PropertyMappingCollection")
            {
                UpdateSettingDialogPriorityValues();
                UpdateStateValues(this.settingViewModel.PropertyMappingCollection["State"], this.settingViewModel.TFSFields);
            }
            else if (e.PropertyName == "ConnectUri" || e.PropertyName == "UserName" || e.PropertyName == "Password")
            {
                this.settingViewModel.ClearMappingData();
            }

            UpdateCommands();
        }

        private void PriorityValuePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.settingViewModel.PriorityRed = string.Join(PriorityRedSeparator,
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
            TestConnectionCommandExcuteCore();
        }

        internal Task TestConnectionCommandExcuteCore()
        {
            this.settingViewModel.ClearMappingData();
            this.settingViewModel.ProgressType = ProgressTypes.OnConnectProgress;
            this.settingViewModel.ProgressValue = 0;

            var testConnectionTask = Task.Factory.StartNew(() =>
            {
                //  Connect
                TfsTeamProjectCollection tpc = null;
                tfsHelper.TryConnection(this.settingViewModel.ConnectUri, this.settingViewModel.UserName,
                                             this.settingViewModel.Password, out tpc);
                return tpc;
            })
            .ContinueWith(task =>
            {
                if (task.Result == null)
                {
                    this.settingViewModel.ProgressType = ProgressTypes.FailedOnConnect;
                    this.settingViewModel.ProgressValue = 100;
                    throw new OperationCanceledException();
                }
                else
                {
                    this.settingViewModel.ProgressType = ProgressTypes.OnGetFiledsProgress;
                    this.settingViewModel.ProgressValue = 50;
                    return task.Result;
                }
            }, CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.FromCurrentSynchronizationContext())
            .ContinueWith(task =>
            {
                //  Query TFS Fields
                return tfsHelper.GetFields(task.Result);
            }, TaskContinuationOptions.OnlyOnRanToCompletion)
            .ContinueWith(task =>
            {
                if (task.Result == null)
                {
                    this.settingViewModel.ProgressType = ProgressTypes.FailedOnGetFileds;
                    this.settingViewModel.ProgressValue = 100;
                    throw new OperationCanceledException();
                }
                else
                {
                    this.settingViewModel.ProgressValue = 80;
                    return task.Result;
                }
            }, CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.FromCurrentSynchronizationContext())
            .ContinueWith(task =>
            {
                this.settingViewModel.TFSFields.Clear();
                this.settingViewModel.BugFilterFields.Clear();
                foreach (var field in task.Result)
                {
                    this.settingViewModel.TFSFields.Add(field);
                    if (field.AllowedValues.Any())
                    {
                        this.settingViewModel.BugFilterFields.Add(field);
                    }
                }
                return task.Result;
            }, CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.FromCurrentSynchronizationContext())
            .ContinueWith(task =>
            {
                this.settingViewModel.ProgressValue = 90;
                this.settingViewModel.ProgressType = ProgressTypes.OnAutoFillMapSettings;
                return task.Result;
            }, CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.FromCurrentSynchronizationContext())
            .ContinueWith(task =>
            {
                try
                {
                    AutoFillMapSettings(task.Result);
                    return true;
                }
                catch
                {
                    return false;
                }
            }, CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.FromCurrentSynchronizationContext())
            .ContinueWith(task =>
            {
                if (task.Result)
                {
                    this.settingViewModel.ProgressValue = 100;
                    this.settingViewModel.ProgressType = ProgressTypes.Success;
                }
                else
                {
                    this.settingViewModel.ProgressType = ProgressTypes.SuccessWithError;
                    this.settingViewModel.ProgressValue = 100;
                }
            }, CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.FromCurrentSynchronizationContext());

            return testConnectionTask;
        }

        private void UpdateCommands()
        {
            this.testConnectionCommand.RaiseCanExecuteChanged();
        }
        #endregion
        #endregion
    }
}
