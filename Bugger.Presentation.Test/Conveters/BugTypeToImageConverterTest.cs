using Bugger.Base.Models;
using Bugger.Presentation.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;


namespace Bugger.Presentation.Test.Conveters
{
    [TestClass]
    public class BugTypeToImageConverterTest
    {
        [TestMethod]
        public void ConvertTest()
        {
            if (!UriParser.IsKnownScheme("pack")) new System.Windows.Application();

            BugTypeToImageConverter converter = BugTypeToImageConverter.Default;

            var redImage = converter.Convert(BugType.Red, null, null, null);
            var yellowImage = converter.Convert(BugType.Yellow, null, null, null);

            Assert.IsNotNull(redImage);
            Assert.IsNotNull(yellowImage);
            Assert.IsTrue(redImage.GetType() == yellowImage.GetType());

            Assert.AreNotEqual(redImage, yellowImage);
        }
    }
}
