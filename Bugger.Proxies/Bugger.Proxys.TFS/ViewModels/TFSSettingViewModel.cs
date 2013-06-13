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

        private SettingDocument settings;
        private ICommand saveCommand;
        private ICommand testConnectionCommand;
        private ICommand uriHelpCommand;
        private bool canConnect;
        private ObservableCollection<string> tfsFields;
        #endregion

        [ImportingConstructor]
        public TFSSettingViewModel(ITFSSettingView view, IMessageService messageService)
            : base(view)
        {
            this.messageService = messageService;

            this.canConnect = false;
            this.tfsFields = new ObservableCollection<string>();
        }

        #region Properties
        public ObservableCollection<string> TFSFields { get { return this.tfsFields; } }

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
