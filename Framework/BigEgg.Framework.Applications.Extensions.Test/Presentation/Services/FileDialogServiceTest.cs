using BigEgg.Framework.Applications.Extensions.Applications.Services.FileDialogs;
using BigEgg.Framework.Applications.Extensions.Presentation.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace BigEgg.Framework.Applications.Extensions.Test.Presentation.Services
{
    [TestClass]
    public class FileDialogServiceTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShowOpenFileDialogTest_FileTypesNull()
        {
            FileDialogService service = new FileDialogService();
            service.ShowOpenFileDialog(null, null, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShowOpenFileDialogTest_FileTypesEmpty()
        {
            FileDialogService service = new FileDialogService();
            service.ShowOpenFileDialog(null, new FileType[] { }, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShowSaveFileDialogTest_FileTypesNull()
        {
            FileDialogService service = new FileDialogService();
            service.ShowSaveFileDialog(null, null, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShowSaveFileDialogTest_FileTypesEmpty()
        {
            FileDialogService service = new FileDialogService();
            service.ShowSaveFileDialog(null, new FileType[] { }, null, null);
        }

        [TestMethod]
        public void CreateFilterTest()
        {
            FileType rtfFileType = new FileType("RichText Document", ".rtf");
            FileType xpsFileType = new FileType("XPS Document", ".xps");

            Assert.AreEqual("RichText Document|*.rtf", InvokeCreateFilter(new FileType[] { rtfFileType }));
            Assert.AreEqual("RichText Document|*.rtf|XPS Document|*.xps",
                InvokeCreateFilter(new FileType[] { rtfFileType, xpsFileType }));
        }


        private static string InvokeCreateFilter(IEnumerable<FileType> fileTypes)
        {
            MethodInfo createFilterInfo = typeof(FileDialogService).GetMethod("CreateFilter",
                BindingFlags.Static | BindingFlags.NonPublic);
            return (string)createFilterInfo.Invoke(null, new object[] { fileTypes });
        }
    }
}
