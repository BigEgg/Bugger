using BigEgg.Framework.Applications.Presentation.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BigEgg.Framework.Applications.Test.Presentation.Converters
{
    [TestClass]
    public class StringFormatConverterTest
    {
        [TestMethod]
        public void DefaultTest()
        {
            var converter = StringFormatConverter.Default;
            Assert.AreEqual(converter, StringFormatConverter.Default);
        }

        [TestMethod]
        public void ConverteTest()
        {
            var username = "John Reese";
            var format = "Name: {0}";
            var converter = StringFormatConverter.Default;

            Assert.AreEqual(string.Format(null, format, username), converter.Convert(username, null, format, null));
            Assert.AreEqual(string.Format(null, "{0}", username), converter.Convert(username, null, null, null));
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ConvertBackTest()
        {
            var converter = StringFormatConverter.Default;
            converter.ConvertBack(null, null, null, null);
        }
    }
}
