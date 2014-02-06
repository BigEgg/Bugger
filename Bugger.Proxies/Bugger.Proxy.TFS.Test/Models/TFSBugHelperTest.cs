using Bugger.Proxy.TFS.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bugger.Proxy.TFS.Test.Models
{
    [TestClass]
    public class TFSBugHelperTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var result = TFSBugHelper.GetPropertyNames();

            Assert.AreEqual(9, result.Count);

            foreach (var model in result)
            {
                Assert.AreEqual(string.Empty, model.Value);
            }

            Assert.IsTrue(result.Keys.Contains("ID"));
            Assert.IsTrue(result.Keys.Contains("Title"));
            Assert.IsTrue(result.Keys.Contains("Description"));
            Assert.IsTrue(result.Keys.Contains("AssignedTo"));
            Assert.IsTrue(result.Keys.Contains("State"));
            Assert.IsTrue(result.Keys.Contains("ChangedDate"));
            Assert.IsTrue(result.Keys.Contains("CreatedBy"));
            Assert.IsTrue(result.Keys.Contains("Priority"));
            Assert.IsTrue(result.Keys.Contains("Severity"));
        }
    }
}
