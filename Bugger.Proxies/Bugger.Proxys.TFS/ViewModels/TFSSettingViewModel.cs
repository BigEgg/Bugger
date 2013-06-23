using BigEgg.Framework.Applications.ViewModels;
using Bugger.Proxy.TFS.Documents;
using Bugger.Proxy.TFS.Models;
using Bugger.Proxy.TFS.Views;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows.Input;

namespace Bugger.Proxy.TFS.ViewModels
{
    [Export]
    public class TFSSettingViewModel : ViewModel<ITFSSettingView>
    {
        #region Fields
        private readonly ObservableCollection<TFSField> tfsFields;
        private readonly ObservableCollection<TFSField> bugFilterFields;
        private readonly ObservableCollection<CheckString> priorityValues;

        private SettingDocument settings;
        private ICommand saveCommand;
        private ICommand testConnectionCommand;
        private ICommand uriHelpCommand;
        private bool canConnect;
        #endregion

        [ImportingConstructor]
        public TFSSettingViewModel(ITFSSettingView view)
            : base(view)
        {
            this.canConnect = false;
            this.tfsFields = new ObservableCollection<TFSField>();
            this.bugFilterFields = new ObservableCollection<TFSField>();
            this.priorityValues = new ObservableCollection<CheckString>();
        }

        #region Properties
        public ObservableCollection<TFSField> TFSFields { get { return this.tfsFields; } }

        public ObservableCollection<TFSField> BugFilterFields { get { return this.bugFilterFields; } }

        public ObservableCollection<CheckString> PriorityValues { get { return this.priorityValues; } }

        public SettingDocument Settings
        {
            get { return this.settings; }
            internal set { this.settings = value; }
        }

        public ICommand UriHelpCommand 
        { 
            get { return this.uriHelpCommand; }
            set
            {
                if (this.uriHelpCommand != value)
                {
                    this.uriHelpCommand = value;
                    RaisePropertyChanged("UriHelpCommand");
                }
            }
        }

        public ICommand SaveCommand
        {
            get { return this.saveCommand; }
            set
            {
                if (this.saveCommand != value)
                {
                    this.saveCommand = value;
                    RaisePropertyChanged("SaveCommand");
                }
            }
        }

        public ICommand TestConnectionCommand
        {
            get { return this.testConnectionCommand; }
            set
            {
                if (this.testConnectionCommand != value)
                {
                    this.testConnectionCommand = value;
                    RaisePropertyChanged("TestConnectionCommand");
                }
            }
        }

        public bool CanConnect
        {
            get { return this.canConnect; }
            internal set
            {
                if (this.canConnect != value)
                {
                    this.canConnect = value;
                    RaisePropertyChanged("CanConnect");
                }
            }
        }
        #endregion
    }
}
