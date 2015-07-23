using BigEgg.Framework.Applications.Extensions.Applications.Services.FileDialogs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BigEgg.Framework.Applications.Extensions.Test.Applications.Services.FileDialogs
{
    [TestClass]
    public class FileDialogResultTest
    {
        [TestMethod]
        public void GeneralTest()
        {
            var fileType = new FileType("Bitmap Image (*.bmp)", ".bmp");
            var result = new FileDialogResult(@"C:\image.bmp", fileType);

            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(@"C:\image.bmp", result.FileName);
            Assert.AreEqual(fileType, result.SelectedFileType);
            Assert.AreEqual(@"C:\image.bmp", result.GetFullFileName());
        }

        [TestMethod]
        public void GetFullFileNameTest()
        {
            var fileType = new FileType("Bitmap Image (*.bmp)", ".bmp");

            var result = new FileDialogResult(@"C:\image.bmp", fileType);
            Assert.AreEqual(@"C:\image.bmp", result.GetFullFileName());

            result = new FileDialogResult(@"C:\image", fileType);
            Assert.AreEqual(@"C:\image.bmp", result.GetFullFileName());

            result = new FileDialogResult(@"C:\image.txt", fileType);
            Assert.AreEqual(@"C:\image.txt.bmp", result.GetFullFileName());

            result = new FileDialogResult(@"C:\image.txt", fileType);
            Assert.AreEqual(@"C:\image.txt.bmp", result.GetFullFileName());
        }

        [TestMethod]
        public void CancelResultTest()
        {
            var result = FileDialogResult.CancelResult();

            Assert.IsFalse(result.IsValid);
            Assert.IsNull(result.FileName);
            Assert.IsNull(result.GetFullFileName());
        }
    }
}
