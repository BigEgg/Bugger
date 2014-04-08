using BigEgg.Framework.UnitTesting;
using Bugger.Proxy.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Bugger.Proxy.Test.Models
{
    [TestClass]
    class PropertyMappingDictionaryTest
    {
        [TestMethod]
        public void AddTest_ParameterValidation()
        {
            var dictionary = new PropertyMappingDictionary();

            AssertHelper.ExpectedException<ArgumentNullException>(() => dictionary.Add(null, null));
            AssertHelper.ExpectedException<ArgumentNullException>(() => dictionary.Add("", null));
            AssertHelper.ExpectedException<ArgumentNullException>(() => dictionary.Add("   ", null));
            AssertHelper.ExpectedException<ArgumentNullException>(() => dictionary.Add("ID", null));
        }

        [TestMethod]
        public void AddTest()
        {
            var dictionary = new PropertyMappingDictionary();

            dictionary.Add("ID", "");
            Assert.AreEqual(1, dictionary.Count);
            AssertHelper.ExpectedException<ArgumentException>(() => dictionary.Add("ID", ""));
        }

        [TestMethod]
        public void ContainsKeyTest_ParameterValidation()
        {
            var dictionary = new PropertyMappingDictionary();

            AssertHelper.ExpectedException<ArgumentNullException>(() => dictionary.ContainsKey(null));
            AssertHelper.ExpectedException<ArgumentNullException>(() => dictionary.ContainsKey(""));
            AssertHelper.ExpectedException<ArgumentNullException>(() => dictionary.ContainsKey("   "));
        }

        [TestMethod]
        public void ContainsKeyTest()
        {
            var dictionary = new PropertyMappingDictionary();
            Assert.IsFalse(dictionary.ContainsKey("ID"));

            dictionary.Add("ID", "");
            Assert.IsTrue(dictionary.ContainsKey("ID"));
        }

        [TestMethod]
        public void RemoveTest_ParameterValidation()
        {
            var dictionary = new PropertyMappingDictionary();

            AssertHelper.ExpectedException<ArgumentNullException>(() => dictionary.Remove(null));
            AssertHelper.ExpectedException<ArgumentNullException>(() => dictionary.Remove(""));
            AssertHelper.ExpectedException<ArgumentNullException>(() => dictionary.Remove("   "));
        }

        [TestMethod]
        public void RemoveTest()
        {
            var dictionary = new PropertyMappingDictionary();
            Assert.IsFalse(dictionary.Remove("ID"));

            dictionary.Add("ID", "");
            Assert.IsTrue(dictionary.Remove("ID"));
            Assert.AreEqual(0, dictionary.Count);
        }

        [TestMethod]
        public void TryGetValueTest_ParameterValidation()
        {
            var dictionary = new PropertyMappingDictionary();
            string value = string.Empty;
            Assert.IsFalse(dictionary.TryGetValue("ID", out value));
            Assert.AreEqual(String.Empty, value);

            dictionary.Add("ID", "ID");
            Assert.IsTrue(dictionary.TryGetValue("ID", out value));
            Assert.AreEqual("ID", value);
        }
    }
}
