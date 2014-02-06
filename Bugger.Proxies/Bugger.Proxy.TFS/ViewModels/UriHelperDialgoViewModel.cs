using BigEgg.Framework.Applications.Commands;
using BigEgg.Framework.Applications.ViewModels;
using BigEgg.Framework.Foundation;
using BigEgg.Framework.Foundation.Validations;
using Bugger.Proxy.TFS.Properties;
using Bugger.Proxy.TFS.Views;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace Bugger.Proxy.TFS.ViewModels
{
    public class UriHelperDialogViewModel : DialogViewModel<IUriHelperDialogView>, IDataErrorInfo
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

        /// <summary>
        /// Initializes a new instance of the <see cref="UriHelperDialogViewModel"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        public UriHelperDialogViewModel(IUriHelperDialogView view)
            : base(view)
        {
            this.dataErrorInfoSupport = new DataErrorInfoSupport(this);

            this.serverName = string.Empty;
            this.port = 8080;
            this.path = "tfs";
            this.isHttpsProtocal = false;
            this.uriPreview = Resources.InvalidUrl;

            this.submitCommand = new DelegateCommand(() => Close(true), CanSubmitCommandExecute);
            this.cancelCommand = new DelegateCommand(() => Close(false));
        }

        #region Implement IDataErrorInfo interface
        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        /// <returns>An error message indicating what is wrong with this object. The default is an empty string ("").</returns>
        string IDataErrorInfo.Error { get { return this.dataErrorInfoSupport.Error; } }

        /// <summary>
        /// Gets the error message for the property with the given name.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <returns>The error message for the property. The default is an empty string ("").</returns>
        string IDataErrorInfo.this[string columnName] { get { return this.dataErrorInfoSupport[columnName]; } }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the submit command.
        /// </summary>
        /// <value>
        /// The submit command.
        /// </value>
        public ICommand SubmitCommand { get { return this.submitCommand; } }

        /// <summary>
        /// Gets the cancel command.
        /// </summary>
        /// <value>
        /// The cancel command.
        /// </value>
        public ICommand CancelCommand { get { return this.cancelCommand; } }

        /// <summary>
        /// Gets or sets the server name.
        /// </summary>
        /// <value>
        /// The server name.
        /// </value>
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

        /// <summary>
        /// Gets a value indicating whether the user can edit connection detail informations.
        /// </summary>
        /// <value>
        /// <c>true</c> if the user can edit connection detail informations; otherwise, <c>false</c>.
        /// </value>
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

        /// <summary>
        /// Gets or sets the connection path.
        /// </summary>
        /// <value>
        /// The connection path.
        /// </value>
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

        /// <summary>
        /// Gets or sets the TFS Server port.
        /// </summary>
        /// <value>
        /// The TFS Server port.
        /// </value>
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

        /// <summary>
        /// Gets or sets a value indicating whether the connection is use HTTPS protocol.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the connection is use HTTPS protocol; otherwise, <c>false</c>.
        /// </value>
        public bool IsHttpsProtocol
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
                    RaisePropertyChanged("IsHttpsProtocol");
                }
            }
        }

        /// <summary>
        /// Gets the URI preview string.
        /// </summary>
        /// <value>
        /// The URI preview string.
        /// </value>
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

        /// <summary>
        /// Gets the dialog's title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title
        {
            get { return Resources.UriHelpDialogTitle; }
        }
        #endregion

        #region Methods
        #region Private Methods
        /// <summary>
        /// Checks is the URI is valid.
        /// </summary>
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
                    uriString = (IsHttpsProtocol ? "https://" : "http://") + this.serverName;
                    uriString = uriString + ":" + this.port.ToString() + "/" + this.path;
                    if (Uri.TryCreate(uriString, UriKind.Absolute, out uri))
                        this.UriPreview = uri.AbsoluteUri;
                    else
                        this.UriPreview = Resources.InvalidUrl;
                }
            }
            UpdateCommands();
        }

        /// <summary>
        /// Determines whether the SubmitCommand can execute.
        /// </summary>
        /// <returns></returns>
        private bool CanSubmitCommandExecute()
        {
            return this.UriPreview != Resources.InvalidUrl;
        }

        /// <summary>
        /// Updates the commands' status.
        /// </summary>
        private void UpdateCommands()
        {
            this.submitCommand.RaiseCanExecuteChanged();
        }
        #endregion
        #endregion
    }
}
