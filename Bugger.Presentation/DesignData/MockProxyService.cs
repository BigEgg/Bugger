using BigEgg.Framework.Applications.ViewModels;
using Bugger.Applications.Services;
using Bugger.Proxy;
using System.Collections.Generic;
using System.Linq;

namespace Bugger.Presentation.DesignData
{
    public class MockProxyService : DataModel, IProxyService
    {
        #region Fields
        private readonly IEnumerable<ITracingSystemProxy> proxys;
        private ITracingSystemProxy activeProxy;
        #endregion

        public MockProxyService()
        {
            this.proxys = new List<ITracingSystemProxy> { new FakeProxy() };
            this.activeProxy = this.proxys.First();
        }

        #region Properties
        public IEnumerable<ITracingSystemProxy> Proxies { get { return this.proxys; } }

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
