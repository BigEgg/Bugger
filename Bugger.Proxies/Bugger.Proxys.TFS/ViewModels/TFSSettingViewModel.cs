using BigEgg.Framework.Applications.Commands;
using BigEgg.Framework.Applications.ViewModels;
using Bugger.Domain.Models;
using Bugger.Proxy.TFS.Models;
using Bugger.Proxy.TFS.Properties;
using Bugger.Proxy.TFS.Views;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

        private readonly DelegateCommand uriHelpCommand;
        private ICommand testConnectionCommand;

        private readonly IUriHelpView uriHelpView;

        private ProgressTypes progressType;
        private int progressValue;
        #endregion

        public TFSSettingViewModel(ITFSSettingView view, IUriHelpView uriHelpView)
            : base(view)
        {
            if (uriHelpView == null) { throw new ArgumentNullException("uriHelpView"); }

            this.uriHelpView = uriHelpView;

            this.uriHelpCommand = new DelegateCommand(OpenUriHelpCommandExcute);

            this.propertyMappingCollection = new PropertyMappingDictionary();
            this.tfsFields = new ObservableCollection<TFSField>();
            this.bugFilterFields = new ObservableCollection<TFSField>();
            this.priorityValues = new ObservableCollection<CheckString>();

            PropertyDescriptorCollection propertyDescriptorCollection = TypeDescriptor.GetProperties(typeof(Bug));
            foreach (PropertyDescriptor propertyDescriptor in propertyDescriptorCollection)
            {
                if (propertyDescriptor.Name == "Type") continue;

                var mapping = new MappingModel(propertyDescriptor.Name);
                this.propertyMappingCollection.Add(mapping);

                AddWeakEventListener(mapping, PropertyMappingModelPropertyChanged);
            }

            ClearData();
        }

        #region Properties
        public ObservableCollection<TFSField> TFSFields { get { return this.tfsFields; } }

        public ObservableCollection<TFSField> BugFilterFields { get { return this.bugFilterFields; } }

        public ObservableCollection<CheckString> PriorityValues { get { return this.priorityValues; } }

        public PropertyMappingDictionary PropertyMappingCollection { get { return this.propertyMappingCollection; } }

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

        public ICommand UriHelpCommand { get { return this.uriHelpCommand; } }

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

        public ProgressTypes ProgressType
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
        public void ClearData()
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

            ProgressType = ProgressTypes.NotWorking;
            ProgressValue = 0;
        }
        #endregion

        #region Private Methods
        private void OpenUriHelpCommandExcute()
        {
            UriHelpViewModel viewModel = new UriHelpViewModel(this.uriHelpView);
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

        private void PropertyMappingModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged("PropertyMappingCollection");
        }
        #endregion
        #endregion
    }
}
