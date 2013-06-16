using BigEgg.Framework.UnitTesting;
using Bugger.Proxy.TFS.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Bugger.Proxy.TFS.Test.Models
{
    [TestClass]
    public class TFSFieldTest
    {
        [TestMethod]
        public void GeneralTFSFieldTest()
        {
            TFSField field = new TFSField("Work Item Type");
            Assert.AreEqual("Work Item Type", field.Name);
            Assert.IsNotNull(field.AllowedValues);
            Assert.AreEqual(0, field.AllowedValues.Count);

            field.AllowedValues.Add("Work Item");
            field.AllowedValues.Add("Bug");
            Assert.AreEqual(2, field.AllowedValues.Count);
        }

        [TestMethod]
        public void ConstructorTest()
        {
            AssertHelper.ExpectedException<ArgumentException>(() => new TFSField(null));
            AssertHelper.ExpectedException<ArgumentException>(() => new TFSField(""));
            AssertHelper.ExpectedException<ArgumentException>(() => new TFSField("  "));
        }
    }
}
