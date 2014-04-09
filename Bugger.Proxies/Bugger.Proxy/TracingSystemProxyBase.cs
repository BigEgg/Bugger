using Bugger.Models;
using Bugger.Models.Proxies.Models;
using Bugger.Plugins;
using Bugger.Plugins.Proxies;
using Bugger.Plugins.Proxies.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Bugger.Proxy
{
    /// <summary>
    /// The base class of the application proxy for source control system.
    /// </summary>
    public abstract class TracingSystemProxyBase : PluginBase, ITracingSystemProxy
    {
        #region Fields
        private bool canQuery;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="TracingSystemProxyBase" /> class.
        /// </summary>
        /// <param name="proxyName">Name of the proxy.</param>
        /// <param name="uniqueName">The unique name of this plug-in.</param>
        /// <param name="description">The description of this plug-in.</param>
        /// <param name="minimumApplicationVersion">The application's minimum version that this plug-in support.</param>
        /// <exception cref="System.ArgumentNullException">proxyName cannot be null or white space.</exception>
        public TracingSystemProxyBase(string proxyName,
                                      string uniqueName,
                                      string description,
                                      Version minimumApplicationVersion)
            : base(uniqueName, string.Format("%1 Proxy", proxyName), description, PluginCategory.Proxy, minimumApplicationVersion)
        {
            if (string.IsNullOrWhiteSpace(proxyName)) { throw new ArgumentNullException("proxyName cannot be null or white space."); }

            this.ProxyName = proxyName;
            this.canQuery = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TracingSystemProxyBase" /> class.
        /// </summary>
        /// <param name="proxyName">Name of the proxy.</param>
        /// <param name="uniqueName">The unique name of this plug-in.</param>
        /// <param name="description">The description of this plug-in.</param>
        /// <param name="minimumApplicationVersion">The application's minimum version that this plug-in support.</param>
        /// <param name="maximumApplicationVersion">The application's maximum version that this plug-in support.</param>
        /// <exception cref="System.ArgumentNullException">proxyName cannot be null or white space.</exception>
        public TracingSystemProxyBase(string proxyName,
                                      string uniqueName,
                                      string description,
                                      Version minimumApplicationVersion,
                                      Version maximumApplicationVersion)
            : base(uniqueName, string.Format("%1 Proxy", proxyName), description, PluginCategory.Proxy, minimumApplicationVersion, maximumApplicationVersion)
        {
            if (string.IsNullOrWhiteSpace(proxyName)) { throw new ArgumentNullException("proxyName cannot be null or white space."); }

            this.ProxyName = proxyName;
            this.canQuery = false;
        }

        #region Properties
        /// <summary>
        /// Gets the name of the proxy.
        /// </summary>
        /// <value>
        /// The name of the proxy.
        /// </value>
        public string ProxyName { get; protected set; }

        /// <summary>
        /// Determines whether this source control proxy can query the bugs.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this source control proxy can query the bugs.; otherwise, <c>false</c>.
        /// </returns>
        public bool CanQuery
        {
            get { return this.canQuery; }
            protected set { this.canQuery = value; }
        }

        /// <summary>
        /// Gets the status values.
        /// </summary>
        /// <value>
        /// The status values.
        /// </value>
        public abstract ObservableCollection<string> StateValues { get; }
        #endregion

        #region Methods
        #region Public Methods
        /// <summary>
        /// Query the bugs with the specified user name which the bug assign to.
        /// </summary>
        /// <param name="userName">The user name which the bug assign to.</param>
        /// <param name="isFilterCreatedBy">if set to <c>true</c> indicating whether filter the created by field.</param>
        /// <returns>
        /// The bugs.
        /// </returns>
        /// <exception cref="System.ArgumentException">userName</exception>
        /// <exception cref="System.NotSupportedException">The Query operation is not supported. CanQuery returned false.</exception>
        public ReadOnlyCollection<IBug> Query(string userName, bool isFilterCreatedBy = true)
        {
            if (string.IsNullOrWhiteSpace(userName)) { throw new ArgumentException("userName"); }

            return Query(new List<string>() { userName }, isFilterCreatedBy);
        }

        /// <summary>
        /// Query the bugs with the specified team members name list which the bug assign to.
        /// </summary>
        /// <param name="teamMembers">The team members name list which the bug assign to.</param>
        /// <param name="isFilterCreatedBy">if set to <c>true</c> indicating whether filter the created by field.</param>
        /// <returns>
        /// The bugs.
        /// </returns>
        /// <exception cref="System.ArgumentException">teamMembers</exception>
        /// <exception cref="System.NotSupportedException">The Query operation is not supported. CanQuery returned false.</exception>
        public ReadOnlyCollection<IBug> Query(List<string> teamMembers, bool isFilterCreatedBy = true)
        {
            if (teamMembers == null) { throw new ArgumentException("teamMembers"); }
            if (!CanQuery) { throw new NotSupportedException("The Query operation is not supported. CanQuery returned false."); }

            if (teamMembers.Count == 0)
            {
                return new ReadOnlyCollection<IBug>(new List<IBug>());
            }

            return QueryCore(teamMembers, isFilterCreatedBy);
        }

        /// <summary>
        /// Initializes the values before open the setting dialog.
        /// </summary>
        /// <returns></returns>
        public virtual ISettingView InitializeSettingDialog()
        {
            return null;
        }

        /// <summary>
        /// Do something afters close setting dialog.
        /// </summary>
        /// <param name="submit">if set to <c>true</c> [submit].</param>
        public virtual void AfterCloseSettingDialog(bool submit)
        { }

        /// <summary>
        /// Validate the setting values before close setting dialog.
        /// </summary>
        /// <returns>
        /// The validation result.
        /// </returns>
        public virtual SettingDialogValidateionResult ValidateBeforeCloseSettingDialog()
        {
            return SettingDialogValidateionResult.Valid;
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Query the bugs with the specified user names list which the bug assign to.
        /// </summary>
        /// <param name="userNames">The user names list which the bug assign to.</param>
        /// <param name="isFilterCreatedBy">if set to <c>true</c> indicating whether filter the created by field.</param>
        /// <returns>
        /// The bugs.
        /// </returns>
        /// <exception cref="System.NotImplementedException">The Query Method not implemented in the base class.</exception>
        protected virtual ReadOnlyCollection<IBug> QueryCore(List<string> userNames, bool isFilterCreatedBy)
        {
            throw new NotImplementedException("The Query Method not implemented in the base class.");
        }
        #endregion
        #endregion
    }
}
