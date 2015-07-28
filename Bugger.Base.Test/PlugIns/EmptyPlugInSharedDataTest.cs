using Bugger.PlugIns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Bugger.Base.Test.PlugIns
{
    [TestClass]
    public class EmptyPlugInSharedDataTest
    {
        [TestMethod]
        public void GeneralTest()
        {
            var sharedData = new EmptyPlugInSharedData(new Guid("1dc425b3-c27b-46ba-9623-a046d1acc754"));

            Assert.AreEqual("1dc425b3-c27b-46ba-9623-a046d1acc754", sharedData.PlugInGuid.ToString());
        }
    }
}
