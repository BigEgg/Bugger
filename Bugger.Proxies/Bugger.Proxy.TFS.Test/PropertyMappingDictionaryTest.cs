using BigEgg.Framework.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bugger.Proxy.TFS.Test
{
    [TestClass]
    public class MappingModelTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            AssertHelper.ExpectedException<ArgumentNullException>(() => new MappingModel(null));
            AssertHelper.ExpectedException<ArgumentNullException>(() => new MappingModel(""));
            AssertHelper.ExpectedException<ArgumentNullException>(() => new MappingModel("  "));
        }

        [TestMethod]
        public void GeneralSettingDocumentTest()
        {
            MappingModel mappingModel = new MappingModel("ID");
            Assert.AreEqual("ID", mappingModel.Key);
            Assert.AreEqual(string.Empty, mappingModel.Value);
        }

        [TestMethod]
        public void PropertiesWithNotification()
        {
            MappingModel mappingModel = new MappingModel("ID");

            AssertHelper.PropertyChangedEvent(mappingModel, x => x.Value, () => mappingModel.Value = "ID");
            Assert.AreEqual("ID", mappingModel.Value);

            mappingModel.Value = null;
            Assert.AreEqual("ID", mappingModel.Value);

            AssertHelper.PropertyChangedEvent(mappingModel, x => x.Value, () => mappingModel.Value = string.Empty);
            Assert.AreEqual(string.Empty, mappingModel.Value);
        }
    }
}
