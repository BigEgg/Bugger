using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BigEgg.Framework.Applications.UnitTesting;
using BigEgg.Framework.Applications.Extensions.Applications;
using System.Xml.Serialization;

namespace BigEgg.Framework.Applications.Extensions.Test.Applications
{
    [TestClass]
    public class RecentFileTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorTest_Precondition()
        {
            new RecentFile(null);
        }

        [TestMethod]
        public void ConstructorTest()
        {
            var recentFile = new RecentFile("Doc1");
            Assert.AreEqual("Doc1", recentFile.Path);
            Assert.IsFalse(recentFile.IsPinned);
        }

        [TestMethod]
        public void PinnedTest() {
            var recentFile = new RecentFile("Doc1");

            AssertHelper.IsRaisePropertyChangedEvent(recentFile, x => x.IsPinned, () => recentFile.IsPinned = true);
            Assert.IsTrue(recentFile.IsPinned);
        }

        [TestMethod]
        public void GetSchemaTest()
        {
            var recentFile = new RecentFile("Doc1");

            IXmlSerializable serializable = recentFile;
            Assert.IsNull(serializable.GetSchema());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadXmlTest_Precondition()
        {
            var recentFile = new RecentFile("Doc1");

            IXmlSerializable serializable = recentFile;
            serializable.ReadXml(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteXmlTest_Precondition()
        {
            var recentFile = new RecentFile("Doc1");

            IXmlSerializable serializable = recentFile;
            serializable.WriteXml(null);
        }
    }
}
