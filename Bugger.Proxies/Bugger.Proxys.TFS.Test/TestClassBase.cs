using Bugger.Proxy.TFS.Documents;
using Bugger.Proxy.TFS.Presentation.Fake.Views;
using Bugger.Proxy.TFS.Test.Services;
using Bugger.Proxy.TFS.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;

namespace Bugger.Proxy.TFS.Test
{
    [TestClass]
    public abstract class TestClassBase
    {
        private readonly CompositionContainer container;
        //  Replace the [Password] with real password.
        protected const string password = "[Password]";


        protected TestClassBase()
        {
            if (File.Exists(SettingDocumentType.FilePath))
                File.Delete(SettingDocumentType.FilePath);

            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new TypeCatalog(
                typeof(TFSProxy), typeof(TFSHelper)
            ));
            catalog.Catalogs.Add(new TypeCatalog(
                typeof(MockMessageService),
                typeof(MockTFSSettingView), typeof(MockUriHelpView)
            ));
            container = new CompositionContainer(catalog);
            CompositionBatch batch = new CompositionBatch();
            batch.AddExportedValue(container);
            container.Compose(batch);

            ITracingSystemProxy proxy = container.GetExportedValue<ITracingSystemProxy>();
            proxy.Initialize();
        }


        protected CompositionContainer Container { get { return container; } }


        [TestInitialize]
        public void TestInitialize()
        {
            if (File.Exists(SettingDocumentType.FilePath))
                File.Delete(SettingDocumentType.FilePath);

            OnTestInitialize();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (File.Exists(SettingDocumentType.FilePath))
                File.Delete(SettingDocumentType.FilePath);

            OnTestCleanup();
        }


        protected virtual void OnTestInitialize() { }

        protected virtual void OnTestCleanup() { }
    }
}
