using Bugger.Base.Models;
using Bugger.Proxy.TFS.Documents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel;

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

            PropertyDescriptorCollection propertyDescriptorCollection = TypeDescriptor.GetProperties(typeof(Bug));
            Assert.AreEqual(propertyDescriptorCollection.Count - 1, document.PropertyMappingCollection.Count);

            document.PropertyMappingCollection["ID"] = "ID";
            document.ConnectUri = new Uri("https://tfs.codeplex.com:443/tfs/TFS12");
            document.BugFilterField = "Work Item Type";
            document.BugFilterValue = "Bugs";
            document.UserName = "BigEgg";
            document.Password = "Password";
            document.PriorityRed = "1;2";

            Assert.AreEqual("ID", document.PropertyMappingCollection["ID"]);
            Assert.AreEqual(new Uri("https://tfs.codeplex.com:443/tfs/TFS12").AbsoluteUri, document.ConnectUri.AbsoluteUri);
            Assert.AreEqual("Work Item Type", document.BugFilterField);
            Assert.AreEqual("Bugs", document.BugFilterValue);
            Assert.AreEqual("BigEgg", document.UserName);
            Assert.AreEqual("Password", document.Password);
            Assert.AreEqual("1;2", document.PriorityRed);
        }
    }
}
