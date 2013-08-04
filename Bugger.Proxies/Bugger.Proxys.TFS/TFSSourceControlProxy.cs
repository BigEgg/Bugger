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

namespace Bugger.Proxy.TFS
{
    [Export(typeof(ISourceControlProxy)), Export]
    public class TFSSourceControlProxy : SourceControlProxy
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
        public override void SaveSettings()
        {
            if (this.saveCommand.CanExecute())
                this.saveCommand.Execute();
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
            if (!this.tfsHelper.IsConnected())
                this.tfsHelper.TestConnection(this.document.ConnectUri, this.document.UserName, this.document.Password);

            List<Bug> bugs = new List<Bug>();
            List<string> redFilter = string.IsNullOrWhiteSpace(this.document.PriorityRed)
                                         ? new List<string>()
                                         : this.document.PriorityRed.Split(';').Select(x => x.Trim()).ToList();

            foreach (string userName in userNames)
            {
                var bugCollection = this.tfsHelper.GetBugs(userName, isFilterCreatedBy, this.document.PropertyMappingCollection, this.document.BugFilterField, this.document.BugFilterValue, redFilter);
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
            return this.document != null && this.document.ConnectUri != null && this.document.ConnectUri.IsAbsoluteUri
                && !string.IsNullOrWhiteSpace(this.document.UserName);
        }

        private void TestConnectionCommandExcute()
        {
            this.settingViewModel.CanConnect = false;

            if (!this.tfsHelper.TestConnection(this.document.ConnectUri, this.document.UserName, this.document.Password))
            {
                messageService.ShowMessage(Resources.CannotConnect);
                return;
            }

            var fields = this.tfsHelper.GetFields();
            if (fields == null)
            {
                messageService.ShowMessage(Resources.CannotQueryFields);
                return;
            }

            this.settingViewModel.TFSFields.Clear();
            this.settingViewModel.BugFilterFields.Clear();
            foreach (var field in fields)
            {
                this.settingViewModel.TFSFields.Add(field);
                if (field.AllowedValues.Any())
                {
                    this.settingViewModel.BugFilterFields.Add(field);
                }
            }

            this.settingViewModel.CanConnect = true;
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
        #endregion
        #endregion
    }
}
