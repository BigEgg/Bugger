using BigEgg.Framework.Applications.ViewModels;
using Bugger.Proxys.Models;
using System;
using System.Collections.Generic;

namespace Bugger.Proxys
{
    /// <summary>
    /// The base class of the application proxy for source control system.
    /// </summary>
    public abstract class SourceControlProxy : Controller, ISourceControlProxy
    {
        #region Fields
        private string proxyName;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceControlProxy" /> class.
        /// </summary>
        /// <param name="proxyName">Name of the proxy.</param>
        /// <exception cref="System.ArgumentException">proxyName</exception>
        public SourceControlProxy(string proxyName)
        {
            if (string.IsNullOrWhiteSpace(proxyName)) { throw new ArgumentException("proxyName"); }

            this.proxyName = proxyName;
        }

        #region Properties
        /// <summary>
        /// Gets the name of the proxy.
        /// </summary>
        /// <value>
        /// The name of the proxy.
        /// </value>
        public string ProxyName
        {
            get { return this.proxyName; }
            protected set
            {
                this.proxyName = value;
            }
        }

        /// <summary>
        /// Gets the setting view model.
        /// </summary>
        /// <value>
        /// The setting view model.
        /// </value>
        public abstract ViewModel SettingViewModel { get; }
        #endregion

        #region Methods
        #region Public Methods
        /// <summary>
        /// Determines whether this source control proxy can query the bugs.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this source control proxy can query the bugs.; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool CanQuery() { return false; }

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
        public List<Bug> Query(string userName, bool isFilterCreatedBy = true)
        {
            if (string.IsNullOrWhiteSpace(userName)) { throw new ArgumentException("userName"); }
            if (!CanQuery()) { throw new NotSupportedException("The Query operation is not supported. CanQuery returned false."); }

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
        public List<Bug> Query(List<string> teamMembers, bool isFilterCreatedBy = true)
        {
            if (teamMembers == null) { throw new ArgumentException("teamMembers"); }
            if (!CanQuery()) { throw new NotSupportedException("The Query operation is not supported. CanQuery returned false."); }

            if (teamMembers.Count == 0)
                return new List<Bug>();

            return QueryCore(teamMembers, isFilterCreatedBy);
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
        protected virtual List<Bug> QueryCore(List<string> userNames, bool isFilterCreatedBy)
        {
            throw new NotImplementedException("The Query Method not implemented in the base class.");
        }
        #endregion
        #endregion
    }
}
