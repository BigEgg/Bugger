using BigEgg.Framework.Applications.Commands;
using BigEgg.Framework.Applications.Services;
using Bugger.Domain.Models;
using Bugger.Proxy.TFS.Documents;
using Bugger.Proxy.TFS.Models;
using Bugger.Proxy.TFS.Properties;
using Bugger.Proxy.TFS.ViewModels;
using Bugger.Proxy.TFS.Views;
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
        private readonly DelegateCommand saveCommand;
        private readonly DelegateCommand testConnectionCommand;
        private readonly DelegateCommand uriHelpCommand;

        private List<string> IgnoreField;

        private SettingDocument document;
        private TFSSettingViewModel settingViewModel;

        private string priorityFieldCache;
        private string stateFieldCache;
        private bool isTestConnectOnProgress = false;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="TFSProxy" /> class.
        /// </summary>
        [ImportingConstructor]
        public TFSProxy(CompositionContainer container, IMessageService messageService)
            : base(Resources.ProxyName)
        {
            this.container = container;
            this.messageService = messageService;
            this.tfsHelper = new TFSHelper();
            this.saveCommand = new DelegateCommand(SaveCommandExcute, CanSaveCommandExcute);
            this.uriHelpCommand = new DelegateCommand(OpenUriHelpCommandExcute);

            this.testConnectionCommand = new DelegateCommand(TestConnectionCommandExcute, CanTestConnectionCommandExcute);
        }

        #region Properties
        public override ISettingView SettingView { get { return this.settingViewModel.View as ISettingView; } }
        #endregion

        #region Methods
        #region Public Methods
        public override void OnSumbitSettings()
        {
            if (this.saveCommand.CanExecute())
                this.saveCommand.Execute();
        }

        public override void OnCancelSettings()
        {
            RestoreTFSData();
        }
        #endregion

        #region Protected Methods
        protected override void OnInitialize()
        {
            IgnoreField = new List<string>();
            IgnoreField.Add("Severity");

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
                GetDataFromTFS();
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
            if (!this.tfsHelper.IsConnected())
                this.tfsHelper.TestConnection(this.document.ConnectUri, this.document.UserName, this.document.Password);

            List<Bug> bugs = new List<Bug>();
            List<string> redFilter = string.IsNullOrWhiteSpace(this.document.PriorityRed)
                                         ? new List<string>()
                                         : this.document.PriorityRed.Split(';').Select(x => x.Trim()).ToList();

            foreach (string userName in userNames)
            {
                var bugCollection = this.tfsHelper.GetBugs(userName, isFilterCreatedBy,
                                                           this.document.PropertyMappingCollection,
                                                           this.document.BugFilterField, this.document.BugFilterValue,
                                                           redFilter);
                if (bugCollection == null)
                    continue;

                bugs.AddRange(bugCollection);
            }

            return new ReadOnlyCollection<Bug>(bugs.Distinct().ToList());
        }
        #endregion

        #region Private Methods
        #region Commands Mehods
        private void SaveCommandExcute()
        {
            SettingDocumentType.Save(this.document);
        }

        private bool CanSaveCommandExcute()
        {
            return this.testConnectionCommand.CanExecute()
                && !string.IsNullOrWhiteSpace(this.document.BugFilterField)
                && !string.IsNullOrWhiteSpace(this.document.BugFilterValue)
                && this.document.PropertyMappingCollection
                        .Where(x => !IgnoreField.Contains(x.Key))
                        .Any(x =>
                        {
                            return !string.IsNullOrWhiteSpace(x.Value);
                        });
        }

        private void OpenUriHelpCommandExcute()
        {
            IUriHelpView view = this.container.GetExportedValue<IUriHelpView>();
            UriHelpViewModel viewModel = new UriHelpViewModel(view);
            if (this.document.ConnectUri != null)
                viewModel.ServerName = this.document.ConnectUri.AbsoluteUri;

            var result = viewModel.ShowDialog(this);

            if (result.HasValue && result.Value)
            {
                this.document.ConnectUri = viewModel.UriPreview == Resources.InvalidUrl
                                               ? null
                                               : new Uri(viewModel.UriPreview);
            }
        }

        private bool CanTestConnectionCommandExcute()
        {
            return !this.isTestConnectOnProgress && this.document != null && this.document.ConnectUri != null && this.document.ConnectUri.IsAbsoluteUri
                && !string.IsNullOrWhiteSpace(this.document.UserName);
        }

        private void TestConnectionCommandExcute()
        {
            this.isTestConnectOnProgress = true;
            ClearTFSData();
            UpdateCommands();

            //  Connect
            this.settingViewModel.ProgressType = ProgressTypes.OnConnectProgress;
            this.settingViewModel.ProgressValue = 0;
            Task.Factory.StartNew(() =>
            {
                return this.tfsHelper.TestConnection(this.document.ConnectUri, this.document.UserName, this.document.Password);
            })
            .ContinueWith(task =>
            {
                if (!task.Result)
                {
                    this.settingViewModel.ProgressType = ProgressTypes.FailedOnConnect;
                    this.settingViewModel.ProgressValue = 100;
                    throw new OperationCanceledException();
                }
            }, CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.FromCurrentSynchronizationContext())
            .ContinueWith(task =>
            {
                //  Query TFS Fields
                this.settingViewModel.ProgressType = ProgressTypes.OnGetFiledsProgress;
                this.settingViewModel.ProgressValue = 50;
            }, CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.FromCurrentSynchronizationContext())
            .ContinueWith(task =>
            {
                return this.tfsHelper.GetFields(); ;
            })
            .ContinueWith(task =>
            {
                if (task.Result == null)
                {
                    this.settingViewModel.ProgressType = ProgressTypes.FailedOnGetFileds;
                    this.settingViewModel.ProgressValue = 100;
                    throw new OperationCanceledException();
                }
                this.settingViewModel.ProgressValue = 80;
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

                this.settingViewModel.CanConnect = true;
                this.settingViewModel.ProgressValue = 90;
                this.settingViewModel.ProgressType = ProgressTypes.OnAutoFillMapSettings;
                try
                {
                    AutoFillMapSettings(task.Result);
                }
                catch
                {
                    this.settingViewModel.ProgressType = ProgressTypes.SuccessWithError;
                    this.settingViewModel.ProgressValue = 100;
                    return;
                }

                this.settingViewModel.ProgressValue = 100;
                this.settingViewModel.ProgressType = ProgressTypes.Success;
            }, CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.FromCurrentSynchronizationContext());

            this.isTestConnectOnProgress = false;
            UpdateCommands();
        }
        #endregion


        private void GetDataFromTFS()
        {
            //  Connect
            bool canConnect = this.tfsHelper.TestConnection(this.document.ConnectUri, this.document.UserName, this.document.Password);
            if (!canConnect)
            {
                return;
            }

            //  Query TFS Fields
            var tfsFields = this.tfsHelper.GetFields();
            if (tfsFields == null)
            {
                return;
            }

            this.settingViewModel.TFSFields.Clear();
            this.settingViewModel.BugFilterFields.Clear();
            foreach (var field in tfsFields)
            {
                this.settingViewModel.TFSFields.Add(field);
                if (field.AllowedValues.Any())
                {
                    this.settingViewModel.BugFilterFields.Add(field);
                }
            }
            this.settingViewModel.CanConnect = true;
        }

        private void ClearTFSData()
        {
            TempTFSData.CanConnect = this.settingViewModel.CanConnect;
            this.settingViewModel.CanConnect = false;
            this.settingViewModel.ProgressType = ProgressTypes.NotWorking;
            this.settingViewModel.ProgressValue = 0;

            TempTFSData.TFSFields = this.settingViewModel.TFSFields.ToList();
            this.settingViewModel.TFSFields.Clear();
            TempTFSData.BugFilterFields = this.settingViewModel.BugFilterFields.ToList();
            this.settingViewModel.BugFilterFields.Clear();
            TempTFSData.PriorityValues = this.settingViewModel.PriorityValues.ToList();
            this.settingViewModel.PriorityValues.Clear();

            this.document.PriorityRed = string.Empty;
            this.document.BugFilterField = string.Empty;
            this.document.BugFilterValue = string.Empty;
            foreach (var pair in this.document.PropertyMappingCollection)
            {
                pair.Value = string.Empty;
            }

            TempTFSData.StateValues = this.StateValues.ToList();
            this.StateValues.Clear();
            this.CanQuery = saveCommand.CanExecute();

            TempTFSData.CanRestore = true;
        }

        private void RestoreTFSData()
        {
            if (!TempTFSData.CanRestore)
            {
                return;
            }

            this.settingViewModel.CanConnect = TempTFSData.CanConnect;

            foreach (var item in TempTFSData.TFSFields)
            {
                this.settingViewModel.TFSFields.Add(item);
            }
            foreach (var item in TempTFSData.BugFilterFields)
            {
                this.settingViewModel.BugFilterFields.Add(item);
            }
            foreach (var item in TempTFSData.PriorityValues)
            {
                this.settingViewModel.PriorityValues.Add(item);
            }

            SettingDocument newDocument = null;
            if (File.Exists(SettingDocumentType.FilePath))
            {
                try
                {
                    newDocument = SettingDocumentType.Open();

                    this.document.ConnectUri = newDocument.ConnectUri;
                    this.document.UserName = newDocument.UserName;
                    this.document.Password = newDocument.Password;

                    this.document.PriorityRed = newDocument.PriorityRed;
                    this.document.BugFilterField = newDocument.BugFilterField;
                    this.document.BugFilterValue = newDocument.BugFilterValue;
                    foreach (var pair in this.document.PropertyMappingCollection)
                    {
                        pair.Value = newDocument.PropertyMappingCollection[pair.Key];
                    }
                }
                catch
                {
                }
            }

            foreach (var item in TempTFSData.StateValues)
            {
                this.StateValues.Add(item);
            }

            this.CanQuery = saveCommand.CanExecute();

            TempTFSData.Clear();
            TempTFSData.CanRestore = false;
        }

        private void DocumentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "PropertyMappingList")
            {
                UpdatePriorityValues();
                UpdateStatusValues();
            }
            else if (e.PropertyName == "ConnectUri" || e.PropertyName == "UserName" || e.PropertyName == "Password")
            {
                ClearTFSData();
            }

            UpdateCommands();
            this.CanQuery = this.saveCommand.CanExecute();
        }

        private void PriorityValuePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.document.PriorityRed = string.Join("; ",
                this.settingViewModel.PriorityValues.Where(x => x.IsChecked).Select(x => x.Name));
        }

        private void UpdateCommands()
        {
            this.saveCommand.RaiseCanExecuteChanged();
            this.testConnectionCommand.RaiseCanExecuteChanged();
        }

        private void UpdatePriorityValues()
        {
            string fieldName = this.document.PropertyMappingCollection["Priority"];

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
            string fieldName = this.document.PropertyMappingCollection["State"];

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
        }
        #endregion
        #endregion
    }
}
