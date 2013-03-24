using Bugger.Proxys.Models;
using System;
using System.Collections.Generic;
using System.Net;

namespace Bugger.Proxys
{
    public interface ISourceController
    {
        #region Properties
        /// <summary>
        /// Gets or sets the connect URI.
        /// </summary>
        /// <value>
        /// The connect URI.
        /// </value>
        Uri ConnectUri { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether filter the created by field.
        /// </summary>
        /// <value>
        ///   <c>true</c> if filter the created by field; otherwise, <c>false</c>.
        /// </value>
        bool IsFilterCreatedByFilter { get; set; }
        #endregion


        #region Methods
        /// <summary>
        /// Sets the connection URI.
        /// </summary>
        /// <param name="serverName">The name of the server that is running the application tier for source control system.</param>
        /// <param name="port">The port that source control system uses.</param>
        /// <param name="virualPath">the virtual path to the source control system application.</param>
        void SetConnectUri(string serverName, uint port, string virtualPath);

        /// <summary>
        /// Query the bugs with the specified credential information and user name which should be query.
        /// </summary>
        /// <param name="credential">The credential information.</param>
        /// <param name="userName">The user name which should be query..</param>
        /// <param name="workItemFilter">The string to filter the bugs.</param>
        /// <returns>
        /// The bugs.
        /// </returns>
        List<Bug> Query(NetworkCredential credential, string userName, string workItemFilter);
        #endregion
    }
}
