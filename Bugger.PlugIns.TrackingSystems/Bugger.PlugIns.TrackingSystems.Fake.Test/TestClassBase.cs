using Bugger.PlugIns.TrackingSystems.Fake.Services;
using Bugger.PlugIns.TrackingSystems.Fake.Test.Views;
using Bugger.PlugIns.TrackingSystems.Fake.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace Bugger.PlugIns.TrackingSystems.Fake.Test
{
    [TestClass]
    public class TestClassBase
    {
        private readonly CompositionContainer container;


        public TestClassBase()
        {
            AggregateCatalog catelog = new AggregateCatalog();
            catelog.Catalogs.Add(new TypeCatalog(
                typeof(DataService)
            ));
            catelog.Catalogs.Add(new TypeCatalog(
                typeof(SettingViewModel)
            ));
            catelog.Catalogs.Add(new TypeCatalog(
                typeof(MockSettingView)
            ));
            container = new CompositionContainer(catelog);
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
