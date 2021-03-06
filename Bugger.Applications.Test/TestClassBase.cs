﻿using Bugger.Applications.Controllers;
using Bugger.Applications.Services;
using Bugger.Applications.Test.Services;
using Bugger.Applications.Test.Views;
using Bugger.Applications.ViewModels;
using Bugger.Proxy.FakeProxy;
using Bugger.Proxy.TFS;
using Bugger.Proxy.TFS.Presentation.Fake.Views;
using Bugger.Proxy.TFS.ViewModels;
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
                typeof(ApplicationController), typeof(DataController), typeof(ProxyController),
                typeof(DataService), typeof(ShellService),
                typeof(FloatingViewModel), typeof(MainViewModel), typeof(UserBugsViewModel), typeof(TeamBugsViewModel)
            ));
            catalog.Catalogs.Add(new TypeCatalog(
                typeof(MockPresentationService), typeof(MockMessageService),
                typeof(MockFloatingView), typeof(MockMainView), typeof(MockSettingsView), typeof(MockUserBugsView), typeof(MockTeamBugsView),
                typeof(MockAboutDialogView), typeof(MockSettingDialogView)
            ));
            catalog.Catalogs.Add(new TypeCatalog(
                typeof(FakeProxy)
            ));
            catalog.Catalogs.Add(new TypeCatalog(
                typeof(TFSProxy), typeof(TFSHelper),
                typeof(MockTFSSettingView), typeof(MockUriHelpView)
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
