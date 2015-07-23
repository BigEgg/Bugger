using BigEgg.Framework.Applications.Extensions.Applications.Services.FileDialog;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BigEgg.Framework.Applications.Extensions.Test.Applications.Services.FileDialog
{
    [TestClass]
    public class FileTypeTest
    {
        [TestMethod]
        public void GeneralTest()
        {
            var fileType = new FileType("Bitmap Image (*.bmp)", ".bmp");
            Assert.AreEqual("Bitmap Image (*.bmp)", fileType.Description);
            Assert.AreEqual(".bmp", fileType.FileExtension);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorTest_Precondition_Description_Null()
        {
            new FileType(null, ".bmp");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorTest_Precondition_Description_Empty()
        {
            new FileType("", ".bmp");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorTest_Precondition_Description_WhiteSpaces()
        {
            new FileType("    ", ".bmp");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorTest_Precondition_FileExtension_Null()
        {
            new FileType("Bitmap Image (*.bmp)", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorTest_Precondition_FileExtension_Empty()
        {
            new FileType("Bitmap Image (*.bmp)", "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorTest_Precondition_FileExtension_WhiteSpaces()
        {
            new FileType("Bitmap Image (*.bmp)", "    ");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorTest_Precondition_FileExtension_WithoutDot()
        {
            new FileType("Bitmap Image (*.bmp)", "bmp");
        }
    }
}
