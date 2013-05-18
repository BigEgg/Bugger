using Bugger.Applications.Controllers;
using Bugger.Applications.Services;
using Bugger.Applications.Test.Services;
using Bugger.Applications.Test.Views;
using Bugger.Applications.ViewModels;
using Bugger.Proxys.FakeProxy;
using Bugger.Proxys.TFS;
using Bugger.Proxys.TFS.Presentation.Fake.Views;
using Bugger.Proxys.TFS.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace Bugger.Applications.Test
{
    [TestClass]
    public abstract class TestClassBase
    {
        private readonly CompositionContainer container;


        protected TestClassBase()
        {
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new TypeCatalog(
                typeof(ApplicationController), typeof(DataController),
                typeof(DataService),
                typeof(FloatingViewModel), typeof(MainViewModel)
            ));
            catalog.Catalogs.Add(new TypeCatalog(
                typeof(MockPresentationService), typeof(MockMessageService),
                typeof(MockFloatingView), typeof(MockMainView), typeof(MockSettingsView),
                typeof(MockAboutDialogView), typeof(MockSettingDialogView)
            ));
            catalog.Catalogs.Add(new TypeCatalog(
                typeof(FakeProxy), typeof(TFSSourceControlProxy),
                typeof(TFSSettingViewModel)
            ));
            catalog.Catalogs.Add(new TypeCatalog(
                typeof(MockTFSSettingView)
            ));
            container = new CompositionContainer(catalog);
            CompositionBatch batch = new CompositionBatch();
            batch.AddExportedValue(container);
            container.Compose(batch);
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
