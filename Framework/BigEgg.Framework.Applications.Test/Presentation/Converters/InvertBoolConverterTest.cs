using BigEgg.Framework.Applications.Presentation.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BigEgg.Framework.Applications.Test.Presentation.Converters
{
    [TestClass]
    public class InvertBooleanConverterTest
    {
        [TestMethod]
        public void DefaultTest()
        {
            var converter = InvertBooleanConverter.Default;
            Assert.AreEqual(converter, InvertBooleanConverter.Default);
        }

        [TestMethod]
        public void ConvertTest()
        {
            var converter = InvertBooleanConverter.Default;

            Assert.IsFalse((bool)converter.Convert(true, null, null, null));
            Assert.IsTrue((bool)converter.Convert(false, null, null, null));
        }

        [TestMethod]
        public void ConvertBackTest()
        {
            var converter = InvertBooleanConverter.Default;

            Assert.IsFalse((bool)converter.ConvertBack(true, null, null, null));
            Assert.IsTrue((bool)converter.ConvertBack(false, null, null, null));
        }
    }
}
