using BigEgg.Framework.Utils;
using Bugger.PlugIns.Configs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Bugger.Domain.Test.PlugIns.Configs
{
    [TestClass]
    public class PlugInInfoTest
    {
        [TestMethod]
        public void GeneralTest()
        {
            var author = new PlugInAuthor()
            {
                Name = "BigEgg",
                EmailAddress = "bigegg@bigegg.com"
            };
            var info = new PlugInInfo()
            {
                PlugInGuid = new Guid("1dc425b3-c27b-46ba-9623-a046d1acc754"),
                Name = "TFS Proxy",
                Description = "The tracking system proxy for TFS",
                Authors = new List<PlugInAuthor>() { author },
                VersionStr = "0.5.0.0",
                MinimumSupportBuggerVersionStr = "0.5.0.0",
                MaximumSupportBuggerVersionStr = "0.5.0.0"
            };

            Assert.AreEqual(1, info.Authors.Count);
            Assert.AreEqual("0.5.0.0", info.Version.ToString());
            Assert.AreEqual("0.5.0.0", info.MinimumSupportBuggerVersion.ToString());
            Assert.AreEqual("0.5.0.0", info.MaximumSupportBuggerVersion.ToString());
        }

        [TestMethod]
        public void SerializeTest()
        {
            var author = new PlugInAuthor()
            {
                Name = "BigEgg",
                EmailAddress = "bigegg@bigegg.com"
            };
            var info = new PlugInInfo()
            {
                PlugInGuid = new Guid("1dc425b3-c27b-46ba-9623-a046d1acc754"),
                Name = "TFS Proxy",
                Description = "The tracking system proxy for TFS",
                Authors = new List<PlugInAuthor>() { author },
                VersionStr = "0.5.0.0",
                MinimumSupportBuggerVersionStr = "0.5.0.0",
                MaximumSupportBuggerVersionStr = "0.5.0.0"
            };
            var xml = info.ObjectToXElement();

            Assert.AreEqual("Info", xml.Name);
            Assert.AreEqual("1dc425b3-c27b-46ba-9623-a046d1acc754", xml.Attribute("PlugInGuid").Value);
            Assert.AreEqual("TFS Proxy", xml.Element("Name").Value);
            Assert.AreEqual("The tracking system proxy for TFS", xml.Element("Description").Value);
            Assert.AreEqual(1, xml.Element("Authors").Elements().Count());
            Assert.AreEqual("0.5.0.0", xml.Element("Version").Value);
            Assert.AreEqual("0.5.0.0", xml.Element("MinimumSupportBuggerVersion").Value);
            Assert.AreEqual("0.5.0.0", xml.Element("MaximumSupportBuggerVersion").Value);
        }

        [TestMethod]
        public void DeserializeTest()
        {
            var xml = new XElement("Info",
                new XAttribute("PlugInGuid", "1dc425b3-c27b-46ba-9623-a046d1acc754"),
                new XElement("Name", "TFS Proxy"),
                new XElement("Description", "The tracking system proxy for TFS"),
                new XElement("Authors",
                    new XElement("Author",
                        new XAttribute("Name", "BigEgg"),
                        new XText("bigegg@bigegg.com")
                    )
                ),
                new XElement("Version", "0.5.0.0"),
                new XElement("MinimumSupportBuggerVersion", "0.5.0.0"),
                new XElement("MaximumSupportBuggerVersion", "0.5.0.0")
            );
            var info = xml.XElementToObject<PlugInInfo>();

            Assert.IsNotNull(info);
            Assert.AreEqual("1dc425b3-c27b-46ba-9623-a046d1acc754", info.PlugInGuid.ToString());
            Assert.AreEqual("TFS Proxy", info.Name);
            Assert.AreEqual("The tracking system proxy for TFS", info.Description);
            Assert.AreEqual(1, info.Authors.Count);
            Assert.AreEqual("0.5.0.0", info.Version.ToString());
            Assert.AreEqual("0.5.0.0", info.MinimumSupportBuggerVersion.ToString());
            Assert.AreEqual("0.5.0.0", info.MaximumSupportBuggerVersion.ToString());
        }
    }
}
