using BigEgg.Framework.Applications.ViewModels;
using Bugger.Applications.Properties;
using Bugger.Applications.Services;
using Bugger.Proxy;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;

namespace Bugger.Applications.Controllers
{
    [Export]
    internal class ProxyController : Controller
    {
        #region Members
        private readonly CompositionContainer container;
        private readonly ProxyService proxyService;
        #endregion

        [ImportingConstructor]
        public ProxyController(CompositionContainer container)
        {
            this.container = container;

            IEnumerable<ITracingSystemProxy> proxys = this.container.GetExportedValues<ITracingSystemProxy>();
            this.proxyService = new ProxyService(proxys);
        }

        #region Implement Controller base class
        protected override void OnInitialize()
        {
            foreach (var proxy in this.proxyService.Proxys)
            {
                proxy.Initialize();
            }

            if (this.ProxyService.Proxys.Any(x => x.ProxyName == Settings.Default.ActiveProxy))
            {
                this.proxyService.ActiveProxy = this.proxyService.Proxys.First(x => x.ProxyName == Settings.Default.ActiveProxy);
            }
            else if (this.ProxyService.Proxys.Any())
            {
                this.proxyService.ActiveProxy = this.proxyService.Proxys.First();
            }
            else
            {
                this.proxyService.ActiveProxy = null;
            }
        }

        public void Shutdown()
        { }
        #endregion

        #region Properties
        public ProxyService ProxyService { get { return this.proxyService; } }
        #endregion
    }
}
