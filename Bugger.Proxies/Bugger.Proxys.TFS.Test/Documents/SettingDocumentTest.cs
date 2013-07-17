using BigEgg.Framework.UnitTesting;
using Bugger.Proxy.TFS.Documents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Bugger.Proxy.TFS.Test.Documents
{
    [TestClass]
    public class SettingDocumentTest
    {
        [TestMethod]
        public void GeneralSettingDocumentTest()
        {
            SettingDocument document = new SettingDocument();
            Assert.IsNotNull(document.PropertyMappingCollection);
            Assert.AreEqual(9, document.PropertyMappingCollection.Count);

            document.ConnectUri = new Uri("https://tfs.codeplex.com:443/tfs/TFS12");
            document.BugFilterField = "Work Item Type";
            document.BugFilterValue = "Bugs";
            document.UserName = "BigEgg";
            document.Password = "Password";
            document.PriorityRed = "1,2";

            Assert.AreEqual("https://tfs.codeplex.com/tfs/TFS12", document.ConnectUri.AbsoluteUri);
            Assert.AreEqual("Work Item Type", document.BugFilterField);
            Assert.AreEqual("Bugs", document.BugFilterValue);
            Assert.AreEqual("BigEgg", document.UserName);
            Assert.AreEqual("Password", document.Password);
            Assert.AreEqual("1,2", document.PriorityRed);
        }

        [TestMethod]
        public void PropertiesWithNotification()
        {
            SettingDocument document = new SettingDocument();

            AssertHelper.PropertyChangedEvent(document, x => x.PropertyMappingCollection, () => document.PropertyMappingCollection["ID"] = "ID");
            AssertHelper.PropertyChangedEvent(document, x => x.ConnectUri, () => document.ConnectUri = new Uri("https://tfs.codeplex.com:443/tfs/TFS12"));
            AssertHelper.PropertyChangedEvent(document, x => x.BugFilterField, () => document.BugFilterField = "Work Item Type");
            AssertHelper.PropertyChangedEvent(document, x => x.BugFilterValue, () => document.BugFilterValue = "Bugs");
            AssertHelper.PropertyChangedEvent(document, x => x.UserName, () => document.UserName = "BigEgg");
            AssertHelper.PropertyChangedEvent(document, x => x.Password, () => document.Password = "Password");
            AssertHelper.PropertyChangedEvent(document, x => x.PriorityRed, () => document.PriorityRed = "1,2");

            Assert.AreEqual("ID", document.PropertyMappingCollection["ID"]);
            Assert.AreEqual("https://tfs.codeplex.com/tfs/TFS12", document.ConnectUri.AbsoluteUri);
            Assert.AreEqual("Work Item Type", document.BugFilterField);
            Assert.AreEqual("Bugs", document.BugFilterValue);
            Assert.AreEqual("BigEgg", document.UserName);
            Assert.AreEqual("Password", document.Password);
            Assert.AreEqual("1,2", document.PriorityRed);
        }
    }
}
