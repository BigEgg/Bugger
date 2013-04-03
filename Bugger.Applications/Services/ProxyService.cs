using BigEgg.Framework.Applications.ViewModels;
using Bugger.Proxys;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Bugger.Applications.Services
{
    [Export(typeof(IProxyService)), Export]
    public class ProxyService : DataModel, IProxyService
    {
        #region Fields
        private readonly IEnumerable<ISourceControlProxy> proxys;
        private ISourceControlProxy activeProxy;
        #endregion

        [ImportingConstructor]
        public ProxyService(IEnumerable<ISourceControlProxy> proxys)
        {
            this.proxys = proxys;
        }

        #region Properties
        public IEnumerable<ISourceControlProxy> Proxys { get { return this.proxys; } }

        public ISourceControlProxy ActiveProxy
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
