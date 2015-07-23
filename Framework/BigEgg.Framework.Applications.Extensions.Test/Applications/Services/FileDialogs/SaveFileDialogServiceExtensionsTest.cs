using BigEgg.Framework.Applications.Extensions.Applications.Services.FileDialogs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BigEgg.Framework.Applications.Extensions.Test.Applications.Services.FileDialogs
{
    [TestClass]
    public class SaveFileDialogServiceExtensionsTest
    {
        private readonly FileType rtfFileType = new FileType("RichText Document", ".rtf");
        private readonly FileType xpsFileType = new FileType("XPS Document", ".xps");
        private readonly string defaultFileName = "Document 1.rtf";
        private readonly object owner = new object();
        private readonly IEnumerable<FileType> fileTypes;
        private readonly FileDialogResult result;

        public SaveFileDialogServiceExtensionsTest()
        {
            fileTypes = new FileType[] { rtfFileType, xpsFileType };
            result = new FileDialogResult("Document 2.rtf", rtfFileType);
        }

        [TestMethod]
        public void ShowSaveFileDialogExtensionTest()
        {
            var service = new MockFileDialogService();
            service.Result = result;

            Assert.AreEqual(result, service.ShowSaveFileDialog(rtfFileType));
            Assert.AreEqual(FileDialogType.SaveFileDialog, service.FileDialogType);
            Assert.AreEqual(rtfFileType, service.FileTypes.Single());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShowSaveFileDialogExtensionTest_ServiceNull()
        {
            SaveFileDialogServiceExtensions.ShowSaveFileDialog(null, rtfFileType);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShowSaveFileDialogExtensionTest_FileTypeNull()
        {
            var service = new MockFileDialogService();
            service.Result = result;

            service.ShowSaveFileDialog((FileType)null);
        }

        [TestMethod]
        public void ShowSaveFileDialogExtensionTest_WithOwner()
        {
            var service = new MockFileDialogService();
            service.Result = result;

            Assert.AreEqual(result, service.ShowSaveFileDialog(owner, rtfFileType));
            Assert.AreEqual(FileDialogType.SaveFileDialog, service.FileDialogType);
            Assert.AreEqual(owner, service.Owner);
            Assert.AreEqual(rtfFileType, service.FileTypes.Single());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShowSaveFileDialogExtensionTest_WithOwner_ServiceNull()
        {
            SaveFileDialogServiceExtensions.ShowSaveFileDialog(null, owner, rtfFileType);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShowSaveFileDialogExtensionTest_WithOwner_OwnerNull()
        {
            var service = new MockFileDialogService();
            service.Result = result;

            service.ShowSaveFileDialog(owner, (FileType)null);
        }

        [TestMethod]
        public void ShowSaveFileDialogExtensionTest_WithDefaultFileName()
        {
            var service = new MockFileDialogService();
            service.Result = result;

            Assert.AreEqual(result, service.ShowSaveFileDialog(rtfFileType, defaultFileName));
            Assert.AreEqual(FileDialogType.SaveFileDialog, service.FileDialogType);
            Assert.AreEqual(rtfFileType, service.FileTypes.Single());
            Assert.AreEqual(defaultFileName, service.DefaultFileName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShowSaveFileDialogExtensionTest_WithDefaultFileName_ServiceNull()
        {
            SaveFileDialogServiceExtensions.ShowSaveFileDialog(null, rtfFileType, defaultFileName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShowSaveFileDialogExtensionTest_WithDefaultFileName_FileTypeNull()
        {
            var service = new MockFileDialogService();
            service.Result = result;

            service.ShowSaveFileDialog(null, defaultFileName);
        }

        [TestMethod]
        public void ShowSaveFileDialogExtensionTest_WithOwner_WithDefaultFileName()
        {
            var service = new MockFileDialogService();
            service.Result = result;

            Assert.AreEqual(result, service.ShowSaveFileDialog(owner, rtfFileType, defaultFileName));
            Assert.AreEqual(FileDialogType.SaveFileDialog, service.FileDialogType);
            Assert.AreEqual(owner, service.Owner);
            Assert.AreEqual(rtfFileType, service.FileTypes.Single());
            Assert.AreEqual(defaultFileName, service.DefaultFileName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShowSaveFileDialogExtensionTest_WithOwner_WithDefaultFileName_ServiceNull()
        {
            SaveFileDialogServiceExtensions.ShowSaveFileDialog(null, owner, rtfFileType, defaultFileName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShowSaveFileDialogExtensionTest_WithOwner_WithDefaultFileName_FileTypeNull()
        {
            var service = new MockFileDialogService();
            service.Result = result;

            service.ShowSaveFileDialog(owner, null, defaultFileName);
        }

        [TestMethod]
        public void ShowSaveFileDialogExtensionTest_WithMultipleFileTypes()
        {
            var service = new MockFileDialogService();
            service.Result = result;

            Assert.AreEqual(result, service.ShowSaveFileDialog(fileTypes));
            Assert.AreEqual(FileDialogType.SaveFileDialog, service.FileDialogType);
            Assert.IsTrue(service.FileTypes.SequenceEqual(new FileType[] { rtfFileType, xpsFileType }));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShowSaveFileDialogExtensionTest_WithMultipleFileTypes_ServiceNull()
        {
            SaveFileDialogServiceExtensions.ShowSaveFileDialog(null, fileTypes);
        }

        [TestMethod]
        public void ShowSaveFileDialogExtensionTest_WithMultipleFileTypes_WithOwner()
        {
            var service = new MockFileDialogService();
            service.Result = result;

            Assert.AreEqual(result, service.ShowSaveFileDialog(owner, fileTypes));
            Assert.AreEqual(FileDialogType.SaveFileDialog, service.FileDialogType);
            Assert.AreEqual(owner, service.Owner);
            Assert.IsTrue(service.FileTypes.SequenceEqual(new FileType[] { rtfFileType, xpsFileType }));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShowSaveFileDialogExtensionTest_WithMultipleFileTypes_WithOwner_ServiceNull()
        {
            SaveFileDialogServiceExtensions.ShowSaveFileDialog(null, owner, fileTypes);
        }

        [TestMethod]
        public void ShowSaveFileDialogExtensionTest_WithMultipleFileTypes_WithOwner_WithDefaultFileName()
        {
            var service = new MockFileDialogService();
            service.Result = result;

            Assert.AreEqual(result, service.ShowSaveFileDialog(fileTypes, rtfFileType, defaultFileName));
            Assert.AreEqual(FileDialogType.SaveFileDialog, service.FileDialogType);
            Assert.IsTrue(service.FileTypes.SequenceEqual(new FileType[] { rtfFileType, xpsFileType }));
            Assert.AreEqual(rtfFileType, service.DefaultFileType);
            Assert.AreEqual(defaultFileName, service.DefaultFileName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShowSaveFileDialogExtensionTest_WithMultipleFileTypes_WithOwner_WithDefaultFileName_ServiceNull()
        {
            SaveFileDialogServiceExtensions.ShowSaveFileDialog(null, fileTypes, rtfFileType, defaultFileName);
        }
    }
}