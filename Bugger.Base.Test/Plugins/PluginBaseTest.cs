using BigEgg.Framework.UnitTesting;
using Bugger.Plugins;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Bugger.Test.Plugins
{
    [TestClass]
    public class PluginBaseTest
    {
        [TestMethod]
        public void ConstructorExceptionTest()
        {
            AssertHelper.ExpectedException<ArgumentNullException>(() => new MockPlugin(null, null, null, PluginCategory.Proxy, null, null));
            AssertHelper.ExpectedException<ArgumentNullException>(() => new MockPlugin("", null, null, PluginCategory.Proxy, null, null));
            AssertHelper.ExpectedException<ArgumentNullException>(() => new MockPlugin("uniqueName", null, null, PluginCategory.Proxy, null, null));
            AssertHelper.ExpectedException<ArgumentNullException>(() => new MockPlugin("uniqueName", "", null, PluginCategory.Proxy, null, null));
            AssertHelper.ExpectedException<ArgumentNullException>(() => new MockPlugin("uniqueName", "pluginName", "", PluginCategory.Proxy, null, null));
            AssertHelper.ExpectedException<ArgumentNullException>(() => new MockPlugin("uniqueName", "pluginName", "", PluginCategory.Proxy, new Version("1.0"), null));

            AssertHelper.ExpectedException<ArgumentException>(() => new MockPlugin("uniqueName", "pluginName", "", PluginCategory.Proxy, new Version("1.0"), new Version("0.1")));
        }

        [TestMethod]
        public void ConstructorTest()
        {
            var plugin = new MockPlugin("uniqueName", "pluginName", "description", PluginCategory.Proxy, new Version("1.0"), new Version("1.0"));
            Assert.AreEqual("uniqueName", plugin.UniqueName);
            Assert.AreEqual("pluginName", plugin.PluginName);
            Assert.AreEqual("description", plugin.Description);
            Assert.AreEqual(PluginCategory.Proxy, plugin.Category);
            Assert.AreEqual(new Version("1.0"), plugin.MinimumApplicationVersion);
            Assert.AreEqual(new Version("1.0"), plugin.MaximumApplicationVersion);
        }


        private class MockPlugin : PluginBase
        {
            public MockPlugin(string uniqueName,
                              string pluginName,
                              string description,
                              PluginCategory category,
                              Version minimumApplicationVersion,
                              Version maximumApplicationVersion)
                : base(uniqueName, pluginName, description, category, minimumApplicationVersion, maximumApplicationVersion)
            {
            }


            #region Properties
            public Action OnInitializeAction { get; set; }
            #endregion

            #region Methods
            protected override void OnInitialize()
            {
                if (OnInitializeAction != null)
                {
                    OnInitializeAction.Invoke();
                }
            }
            #endregion
        }
    }
}
