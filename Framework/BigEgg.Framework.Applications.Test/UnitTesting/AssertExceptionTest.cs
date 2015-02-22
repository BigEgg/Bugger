using BigEgg.Framework.Applications.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace BigEgg.Framework.Applications.Test.UnitTesting
{
    [TestClass]
    public class AssertExceptionTest
    {
        public TestContext TestContext { get; set; }


        [TestMethod]
        public void AssertExceptionConstructorTest()
        {
            new AssertException();
            new AssertException("message");
            new AssertException("message", null);
        }

        [TestMethod]
        public void AssertExceptionSerializationTest()
        {
            AssertException assertException = new AssertException("message");

            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(stream, assertException);

            stream.Position = 0;
            AssertException newAssertException = (AssertException)formatter.Deserialize(stream);

            Assert.AreEqual(assertException.Message, newAssertException.Message);
        }
    }
}
