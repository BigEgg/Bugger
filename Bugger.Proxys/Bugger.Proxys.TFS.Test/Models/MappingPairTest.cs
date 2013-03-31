using BigEgg.Framework.UnitTesting;
using Bugger.Proxys.TFS.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Bugger.Proxys.TFS.Test.Models
{
    [TestClass]
    public class MappingPairTest
    {
        [TestMethod]
        public void GeneralMappingPairTest()
        {
            MappingPair pair = new MappingPair("ID");
            Assert.AreEqual("ID", pair.PropertyName);

            pair.FieldName = "ID";
            Assert.AreEqual("ID", pair.FieldName);
        }

        [TestMethod]
        public void ConstructorTest()
        {
            AssertHelper.ExpectedException<ArgumentException>(() => new MappingPair(null));
            AssertHelper.ExpectedException<ArgumentException>(() => new MappingPair(""));
            AssertHelper.ExpectedException<ArgumentException>(() => new MappingPair("  "));
        }

        [TestMethod]
        public void PropertiesWithNotification()
        {
            MappingPair pair = new MappingPair("ID");

            AssertHelper.PropertyChangedEvent(pair, x => x.FieldName, () => pair.FieldName = "ID");
            Assert.AreEqual("ID", pair.FieldName);
            AssertHelper.PropertyChangedEvent(pair, x => x.FieldName, () => pair.FieldName = "Status");
            Assert.AreEqual("Status", pair.FieldName);
        }
    }
}
