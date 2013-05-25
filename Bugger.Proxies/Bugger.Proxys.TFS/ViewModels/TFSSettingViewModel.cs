using BigEgg.Framework.Applications.Commands;
using BigEgg.Framework.Applications.Services;
using BigEgg.Framework.Applications.ViewModels;
using Bugger.Proxy.TFS.Documents;
using Bugger.Proxy.TFS.Properties;
using Bugger.Proxy.TFS.Views;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Net;
using System.Windows.Input;

namespace Bugger.Proxy.TFS.ViewModels
{
    [Export]
    public class TFSSettingViewModel : ViewModel<ITFSSettingView>
    {
        #region Fields
        private readonly IMessageService messageService;
        private readonly DelegateCommand testConnectionCommand;

        private SettingDocument settings;
        private ICommand saveCommand;
        private ICommand uriHelpCommand;
        private bool canConnect;
        private ReadOnlyCollection<string> readonlyTFSFields;
        private List<string> tfsFields;
        #endregion

        [ImportingConstructor]
        public TFSSettingViewModel(ITFSSettingView view, IMessageService messageService)
            : base(view)
        {
            this.messageService = messageService;

            this.canConnect = false;
            this.tfsFields = new List<string>();
            this.readonlyTFSFields = new ReadOnlyCollection<string>(this.tfsFields);

            this.testConnectionCommand = new DelegateCommand(TestConnectionExcute, CanTestConnectionExcute);
        }

        #region Properties
        public ReadOnlyCollection<string> TFSFields { get { return this.readonlyTFSFields; } }

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

        public ICommand TestConnectionCommand { get { return this.testConnectionCommand; } }

        public bool CanConnect
        {
            get { return this.canConnect; }
            private set
            {
                if (this.canConnect != value)
                {
                    this.canConnect = value;
                    RaisePropertyChanged("CanConnect");
                }
            }
        }
        #endregion

        #region Methods
        #region Private Commands Methods
        private bool CanTestConnectionExcute()
        {
            return this.settings.ConnectUri != null && this.settings.ConnectUri.IsAbsoluteUri;
        }

        private void TestConnectionExcute()
        {
            this.CanConnect = false;

            TfsTeamProjectCollection tpc = null;

            try
            {
                tpc = new TfsTeamProjectCollection(
                    this.settings.ConnectUri,
                    new NetworkCredential(this.settings.UserName, this.settings.Password));
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
                foreach (FieldDefinition field in collection)
                {
                    this.tfsFields.Add(field.Name);
                }

                this.CanConnect = true;
                RaisePropertyChanged("TFSFields");
            }
            catch
            {
                messageService.ShowMessage(Resources.CannotQueryFields);
            }
        }
        #endregion
        #region Private Methods
        private void SettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ConnectUri")
            {
                UpdateCommands();
            }
        }

        private void UpdateCommands()
        {
            this.testConnectionCommand.RaiseCanExecuteChanged();
        }
        #endregion
        #endregion
    }
}
