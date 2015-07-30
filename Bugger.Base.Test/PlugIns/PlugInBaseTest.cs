using Bugger.PlugIns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bugger.Domain.Test.PlugIns
{
    [TestClass]
    public class PlugInBaseTest
    {
        [TestMethod]
        public void GeneralTest()
        {
            var plugIn = new MockPlugIn(new Guid("1dc425b3-c27b-46ba-9623-a046d1acc754"), PlugInType.TrackingSystem);
            Assert.AreEqual("1dc425b3-c27b-46ba-9623-a046d1acc754", plugIn.Guid.ToString());
            Assert.AreEqual(PlugInType.TrackingSystem, plugIn.PlugInType);
            Assert.IsFalse(plugIn.IsInitialized);
            Assert.IsNotNull(plugIn.OpenSettingDialog());

            var sharedData = plugIn.GetSharedData();
            Assert.IsNotNull(sharedData);
            Assert.IsInstanceOfType(sharedData, typeof(EmptyPlugInSharedData));
            Assert.IsNotNull(plugIn.EnviromentSharedData);
            Assert.IsFalse(plugIn.EnviromentSharedData.Any());
        }

        [TestMethod]
        public void InitializeTest()
        {
            var plugIn = new MockPlugIn(new Guid("1dc425b3-c27b-46ba-9623-a046d1acc754"), PlugInType.TrackingSystem);
            var isInitializeCoreCalled = false;
            plugIn.InitializeCoreAction = () => isInitializeCoreCalled = true;
            plugIn.Initialize();

            Assert.IsTrue(isInitializeCoreCalled);
            Assert.IsTrue(plugIn.IsInitialized);

            isInitializeCoreCalled = false;
            plugIn.Initialize();

            Assert.IsFalse(isInitializeCoreCalled);
            Assert.IsTrue(plugIn.IsInitialized);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void InitializeTest_Exception()
        {
            var plugIn = new MockPlugIn(new Guid("1dc425b3-c27b-46ba-9623-a046d1acc754"), PlugInType.TrackingSystem);
            plugIn.InitializeCoreAction = () => { throw new NotSupportedException(); };
            plugIn.Initialize();
        }

        [TestMethod]
        public void SetSharedDataTest()
        {
            var plugIn = new MockPlugIn(new Guid("1dc425b3-c27b-46ba-9623-a046d1acc754"), PlugInType.TrackingSystem);
            var guid = new Guid("26e54ac9-6286-4991-a687-c8c6b7c50289");
            plugIn.SetSharedData(new List<IPlugInSharedData> { new EmptyPlugInSharedData(guid) });

            Assert.AreEqual(1, plugIn.EnviromentSharedData.Count);
            Assert.IsNotNull(plugIn.EnviromentSharedData[guid]);
        }
    }
}
