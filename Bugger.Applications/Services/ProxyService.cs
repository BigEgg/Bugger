using BigEgg.Framework.Applications.ViewModels;
using Bugger.Proxy;
using System.Collections.Generic;

namespace Bugger.Applications.Services
{
    /// <summary>
    /// The service class that contains all the data that related with the proxy.
    /// </summary>
    internal class ProxyService : DataModel, IProxyService
    {
        #region Fields
        private readonly IEnumerable<ITracingSystemProxy> proxies;
        private ITracingSystemProxy activeProxy;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyService"/> class.
        /// </summary>
        /// <param name="proxies">The proxies.</param>
        public ProxyService(IEnumerable<ITracingSystemProxy> proxies)
        {
            this.proxies = proxies;
        }

        #region Properties
        /// <summary>
        /// Gets the proxies.
        /// </summary>
        /// <value>
        /// The proxies.
        /// </value>
        public IEnumerable<ITracingSystemProxy> Proxies { get { return this.proxies; } }

        /// <summary>
        /// Gets or sets the active proxy.
        /// </summary>
        /// <value>
        /// The active proxy.
        /// </value>
        public ITracingSystemProxy ActiveProxy
        {
            get { return this.activeProxy; }
            set
            {
                if (this.activeProxy != value)
                {
                    this.activeProxy = value;
                    RaisePropertyChanged("ActiveProxy");
                }
            }
        }
        #endregion
    }
}
