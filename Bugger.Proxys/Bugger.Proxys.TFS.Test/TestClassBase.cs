using Bugger.Proxys.TFS.Test.Services;
using Bugger.Proxys.TFS.Test.Views;
using Bugger.Proxys.TFS.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace Bugger.Proxys.TFS.Test
{
    [TestClass]
    public abstract class TestClassBase
    {
        private readonly CompositionContainer container;

        
        protected TestClassBase()
        {
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new TypeCatalog(
                typeof(TFSSourceControlProxy),
                typeof(SettingViewModel)
            ));
            catalog.Catalogs.Add(new TypeCatalog(
                typeof(MockMessageService),
                typeof(MockSettingView), typeof(MockUriHelpView)
            ));
            container = new CompositionContainer(catalog);
            CompositionBatch batch = new CompositionBatch();
            batch.AddExportedValue(container);
            container.Compose(batch);

            ISourceControlProxy proxy = container.GetExportedValue<ISourceControlProxy>();
            proxy.Initialize();
        }

        
        protected CompositionContainer Container { get { return container; } }

        
        [TestInitialize]
        public void TestInitialize()
        {
            OnTestInitialize();
        }
        
        [TestCleanup]
        public void TestCleanup()
        {
            OnTestCleanup();
        }
        

        protected virtual void OnTestInitialize() { }
        
        protected virtual void OnTestCleanup() { }
    }
}
