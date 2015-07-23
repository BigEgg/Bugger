using BigEgg.Framework.Applications.Extensions.Applications.Services.FileDialogs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BigEgg.Framework.Applications.Extensions.Test.Applications.Services.FileDialogs
{
    [TestClass]
    public class OpenFileDialogServiceExtensionsTest
    {
        private readonly FileType rtfFileType = new FileType("RichText Document", ".rtf");
        private readonly FileType xpsFileType = new FileType("XPS Document", ".xps");
        private readonly string defaultFileName = "Document 1.rtf";
        private readonly object owner = new object();
        private readonly MockFileDialogService service = new MockFileDialogService();

        private readonly IEnumerable<FileType> fileTypes;
        private readonly FileDialogResult result;

        public OpenFileDialogServiceExtensionsTest()
        {
            fileTypes = new FileType[] { rtfFileType, xpsFileType };
            result = new FileDialogResult("Document 2.rtf", rtfFileType);

            service.Result = result;
        }

        [TestMethod]
        public void ShowOpenFileDialogExtensionTest()
        {
            Assert.AreEqual(result, service.ShowOpenFileDialog(rtfFileType));
            Assert.AreEqual(FileDialogType.OpenFileDialog, service.FileDialogType);
            Assert.AreEqual(rtfFileType, service.FileTypes.Single());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShowOpenFileDialogExtensionTest_ServiceNull()
        {
            OpenFileDialogServiceExtensions.ShowOpenFileDialog(null, rtfFileType);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShowOpenFileDialogExtensionTest_FileTypeNull()
        {
            service.ShowOpenFileDialog((FileType)null);
        }

        [TestMethod]
        public void ShowOpenFileDialogExtensionTest_WithOwner()
        {
            Assert.AreEqual(result, service.ShowOpenFileDialog(owner, rtfFileType));
            Assert.AreEqual(FileDialogType.OpenFileDialog, service.FileDialogType);
            Assert.AreEqual(owner, service.Owner);
            Assert.AreEqual(rtfFileType, service.FileTypes.Single());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShowOpenFileDialogExtensionTest_WithOwner_ServiceNull()
        {
            OpenFileDialogServiceExtensions.ShowOpenFileDialog(null, owner, rtfFileType);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShowOpenFileDialogExtensionTest_WithOwner_OwnerNull()
        {
            service.ShowOpenFileDialog(owner, (FileType)null);
        }

        [TestMethod]
        public void ShowOpenFileDialogExtensionTest_WithDefaultFileName()
        {
            Assert.AreEqual(result, service.ShowOpenFileDialog(rtfFileType, defaultFileName));
            Assert.AreEqual(FileDialogType.OpenFileDialog, service.FileDialogType);
            Assert.AreEqual(rtfFileType, service.FileTypes.Single());
            Assert.AreEqual(defaultFileName, service.DefaultFileName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShowOpenFileDialogExtensionTest_WithDefaultFileName_ServiceNull()
        {
            OpenFileDialogServiceExtensions.ShowOpenFileDialog(null, rtfFileType, defaultFileName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShowOpenFileDialogExtensionTest_WithDefaultFileName_FileTypeNull()
        {
            service.ShowOpenFileDialog(null, defaultFileName);
        }

        [TestMethod]
        public void ShowOpenFileDialogExtensionTest_WithOwner_WithDefaultFileName()
        {
            Assert.AreEqual(result, service.ShowOpenFileDialog(owner, rtfFileType, defaultFileName));
            Assert.AreEqual(FileDialogType.OpenFileDialog, service.FileDialogType);
            Assert.AreEqual(owner, service.Owner);
            Assert.AreEqual(rtfFileType, service.FileTypes.Single());
            Assert.AreEqual(defaultFileName, service.DefaultFileName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShowOpenFileDialogExtensionTest_WithOwner_WithDefaultFileName_ServiceNull()
        {
            OpenFileDialogServiceExtensions.ShowOpenFileDialog(null, owner, rtfFileType, defaultFileName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShowOpenFileDialogExtensionTest_WithOwner_WithDefaultFileName_FileTypeNull()
        {
            service.ShowOpenFileDialog(owner, null, defaultFileName);
        }

        [TestMethod]
        public void ShowOpenFileDialogExtensionTest_WithMultipleFileTypes()
        {
            Assert.AreEqual(result, service.ShowOpenFileDialog(fileTypes));
            Assert.AreEqual(FileDialogType.OpenFileDialog, service.FileDialogType);
            Assert.IsTrue(service.FileTypes.SequenceEqual(new FileType[] { rtfFileType, xpsFileType }));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShowOpenFileDialogExtensionTest_WithMultipleFileTypes_ServiceNull()
        {
            OpenFileDialogServiceExtensions.ShowOpenFileDialog(null, fileTypes);
        }

        [TestMethod]
        public void ShowOpenFileDialogExtensionTest_WithMultipleFileTypes_WithOwner()
        {
            Assert.AreEqual(result, service.ShowOpenFileDialog(owner, fileTypes));
            Assert.AreEqual(FileDialogType.OpenFileDialog, service.FileDialogType);
            Assert.AreEqual(owner, service.Owner);
            Assert.IsTrue(service.FileTypes.SequenceEqual(new FileType[] { rtfFileType, xpsFileType }));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShowOpenFileDialogExtensionTest_WithMultipleFileTypes_WithOwner_ServiceNull()
        {
            OpenFileDialogServiceExtensions.ShowOpenFileDialog(null, owner, fileTypes);
        }

        [TestMethod]
        public void ShowOpenFileDialogExtensionTest_WithMultipleFileTypes_WithOwner_WithDefaultFileName()
        {
            Assert.AreEqual(result, service.ShowOpenFileDialog(fileTypes, rtfFileType, defaultFileName));
            Assert.AreEqual(FileDialogType.OpenFileDialog, service.FileDialogType);
            Assert.IsTrue(service.FileTypes.SequenceEqual(new FileType[] { rtfFileType, xpsFileType }));
            Assert.AreEqual(rtfFileType, service.DefaultFileType);
            Assert.AreEqual(defaultFileName, service.DefaultFileName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShowOpenFileDialogExtensionTest_WithMultipleFileTypes_WithOwner_WithDefaultFileName_ServiceNull()
        {
            OpenFileDialogServiceExtensions.ShowOpenFileDialog(null, fileTypes, rtfFileType, defaultFileName);
        }
    }
}