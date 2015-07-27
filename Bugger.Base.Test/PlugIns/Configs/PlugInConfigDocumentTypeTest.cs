using Bugger.Documents;
using Bugger.PlugIns;
using Bugger.PlugIns.Configs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bugger.Domain.Test.PlugIns.Configs
{
    [TestClass]
    public class PlugInConfigDocumentTypeTest : TestClassBase
    {
        private const string FILE_NAME = "Bugger.PlugIn.Click.TFSClick";
        private const string FILE_EXTENSION = ".xml";

        protected override void OnTestCleanup()
        {
            var fileName = FILE_NAME + FILE_EXTENSION;

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }


        [TestMethod]
        public void InjectionTest()
        {
            var documentType1 = Container.GetExportedValue<PlugInConfigDocumentType>();
            Assert.IsNotNull(documentType1);

            var documentType2 = Container.GetExportedValue<IDocumentType<PlugInConfigDocument>>();
            Assert.IsNotNull(documentType2);

            Assert.AreEqual(documentType1, documentType2);
        }

        [TestMethod]
        public void OpenAndSaveTest()
        {
            var fileName = FILE_NAME + FILE_EXTENSION;
            var documentType = Container.GetExportedValue<PlugInConfigDocumentType>();

            documentType.Save(fileName, NewDocument());
            Assert.IsTrue(File.Exists(fileName));

            var newDocument = documentType.Open(fileName);
            Assert.IsNotNull(newDocument);
            Assert.IsNotNull(newDocument.PlugInInfo);
            Assert.AreEqual("26e54ac9-6286-4991-a687-c8c6b7c50289", newDocument.PlugInInfo.PlugInGuid.ToString());
            Assert.AreEqual("TFS Click", newDocument.PlugInInfo.Name);
            Assert.AreEqual("When click the bug open related website", newDocument.PlugInInfo.Description);
            Assert.AreEqual(1, newDocument.PlugInInfo.Authors.Count);
            Assert.AreEqual("BigEgg", newDocument.PlugInInfo.Authors.First().Name);
            Assert.AreEqual("bigegg@bigegg.com", newDocument.PlugInInfo.Authors.First().EmailAddress);
            Assert.AreEqual("0.5.0.0", newDocument.PlugInInfo.Version.ToString());
            Assert.AreEqual("0.5.0.0", newDocument.PlugInInfo.MinimumSupportBuggerVersion.ToString());
            Assert.AreEqual("0.5.0.0", newDocument.PlugInInfo.MaximumSupportBuggerVersion.ToString());
            Assert.AreEqual(PlugInType.Click, newDocument.PlugInType);
            Assert.AreEqual("Bugger.PlugIn.Click.TFSClick", newDocument.AssemblyNames.First());
            Assert.AreEqual("1dc425b3-c27b-46ba-9623-a046d1acc754", newDocument.DependencyPlugIns.First().ToString());
        }

        [TestMethod]
        public void OpenAndSaveTest_WithoutFileExtension()
        {
            var fileName = FILE_NAME;
            var documentType = Container.GetExportedValue<PlugInConfigDocumentType>();

            documentType.Save(fileName, NewDocument());
            Assert.IsTrue(File.Exists(FILE_NAME + FILE_EXTENSION));

            var newDocument = documentType.Open(fileName);
            Assert.IsNotNull(newDocument);
            Assert.IsNotNull(newDocument.PlugInInfo);
            Assert.AreEqual("26e54ac9-6286-4991-a687-c8c6b7c50289", newDocument.PlugInInfo.PlugInGuid.ToString());
            Assert.AreEqual("TFS Click", newDocument.PlugInInfo.Name);
            Assert.AreEqual("When click the bug open related website", newDocument.PlugInInfo.Description);
            Assert.AreEqual(1, newDocument.PlugInInfo.Authors.Count);
            Assert.AreEqual("BigEgg", newDocument.PlugInInfo.Authors.First().Name);
            Assert.AreEqual("bigegg@bigegg.com", newDocument.PlugInInfo.Authors.First().EmailAddress);
            Assert.AreEqual("0.5.0.0", newDocument.PlugInInfo.Version.ToString());
            Assert.AreEqual("0.5.0.0", newDocument.PlugInInfo.MinimumSupportBuggerVersion.ToString());
            Assert.AreEqual("0.5.0.0", newDocument.PlugInInfo.MaximumSupportBuggerVersion.ToString());
            Assert.AreEqual(PlugInType.Click, newDocument.PlugInType);
            Assert.AreEqual("Bugger.PlugIn.Click.TFSClick", newDocument.AssemblyNames.First());
            Assert.AreEqual("1dc425b3-c27b-46ba-9623-a046d1acc754", newDocument.DependencyPlugIns.First().ToString());
        }


        private PlugInConfigDocument NewDocument()
        {
            var author = new PlugInAuthor()
            {
                Name = "BigEgg",
                EmailAddress = "bigegg@bigegg.com"
            };
            var info = new PlugInInfo()
            {
                PlugInGuid = new Guid("26e54ac9-6286-4991-a687-c8c6b7c50289"),
                Name = "TFS Click",
                Description = "When click the bug open related website",
                Authors = new List<PlugInAuthor>() { author },
                VersionStr = "0.5.0.0",
                MinimumSupportBuggerVersionStr = "0.5.0.0",
                MaximumSupportBuggerVersionStr = "0.5.0.0"
            };

            return new PlugInConfigDocument()
            {
                PlugInInfo = info,
                PlugInType = PlugInType.Click,
                AssemblyNames = new List<string>() { "Bugger.PlugIn.Click.TFSClick" },
                DependencyPlugIns = new List<Guid>() { new Guid("1dc425b3-c27b-46ba-9623-a046d1acc754") }
            };
        }
    }
}
