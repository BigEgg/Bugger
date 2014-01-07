using Bugger.Proxy.TFS.Documents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Bugger.Proxy.TFS.Test.Documents
{
    [TestClass]
    public class SettingDocumentTypeTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            if (File.Exists(SettingDocumentType.FilePath))
            {
                File.Delete(SettingDocumentType.FilePath);
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (File.Exists(SettingDocumentType.FilePath))
            {
                File.Delete(SettingDocumentType.FilePath);
            }
        }


        [TestMethod]
        public void GeneralSettingDocumentTypeTest()
        {
            Assert.IsTrue(Path.IsPathRooted(SettingDocumentType.FilePath));
            Assert.IsFalse(File.Exists(SettingDocumentType.FilePath));
        }

        [TestMethod]
        public void NewTest()
        {
            SettingDocument document = SettingDocumentType.New();

            Assert.IsNotNull(document);
            Assert.IsFalse(File.Exists(SettingDocumentType.FilePath));
        }

        [TestMethod]
        public void SaveAndOpenTest()
        {
            SettingDocument document = SettingDocumentType.New();

            document.ConnectUri = new Uri("https://tfs.codeplex.com:443/tfs/TFS12");
            document.BugFilterField = "Work Item Type";
            document.BugFilterValue = "Bugs";
            document.UserName = "BigEgg";
            document.Password = "Password";
            document.PriorityRed = "1;2";
            document.PropertyMappingCollection["ID"] = "ID";

            SettingDocumentType.Save(document);
            Assert.IsTrue(File.Exists(SettingDocumentType.FilePath));

            SettingDocument openDocument = SettingDocumentType.Open();
            Assert.AreEqual(document.ConnectUri.AbsoluteUri, openDocument.ConnectUri.AbsoluteUri);
            Assert.AreEqual(document.BugFilterField, openDocument.BugFilterField);
            Assert.AreEqual(document.BugFilterValue, openDocument.BugFilterValue);
            Assert.AreEqual(document.UserName = "BigEgg", openDocument.UserName);
            Assert.AreEqual(document.Password = "Password", openDocument.Password);
            Assert.AreEqual(document.PriorityRed = "1;2", openDocument.PriorityRed);
            Assert.AreEqual(document.PropertyMappingCollection["ID"], openDocument.PropertyMappingCollection["ID"]);
        }
    }
}
