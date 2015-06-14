using BigEgg.Framework.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BigEgg.Framework.Test.Utils
{
    [TestClass]
    public class PathExtensionTest
    {
        [TestMethod]
        public void GetRelativePathTest()
        {
            string relativePath = @"D:\Windows\Web\Wallpaper\".GetRelativePath(@"D:\Windows\regedit.exe");
            Assert.AreEqual(@"..\..\regedit.exe", relativePath);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetRelativePath_fromPathNull()
        {
            PathExtension.GetRelativePath(null, @"D:\Windows\regedit.exe");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetRelativePath_toPathNull()
        {
            PathExtension.GetRelativePath(@"D:\Windows\Web\Wallpaper\", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetRelativePath_fromPathNotValid()
        {
            PathExtension.GetRelativePath(@"dows\Web\Wallpaper\", @"D:\Windows\regedit.exe");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetRelativePath_toPathNotValid()
        {
            PathExtension.GetRelativePath(@"D:\Windows\Web\Wallpaper\", @"ndows\regedit.exe");
        }
    }
}
