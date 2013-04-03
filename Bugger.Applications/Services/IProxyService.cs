using Bugger.Proxys;
using System.Collections.Generic;
using System.ComponentModel;

namespace Bugger.Applications.Services
{
    public interface IProxyService : INotifyPropertyChanged
    {
        IEnumerable<ISourceControlProxy> Proxys { get; }

        ISourceControlProxy ActiveProxy { get; set; }
    }
}
