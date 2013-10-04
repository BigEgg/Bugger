using Bugger.Proxy;
using System.Collections.Generic;
using System.ComponentModel;

namespace Bugger.Applications.Services
{
    public interface IProxyService : INotifyPropertyChanged
    {
        IEnumerable<ITracingSystemProxy> Proxies { get; }

        ITracingSystemProxy ActiveProxy { get; set; }
    }
}
