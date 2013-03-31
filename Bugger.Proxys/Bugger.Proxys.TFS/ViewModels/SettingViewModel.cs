using BigEgg.Framework.Applications.Commands;
using BigEgg.Framework.Applications.Services;
using BigEgg.Framework.Applications.ViewModels;
using Bugger.Proxys.TFS.Documents;
using Bugger.Proxys.TFS.Properties;
using Bugger.Proxys.TFS.Views;
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

namespace Bugger.Proxys.TFS.ViewModels
{
    [Export]
    public class SettingViewModel : ViewModel<ISettingView>
    {
        #region Fields
        private readonly CompositionContainer container;
        private readonly DelegateCommand testConnectionCommand;
        private readonly DelegateCommand uriHelpCommand;

        private SettingDocument settings;
        private ICommand saveCommand;
        private bool canConnect;
        private ReadOnlyCollection<string> readonlyTFSFields;
        private List<string> tfsFields;
        #endregion

        [ImportingConstructor]
        public SettingViewModel(ISettingView view, CompositionContainer container)
            : base(view)
        {
            this.container = container;

            this.canConnect = false;
            this.tfsFields = new List<string>();
            this.readonlyTFSFields = new ReadOnlyCollection<string>(this.tfsFields);

            this.testConnectionCommand = new DelegateCommand(TestConnectionExcute, CanTestConnectionExcute);
            this.uriHelpCommand = new DelegateCommand(OpenUriHelpExcute);
        }

        #region Properties
        public ReadOnlyCollection<string> TFSFields { get { return this.readonlyTFSFields; } }

        internal SettingDocument Settings
        {
            get { return this.settings; }
            set { this.settings = value; }
        }

        public ICommand UriHelpCommand { get { return this.uriHelpCommand; } }

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

            IMessageService messageService = this.container.GetExportedValue<IMessageService>();
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

        private void OpenUriHelpExcute()
        {
            IUriHelpView view = this.container.GetExportedValue<IUriHelpView>();
            UriHelpViewModel viewModel = new UriHelpViewModel(view);

            viewModel.ShowDialog(this);

            if (viewModel.UriPreview == Resources.InvalidUrl)
                this.settings.ConnectUri = null;
            else
                this.settings.ConnectUri = new Uri(viewModel.UriPreview);
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
