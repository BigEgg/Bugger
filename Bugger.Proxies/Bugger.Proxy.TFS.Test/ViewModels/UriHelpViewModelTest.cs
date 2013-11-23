using BigEgg.Framework.Foundation;
using BigEgg.Framework.UnitTesting;
using Bugger.Proxy.TFS.Properties;
using Bugger.Proxy.TFS.ViewModels;
using Bugger.Proxy.TFS.Views;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bugger.Proxy.TFS.Test.ViewModels
{
    [TestClass]
    public class UriHelpViewModelTest : TestClassBase
    {
        private UriHelpViewModel viewModel;

        protected override void OnTestInitialize()
        {
            IUriHelpView view = Container.GetExportedValue<IUriHelpView>();
            this.viewModel = new UriHelpViewModel(view);
        }

        [TestMethod]
        public void GeneralUriHelpViewModelTest()
        {
            Assert.IsNotNull(viewModel.SubmitCommand);
            Assert.IsNotNull(viewModel.CancelCommand);
            Assert.IsTrue(viewModel.CanEditConnectionDetail);
            Assert.AreEqual("tfs", viewModel.Path);
            Assert.AreEqual((uint)8080, viewModel.Port);
            Assert.AreEqual(string.Empty, viewModel.ServerName);
            Assert.AreEqual(Resources.InvalidUrl, viewModel.UriPreview);
            Assert.IsFalse(viewModel.IsHttpsProtocal);
            Assert.AreEqual(Resources.UriHelpDialogTitle, viewModel.Title);
        }

        [TestMethod]
        public void UriHelpViewModelCloseTest()
        {
            Assert.IsFalse(viewModel.SubmitCommand.CanExecute(null));

            viewModel.ServerName = "https://tfs.codeplex.com:443/tfs/TFS12";
            Assert.IsTrue(viewModel.SubmitCommand.CanExecute(null));

            viewModel.ServerName = "!serverName!";
            Assert.IsFalse(viewModel.SubmitCommand.CanExecute(null));
        }

        [TestMethod]
        public void ServerNamePropertyChangedTest()
        {
            AssertHelper.PropertyChangedEvent(viewModel, x => x.ServerName, () => viewModel.ServerName = "SomeServer");
            Assert.AreEqual("SomeServer", viewModel.ServerName);
            AssertHelper.PropertyChangedEvent(viewModel, x => x.CanEditConnectionDetail, () => viewModel.ServerName = "ServerName");
            Assert.AreEqual("ServerName", viewModel.ServerName);
            AssertHelper.PropertyChangedEvent(viewModel, x => x.UriPreview, () => viewModel.ServerName = "SomeServer");
            Assert.AreEqual("SomeServer", viewModel.ServerName);
            Assert.IsTrue(viewModel.CanEditConnectionDetail);
            Assert.AreEqual("http://someserver:8080/tfs", viewModel.UriPreview);

            viewModel.ServerName = "http://someserver:8080/tfs";
            Assert.IsFalse(viewModel.CanEditConnectionDetail);
            Assert.AreEqual("http://someserver:8080/tfs", viewModel.UriPreview);

            viewModel.ServerName = "!ServerName!";
            Assert.IsTrue(viewModel.CanEditConnectionDetail);
            Assert.AreEqual(Resources.InvalidUrl, viewModel.UriPreview);

            viewModel.ServerName = null;
            Assert.IsTrue(viewModel.CanEditConnectionDetail);
            Assert.AreEqual(Resources.InvalidUrl, viewModel.UriPreview);
        }

        [TestMethod]
        public void PathPropertyChangedTest()
        {
            AssertHelper.PropertyChangedEvent(viewModel, x => x.Path, () => viewModel.Path = "path");
            Assert.AreEqual("path", viewModel.Path);
            Assert.AreEqual(Resources.InvalidUrl, viewModel.UriPreview);

            viewModel.ServerName = "SomeServer";
            Assert.AreEqual("http://someserver:8080/path", viewModel.UriPreview);
            AssertHelper.PropertyChangedEvent(viewModel, x => x.UriPreview, () => viewModel.Path = "tfs");
            Assert.AreEqual("tfs", viewModel.Path);
            Assert.AreEqual("http://someserver:8080/tfs", viewModel.UriPreview);
        }

        [TestMethod]
        public void PortPropertyChangedTest()
        {
            AssertHelper.PropertyChangedEvent(viewModel, x => x.Port, () => viewModel.Port = 123);
            Assert.AreEqual((uint)123, viewModel.Port);
            Assert.AreEqual(Resources.InvalidUrl, viewModel.UriPreview);

            viewModel.ServerName = "SomeServer";
            Assert.AreEqual("http://someserver:123/tfs", viewModel.UriPreview);
            AssertHelper.PropertyChangedEvent(viewModel, x => x.UriPreview, () => viewModel.Port = 234);
            Assert.AreEqual((uint)234, viewModel.Port);
            Assert.AreEqual("http://someserver:234/tfs", viewModel.UriPreview);

            viewModel.Port = (uint)0;
            Assert.AreEqual(Resources.InvalidUrl, viewModel.UriPreview);

            viewModel.Port = (uint)-0;
            Assert.AreEqual(Resources.InvalidUrl, viewModel.UriPreview);

            Assert.IsFalse(viewModel.IsHttpsProtocal);
            viewModel.Port = (uint)8080;
            viewModel.IsHttpsProtocal = true;
            Assert.AreEqual((uint)443, viewModel.Port);
            viewModel.IsHttpsProtocal = false;
            Assert.AreEqual((uint)8080, viewModel.Port);
        }

        [TestMethod]
        public void IsHttpsProtocalPropertyChangedTes()
        {
            AssertHelper.PropertyChangedEvent(viewModel, x => x.IsHttpsProtocal, () => viewModel.IsHttpsProtocal = true);
            Assert.IsTrue(viewModel.IsHttpsProtocal);
            Assert.AreEqual(Resources.InvalidUrl, viewModel.UriPreview);
            Assert.AreEqual((uint)443, viewModel.Port);

            viewModel.ServerName = "SomeServer";
            Assert.AreEqual("https://someserver/tfs", viewModel.UriPreview);
            AssertHelper.PropertyChangedEvent(viewModel, x => x.UriPreview, () => viewModel.IsHttpsProtocal = false);
            Assert.IsFalse(viewModel.IsHttpsProtocal);
            Assert.AreEqual("http://someserver:8080/tfs", viewModel.UriPreview);

            viewModel.IsHttpsProtocal = true;
            viewModel.Port = (uint)123;
            Assert.AreEqual("https://someserver:123/tfs", viewModel.UriPreview);

            viewModel.IsHttpsProtocal = false;
            Assert.AreEqual("http://someserver:123/tfs", viewModel.UriPreview);
        }

        [TestMethod]
        public void ServerNameValidationTest()
        {
            Assert.AreEqual(string.Empty, viewModel.ServerName);
            Assert.AreEqual(Resources.ServerNameMandatory, viewModel.Validate("ServerName"));

            viewModel.ServerName = "ServerName";
            Assert.AreEqual("ServerName", viewModel.ServerName);
            Assert.AreEqual(string.Empty, viewModel.Validate("ServerName"));

            viewModel.ServerName = null;
            Assert.IsNull(viewModel.ServerName);
            Assert.AreEqual(Resources.ServerNameMandatory, viewModel.Validate("ServerName"));
        }

        [TestMethod]
        public void PathValidationTest()
        {
            Assert.AreEqual("tfs", viewModel.Path);
            Assert.AreEqual(string.Empty, viewModel.Validate("Path"));

            viewModel.Path = null;
            Assert.IsNull(viewModel.Path);
            Assert.AreEqual(Resources.PathMandatory, viewModel.Validate("Path"));

            viewModel.Path = "";
            Assert.AreEqual("", viewModel.Path);
            Assert.AreEqual(Resources.PathMandatory, viewModel.Validate("Path"));

            viewModel.ServerName = "http://someserver:8080/tfs";
            Assert.AreEqual(string.Empty, viewModel.Validate("Path"));

            viewModel.Path = null;
            Assert.AreEqual(string.Empty, viewModel.Validate("Path"));
        }

        [TestMethod]
        public void PortValidationTest()
        {
            Assert.AreEqual((uint)8080, viewModel.Port);
            Assert.AreEqual(string.Empty, viewModel.Validate("Port"));

            viewModel.Port = (uint)0;
            Assert.AreEqual(
                string.Format(Resources.PortRange, "", 1, 65535),
                viewModel.Validate("Port"));

            viewModel.Port = (uint)-0;
            Assert.AreEqual(
                string.Format(Resources.PortRange, "", 1, 65535),
                viewModel.Validate("Port"));

            viewModel.Port = (uint)65536;
            Assert.AreEqual(
                string.Format(Resources.PortRange, "", 1, 65535),
                viewModel.Validate("Port"));

            viewModel.Port = (uint)65535;
            Assert.AreEqual(string.Empty, viewModel.Validate("Port"));
        }
    }
}
