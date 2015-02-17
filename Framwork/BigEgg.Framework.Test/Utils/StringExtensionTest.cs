using BigEgg.Framework.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace BigEgg.Framework.Test.Utils
{
    [TestClass]
    public class StringExtensionTest
    {
        [TestMethod]
        public void BuildTest()
        {
            var str = 'A'.Build(64);
            Assert.AreEqual(64, str.Length);
            Assert.AreEqual(64, str.Count(x => x == 'A'));
        }
    }
}
