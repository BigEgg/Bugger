using Bugger.Proxys.Models;
using System;
using System.Collections.Generic;
using System.Net;

namespace Bugger.Proxys
{
    public abstract class SourceController : ISourceController
    {
        #region Fields
        private Uri connectUri;
        private bool isFilterCreatedByFilter;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceController"/> class.
        /// </summary>
        /// <param name="serverName">The name of the server that is running the application tier for source control system.</param>
        /// <param name="port">The port that source control system uses.</param>
        /// <param name="virtualPath">the virtual path to the source control system application.</param>
        /// <exception cref="System.ArgumentException">
        /// serverName
        /// or
        /// virtualPath
        /// or
        /// Please Enter the right Uri name
        /// </exception>
        public SourceController(string serverName, uint port, string virtualPath)
        {
            SetConnectUri(serverName, port, virtualPath);
            this.isFilterCreatedByFilter = true;
        }

        #region Properties
        /// <summary>
        /// Gets or sets the connect URI.
        /// </summary>
        /// <value>
        /// The connect URI.
        /// </value>
        public Uri ConnectUri
        {
            get { return this.connectUri; }
            set { this.connectUri = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether filter the created by field.
        /// </summary>
        /// <value>
        ///   <c>true</c> if filter the created by field; otherwise, <c>false</c>.
        /// </value>
        public bool IsFilterCreatedByFilter
        {
            get { return this.isFilterCreatedByFilter; }
            set { this.isFilterCreatedByFilter = value; }
        }
        #endregion

        #region Methods
        #region Public Methods
        /// <summary>
        /// Sets the connection URI.
        /// </summary>
        /// <param name="serverName">The name of the server that is running the application tier for source control system.</param>
        /// <param name="port">The port that source control system uses.</param>
        /// <param name="virtualPath"></param>
        /// <exception cref="System.ArgumentException">
        /// serverName
        /// or
        /// virtualPath
        /// or
        /// Please Enter the right Uri name
        /// </exception>
        public void SetConnectUri(string serverName, uint port, string virtualPath)
        {
            if (string.IsNullOrWhiteSpace(serverName)) { throw new ArgumentException("serverName"); }
            if (string.IsNullOrWhiteSpace(virtualPath)) { throw new ArgumentException("virtualPath"); }

            string uri = string.Empty;
            if (!serverName.StartsWith("http://") && !serverName.StartsWith("https://"))
                serverName = "http://" + serverName;
            uri = serverName + ":" + port.ToString() + "/" + virtualPath;

            if (!Uri.TryCreate(uri, UriKind.Absolute, out this.connectUri))
            {
                throw new ArgumentException("Please Enter the right Uri name");
            }
        }

        /// <summary>
        /// Query the bugs with the specified credential information and user name which should be query.
        /// </summary>
        /// <param name="credential">The credential information.</param>
        /// <param name="userName">The user name which should be query..</param>
        /// <param name="workItemFilter">The string to filter the bugs.</param>
        /// <returns>
        /// The bugs.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">credential</exception>
        /// <exception cref="System.ArgumentException">userName</exception>
        public List<Bug> Query(NetworkCredential credential, string userName, string workItemFilter)
        {
            if (credential == null) { throw new ArgumentNullException("credential"); }
            if (string.IsNullOrWhiteSpace(userName)) { throw new ArgumentException("userName"); }

            return QueryCore(credential, userName, workItemFilter);
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// The core functionality query the bugs with the specified user name which should be query.
        /// </summary>
        /// <param name="credential">The credential information.</param>
        /// <param name="userName">The user name which should be query.</param>
        /// <param name="workItemFilter">The string to filter the bugs.</param>
        /// <returns>
        /// The bugs.
        /// </returns>
        /// <exception cref="System.NotSupportedException">The Query Method not support in the base class.</exception>
        protected virtual List<Bug> QueryCore(NetworkCredential credential, string userName, string workItemFilter)
        {
            throw new NotSupportedException("The Query Method not support in the base class.");
        }
        #endregion
        #endregion
    }
}
