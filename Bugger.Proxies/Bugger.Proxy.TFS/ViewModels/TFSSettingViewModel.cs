using BigEgg.Framework.Applications.Commands;
using BigEgg.Framework.Applications.ViewModels;
using Bugger.Proxy.Models;
using Bugger.Proxy.TFS.Models;
using Bugger.Proxy.TFS.Properties;
using Bugger.Proxy.TFS.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Bugger.Proxy.TFS.ViewModels
{
    public class TFSSettingViewModel : ViewModel<ITFSSettingView>
    {
        #region Fields
        private readonly ObservableCollection<TFSField> tfsFields;
        private readonly ObservableCollection<TFSField> bugFilterFields;
        private readonly ObservableCollection<CheckString> priorityValues;
        private readonly PropertyMappingDictionary propertyMappingCollection;
        private Uri connectUri;
        private string userName;
        private string password;
        private string bugFilterField;
        private string bugFilterValue;
        private string priorityRed;

        private readonly DelegateCommand openUriHelperDialogCommand;
        private ICommand testConnectionCommand;

        private readonly IUriHelperDialogView uriHelpView;

        private ProgressType progressType;
        private int progressValue;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="TFSSettingViewModel"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="uriHelpView">The URI help view.</param>
        /// <exception cref="System.ArgumentNullException">uriHelpView</exception>
        public TFSSettingViewModel(ITFSSettingView view, IUriHelperDialogView uriHelpView)
            : base(view)
        {
            if (uriHelpView == null) { throw new ArgumentNullException("uriHelpView"); }

            this.uriHelpView = uriHelpView;

            this.openUriHelperDialogCommand = new DelegateCommand(OpenUriHelperDialogCommandExecute);

            this.propertyMappingCollection = new PropertyMappingDictionary();
            this.tfsFields = new ObservableCollection<TFSField>();
            this.bugFilterFields = new ObservableCollection<TFSField>();
            this.priorityValues = new ObservableCollection<CheckString>();

            this.propertyMappingCollection = BugHelper.GetPropertyNames();
            foreach (var mappingModel in propertyMappingCollection)
            {
                AddWeakEventListener(mappingModel, PropertyMappingModelPropertyChanged);
            }

            ClearMappingData();
        }

        #region Properties
        /// <summary>
        /// Gets the TFS fields.
        /// </summary>
        /// <value>
        /// The TFS fields.
        /// </value>
        public ObservableCollection<TFSField> TFSFields { get { return this.tfsFields; } }

        /// <summary>
        /// Gets the bug filter fields.
        /// </summary>
        /// <value>
        /// The bug filter fields.
        /// </value>
        public ObservableCollection<TFSField> BugFilterFields { get { return this.bugFilterFields; } }

        /// <summary>
        /// Gets the priority values.
        /// </summary>
        /// <value>
        /// The priority values.
        /// </value>
        public ObservableCollection<CheckString> PriorityValues { get { return this.priorityValues; } }

        /// <summary>
        /// Gets the property mapping collection.
        /// </summary>
        /// <value>
        /// The property mapping collection.
        /// </value>
        public PropertyMappingDictionary PropertyMappingCollection { get { return this.propertyMappingCollection; } }

        /// <summary>
        /// Gets or sets the connect URI.
        /// </summary>
        /// <value>
        /// The connect URI.
        /// </value>
        public Uri ConnectUri
        {
            get { return this.connectUri; }
            set
            {
                if (this.connectUri != value)
                {
                    this.connectUri = value;
                    RaisePropertyChanged("ConnectUri");
                }
            }
        }

        /// <summary>
        /// Gets or sets the TFS user name.
        /// </summary>
        /// <value>
        /// The TFS user name.
        /// </value>
        public string UserName
        {
            get { return this.userName; }
            set
            {
                if (this.userName != value)
                {
                    this.userName = value;
                    RaisePropertyChanged("UserName");
                }
            }
        }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password
        {
            get { return this.password; }
            set
            {
                if (this.password != value)
                {
                    this.password = value;
                    RaisePropertyChanged("Password");
                }
            }
        }

        /// <summary>
        /// Gets or sets the bug filter field.
        /// </summary>
        /// <value>
        /// The bug filter field.
        /// </value>
        public string BugFilterField
        {
            get { return this.bugFilterField; }
            set
            {
                if (this.bugFilterField != value)
                {
                    this.bugFilterField = value;
                    RaisePropertyChanged("BugFilterField");
                }
            }
        }

        /// <summary>
        /// Gets or sets the bug filter value.
        /// </summary>
        /// <value>
        /// The bug filter value.
        /// </value>
        public string BugFilterValue
        {
            get { return this.bugFilterValue; }
            set
            {
                if (this.bugFilterValue != value)
                {
                    this.bugFilterValue = value;
                    RaisePropertyChanged("BugFilterValue");
                }
            }
        }

        /// <summary>
        /// Gets or sets the priority value to indicate which bug is the red gift.
        /// </summary>
        /// <value>
        /// The priority value.
        /// </value>
        public string PriorityRed
        {
            get { return this.priorityRed; }
            set
            {
                if (this.priorityRed != value)
                {
                    this.priorityRed = value;
                    RaisePropertyChanged("PriorityRed");
                }
            }
        }

        /// <summary>
        /// Gets the OpenUriHelperDialog command.
        /// </summary>
        /// <value>
        /// The URI help command.
        /// </value>
        public ICommand OpenUriHelperDialogCommand { get { return this.openUriHelperDialogCommand; } }

        /// <summary>
        /// Gets the test connection command.
        /// </summary>
        /// <value>
        /// The test connection command.
        /// </value>
        public ICommand TestConnectionCommand
        {
            get { return this.testConnectionCommand; }
            internal set
            {
                if (this.testConnectionCommand != value)
                {
                    this.testConnectionCommand = value;
                    RaisePropertyChanged("TestConnectionCommand");
                }
            }
        }

        /// <summary>
        /// Gets or sets the progress type of the setting view-model.
        /// </summary>
        /// <value>
        /// The progress type of the setting view-model.
        /// </value>
        public ProgressType ProgressType
        {
            get { return this.progressType; }
            set
            {
                if (this.progressType != value)
                {
                    this.progressType = value;
                    RaisePropertyChanged("ProgressType");
                }
            }
        }

        /// <summary>
        /// Gets or sets the progress value.
        /// </summary>
        /// <value>
        /// The progress value.
        /// </value>
        public int ProgressValue
        {
            get { return this.progressValue; }
            set
            {
                if (this.progressValue != value)
                {
                    this.progressValue = value;
                    RaisePropertyChanged("ProgressValue");
                }
            }
        }
        #endregion

        #region Methods
        #region Public Methods
        /// <summary>
        /// Clears the mapping data.
        /// </summary>
        public void ClearMappingData()
        {
            TFSFields.Clear();
            BugFilterFields.Clear();
            PriorityValues.Clear();

            foreach (var mapping in PropertyMappingCollection)
            {
                mapping.Value = string.Empty;
            }

            BugFilterField = string.Empty;
            BugFilterValue = string.Empty;
            PriorityRed = string.Empty;

            ProgressType = ProgressType.NotWorking;
            ProgressValue = 0;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// The OpenUriHelpCommand execute.
        /// </summary>
        private void OpenUriHelperDialogCommandExecute()
        {
            UriHelperDialogViewModel viewModel = new UriHelperDialogViewModel(this.uriHelpView);
            if (this.ConnectUri != null)
            {
                viewModel.ServerName = this.ConnectUri.AbsoluteUri;
            }

            var result = viewModel.ShowDialog(this);

            if (result.HasValue && result.Value)
            {
                this.ConnectUri = viewModel.UriPreview == Resources.InvalidUrl
                                      ? null
                                      : new Uri(viewModel.UriPreview);
            }
        }

        /// <summary>
        /// Properties the mapping model property changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void PropertyMappingModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged("PropertyMappingCollection");
        }
        #endregion
        #endregion
    }
}
