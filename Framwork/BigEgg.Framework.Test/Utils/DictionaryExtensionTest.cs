using BigEgg.Framework.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace BigEgg.Framework.Test.Utils
{
    [TestClass]
    public class DictionaryExtensionTest
    {
        [TestMethod]
        public void AddOrUpdateTest_CommonValue_NotExist()
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>();

            Assert.AreEqual(0, dictionary.Count);
            dictionary.AddOrUpdate("key", "value");
            Assert.AreEqual(1, dictionary.Count);
            Assert.IsTrue(dictionary.ContainsKey("key"));
            Assert.AreEqual("value", dictionary["key"]);
        }

        [TestMethod]
        public void AddOrUpdateTest_CommonValue_Exist()
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.AddOrUpdate("key", "value");

            Assert.AreEqual(1, dictionary.Count);
            dictionary.AddOrUpdate("key", "newValue");
            Assert.AreEqual(1, dictionary.Count);
            Assert.IsTrue(dictionary.ContainsKey("key"));
            Assert.AreEqual("newValue", dictionary["key"]);
        }

        [TestMethod]
        public void AddOrUpdateTest_ListValue_NotExist()
        {
            IDictionary<string, IList<string>> dictionary = new Dictionary<string, IList<string>>();

            Assert.AreEqual(0, dictionary.Count);
            dictionary.AddOrUpdate("key", "value");
            Assert.AreEqual(1, dictionary.Count);
            Assert.IsTrue(dictionary.ContainsKey("key"));
            Assert.IsNotNull(dictionary["key"]);
            Assert.AreEqual(1, dictionary["key"].Count);
            Assert.AreEqual("value", dictionary["key"].Single());
        }

        [TestMethod]
        public void AddOrUpdateTest_ListValue_Exist()
        {
            IDictionary<string, IList<string>> dictionary = new Dictionary<string, IList<string>>();
            dictionary.AddOrUpdate("key", "value");

            Assert.AreEqual(1, dictionary.Count);
            dictionary.AddOrUpdate("key", "newValue");
            Assert.AreEqual(1, dictionary.Count);
            Assert.IsTrue(dictionary.ContainsKey("key"));
            Assert.IsNotNull(dictionary["key"]);
            Assert.AreEqual(2, dictionary["key"].Count);
            Assert.AreEqual("value", dictionary["key"].First());
            Assert.AreEqual("newValue", dictionary["key"].Last());
        }
    }
}
