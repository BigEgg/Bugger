using BigEgg.Framework.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Bugger.Proxy.Test.Models
{
    [TestClass]
    public class MappingModelTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            AssertHelper.ExpectedException<ArgumentNullException>(() => new Bugger.Proxy.Models.PropertyMappingDictionary.MappingModel(null));
            AssertHelper.ExpectedException<ArgumentNullException>(() => new Bugger.Proxy.Models.PropertyMappingDictionary.MappingModel(""));
            AssertHelper.ExpectedException<ArgumentNullException>(() => new Bugger.Proxy.Models.PropertyMappingDictionary.MappingModel("  "));
        }

        [TestMethod]
        public void GeneralSettingDocumentTest()
        {
            var mappingModel = new Bugger.Proxy.Models.PropertyMappingDictionary.MappingModel("ID");
            Assert.AreEqual("ID", mappingModel.Key);
            Assert.AreEqual(string.Empty, mappingModel.Value);
        }

        [TestMethod]
        public void PropertiesWithNotification()
        {
            var mappingModel = new Bugger.Proxy.Models.PropertyMappingDictionary.MappingModel("ID");

            AssertHelper.PropertyChangedEvent(mappingModel, x => x.Value, () => mappingModel.Value = "ID");
            Assert.AreEqual("ID", mappingModel.Value);

            mappingModel.Value = null;
            Assert.AreEqual("ID", mappingModel.Value);

            AssertHelper.PropertyChangedEvent(mappingModel, x => x.Value, () => mappingModel.Value = string.Empty);
            Assert.AreEqual(string.Empty, mappingModel.Value);
        }
    }
}
