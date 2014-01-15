using Bugger.Proxy;
using System.Collections.Generic;
using System.ComponentModel;

namespace Bugger.Applications.Services
{
    /// <summary>
    /// The interface that define all the data that related with the proxy
    /// which the service should contains.
    /// </summary>
    public interface IProxyService : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the proxies.
        /// </summary>
        /// <value>
        /// The proxies.
        /// </value>
        IEnumerable<ITracingSystemProxy> Proxies { get; }

        /// <summary>
        /// Gets or sets the active proxy.
        /// </summary>
        /// <value>
        /// The active proxy.
        /// </value>
        ITracingSystemProxy ActiveProxy { get; set; }
    }
}
