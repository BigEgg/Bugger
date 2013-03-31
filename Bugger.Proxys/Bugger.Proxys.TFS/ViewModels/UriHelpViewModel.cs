using BigEgg.Framework.Applications.Commands;
using BigEgg.Framework.Applications.ViewModels;
using BigEgg.Framework.Foundation;
using BigEgg.Framework.Foundation.Validations;
using Bugger.Proxys.TFS.Properties;
using Bugger.Proxys.TFS.Views;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace Bugger.Proxys.TFS.ViewModels
{
    public class UriHelpViewModel : DialogViewModel<IUriHelpView>, IDataErrorInfo 
    {
        #region Fields
        protected readonly DataErrorInfoSupport dataErrorInfoSupport;

        private readonly DelegateCommand submitCommand;
        private readonly DelegateCommand cancelCommand;

        private string serverName;
        private string path;
        private uint port;
        private bool isHttpsProtocal;
        private string uriPreview;
        #endregion

        public UriHelpViewModel(IUriHelpView view)
            : base(view)
        {
            this.dataErrorInfoSupport = new DataErrorInfoSupport(this);

            this.serverName = string.Empty;
            this.port = 8080;
            this.path = "tfs";
            this.isHttpsProtocal = false;
            this.uriPreview = Resources.InvalidUrl;

            this.submitCommand = new DelegateCommand(() => Close(true), CanSubmitSetting);
            this.cancelCommand = new DelegateCommand(() => Close(false));
        }

        #region Implement IDataErrorInfo interface
        string IDataErrorInfo.Error { get { return this.dataErrorInfoSupport.Error; } }

        string IDataErrorInfo.this[string columnName] { get { return this.dataErrorInfoSupport[columnName]; } }
        #endregion

        #region Properties
        public ICommand SubmitCommand { get { return this.submitCommand; } }

        public ICommand CancelCommand { get { return this.cancelCommand; } }

        [Required(ErrorMessageResourceName = "ServerNameMandatory", ErrorMessageResourceType = typeof(Resources))]
        public string ServerName
        {
            get { return this.serverName; }
            set
            {
                if (this.serverName != value)
                {
                    this.serverName = value;
                    RaisePropertyChanged("ServerName");
                    RaisePropertyChanged("CanEditConnectionDetail");
                    CheckUri();
                }
            }
        }

        public bool CanEditConnectionDetail
        {
            get 
            {
                if (string.IsNullOrWhiteSpace(serverName))
                    return true;
                else
                    return !serverName.StartsWith("http://") && !serverName.StartsWith("https://"); 
            }
        }

        [RequiredIf("CanEditConnectionDetail", true, ErrorMessageResourceName = "PathMandatory", ErrorMessageResourceType = typeof(Resources))]
        public string Path
        {
            get { return this.path; }
            set
            {
                if (this.path != value)
                {
                    this.path = value;
                    CheckUri();
                    RaisePropertyChanged("Path");
                }
            }
        }

        [RequiredIf("CanEditConnectionDetail", true, ErrorMessageResourceName = "PortMandatory", ErrorMessageResourceType = typeof(Resources))]
        [Range(1, 65535, ErrorMessageResourceName = "PortRange", ErrorMessageResourceType = typeof(Resources))]
        public uint Port
        {
            get { return this.port; }
            set
            {
                if (this.port != value)
                {
                    this.port = value;
                    CheckUri();
                    RaisePropertyChanged("Port");
                }
            }
        }

        public bool IsHttpsProtocal
        {
            get { return this.isHttpsProtocal; }
            set
            {
                if (this.isHttpsProtocal != value)
                {
                    this.isHttpsProtocal = value;
                    if (value && this.Port == 8080)
                        this.Port = 443;
                    else if (!value && this.Port == 443)
                        this.Port = 8080;
                    
                    CheckUri();
                    RaisePropertyChanged("IsHttpsProtocal");
                }
            }
        }

        public string UriPreview
        {
            get { return this.uriPreview; }
            private set
            {
                if (this.uriPreview != value)
                {
                    this.uriPreview = value;
                    RaisePropertyChanged("UriPreview");
                }
            }
        }

        public override string Title
        {
            get { return Resources.UriHelpDialogTitle; }
        }
        #endregion

        #region Methods
        #region Private Methdos
        private void CheckUri()
        {
            Uri uri = null;

            if (!string.IsNullOrWhiteSpace(this.Validate()))
            {
                this.UriPreview = Resources.InvalidUrl;
                return;
            }
            else
            {
                if (!CanEditConnectionDetail)
                {
                    if (Uri.TryCreate(this.serverName, UriKind.Absolute, out uri))
                        this.UriPreview = uri.AbsoluteUri;
                }
                else
                {
                    string uriString = string.Empty;
                    uriString = (IsHttpsProtocal ? "https://" : "http://") + this.serverName;
                    uriString = uriString + ":" + this.port.ToString() + "/" + this.path;
                    if (Uri.TryCreate(uriString, UriKind.Absolute, out uri))
                        this.UriPreview = uri.AbsoluteUri;
                    else
                        this.UriPreview = Resources.InvalidUrl;
                }
            }
            UpdateCommands();
        }

        private bool CanSubmitSetting()
        {
            return this.UriPreview != Resources.InvalidUrl;
        }

        private void UpdateCommands()
        {
            this.submitCommand.RaiseCanExecuteChanged();
        }
        #endregion
        #endregion
    }
}
