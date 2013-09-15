using BigEgg.Framework.Applications.ViewModels;
using Bugger.Proxy;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Bugger.Applications.Services
{
    internal class ProxyService : DataModel, IProxyService
    {
        #region Fields
        private readonly IEnumerable<ITracingSystemProxy> proxys;
        private ITracingSystemProxy activeProxy;
        #endregion

        public ProxyService(IEnumerable<ITracingSystemProxy> proxys)
        {
            this.proxys = proxys;
        }

        #region Properties
        public IEnumerable<ITracingSystemProxy> Proxys { get { return this.proxys; } }

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
