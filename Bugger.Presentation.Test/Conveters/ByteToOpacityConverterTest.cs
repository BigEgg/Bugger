using System;
using Bugger.Presentation.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bugger.Presentation.Test.Conveters
{
    [TestClass]
    public class ByteToOpacityConverterTest
    {
        [TestMethod]
        public void ConvertTest()
        {
            ByteToOpacityConverter converter = ByteToOpacityConverter.Default;

            var result1 = converter.Convert(80, null, null, null);
            var result2 = converter.Convert(25, null, null, null);

            Assert.AreEqual(0.8, result1);
            Assert.AreEqual(0.25, result2);
        }
    }
}
