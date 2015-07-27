using BigEgg.Framework.Utils;
using Bugger.PlugIns.Configs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;

namespace Bugger.Domain.Test.PlugIns.Configs
{
    [TestClass]
    public class PlugInAuthorTest
    {
        [TestMethod]
        public void SerializeTest()
        {
            var author = new PlugInAuthor()
            {
                Name = "BigEgg",
                EmailAddress = "bigegg@bigegg.com"
            };
            var xml = author.ObjectToXElement();

            Assert.AreEqual("Author", xml.Name);
            Assert.AreEqual("BigEgg", xml.Attribute("Name").Value);
            Assert.AreEqual("bigegg@bigegg.com", xml.Value);
        }

        [TestMethod]
        public void DeserializeTest()
        {
            var xml = new XElement("Author",
                new XAttribute("Name", "BigEgg"),
                new XText("bigegg@bigegg.com")
            );
            var author = xml.XElementToObject<PlugInAuthor>();

            Assert.IsNotNull(author);
            Assert.AreEqual("BigEgg", author.Name);
            Assert.AreEqual("bigegg@bigegg.com", author.EmailAddress);
        }
    }
}
