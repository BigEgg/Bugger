using BigEgg.Framework.UnitTesting;
using Bugger.Proxys.Models;
using Microsoft.TeamFoundation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Bugger.Proxys.TFS.Test
{
    [TestClass]
    public class TFSSourceControllerTest
    {
        [TestMethod]
        public void GeneralTFSSourceControllerTest()
        {
            TFSSourceController controller = new TFSSourceController("server");

            Assert.AreEqual("http://server:8080/tfs", controller.ConnectUri.AbsoluteUri);
        }

        [TestMethod]
        public void QueryTest()
        {
            TFSSourceController controller = new TFSSourceController(
                "https://tfs.codeplex.com", 443, "tfs/TFS12");

            AssertHelper.ExpectedException<TeamFoundationServerUnauthorizedException>(() =>
                controller.Query(new NetworkCredential("snd\\BigEgg_cp", "SomePassword"), "BigEgg_cp", "Feature"));

            //  Replace the password to the right one when run this unit test.
            List<Bug> items = controller.Query(
                new NetworkCredential("snd\\BigEgg_cp", "[password]"), "BigEgg_cp", "Feature");

            Assert.IsTrue(items.Any());
        }
    }
}
