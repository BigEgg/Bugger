using BigEgg.Framework.Utils;
using Bugger.PlugIns.Configs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Xml.Linq;

namespace Bugger.Base.Test.PlugIns.Configs
{
    [TestClass]
    public class DependencyPlugInTest
    {
        [TestMethod]
        public void SerializeTest()
        {
            var dependencyPlugIn = new DependencyPlugIn()
            {
                PlugInGuid = new Guid("1dc425b3-c27b-46ba-9623-a046d1acc754"),
                DependencyType = DependencyType.Mandatory,
                MinimumSupportPlugInVersionStr = "0.5.0.0",
                MaximumSupportPlugInVersionStr = "0.5.0.0"
            };
            var xml = dependencyPlugIn.ObjectToXElement();

            Assert.AreEqual("Dependency", xml.Name);
            Assert.AreEqual("1dc425b3-c27b-46ba-9623-a046d1acc754", xml.Attribute("Guid").Value.ToString());
            Assert.AreEqual("Mandatory", xml.Element("Type").Value);
            Assert.AreEqual("", xml.Element("Group").Value);
            Assert.AreEqual("0.5.0.0", xml.Element("MinimumSupportPlugInVersion").Value);
            Assert.AreEqual("0.5.0.0", xml.Element("MaximumSupportPlugInVersion").Value);
        }

        [TestMethod]
        public void SerializeTest_WithGroupId()
        {
            var dependencyPlugIn = new DependencyPlugIn()
            {
                PlugInGuid = new Guid("1dc425b3-c27b-46ba-9623-a046d1acc754"),
                DependencyType = DependencyType.OneOfGroup,
                GroupId = 1,
                MinimumSupportPlugInVersionStr = "0.5.0.0",
                MaximumSupportPlugInVersionStr = "0.5.0.0"
            };
            var xml = dependencyPlugIn.ObjectToXElement();

            Assert.AreEqual("Dependency", xml.Name);
            Assert.AreEqual("1dc425b3-c27b-46ba-9623-a046d1acc754", xml.Attribute("Guid").Value.ToString());
            Assert.AreEqual("OneOfGroup", xml.Element("Type").Value);
            Assert.AreEqual("1", xml.Element("Group").Value);
            Assert.AreEqual("0.5.0.0", xml.Element("MinimumSupportPlugInVersion").Value);
            Assert.AreEqual("0.5.0.0", xml.Element("MaximumSupportPlugInVersion").Value);
        }

        [TestMethod]
        public void SerializeTest_WithoutMaximumSuportPlugInVersion()
        {
            var dependencyPlugIn = new DependencyPlugIn()
            {
                PlugInGuid = new Guid("1dc425b3-c27b-46ba-9623-a046d1acc754"),
                DependencyType = DependencyType.Mandatory,
                MinimumSupportPlugInVersionStr = "0.5.0.0"
            };
            var xml = dependencyPlugIn.ObjectToXElement();

            Assert.AreEqual("Dependency", xml.Name);
            Assert.AreEqual("1dc425b3-c27b-46ba-9623-a046d1acc754", xml.Attribute("Guid").Value.ToString());
            Assert.AreEqual("Mandatory", xml.Element("Type").Value);
            Assert.AreEqual("", xml.Element("Group").Value);
            Assert.AreEqual("0.5.0.0", xml.Element("MinimumSupportPlugInVersion").Value);
            Assert.AreEqual("", xml.Element("MaximumSupportPlugInVersion").Value);
        }

        [TestMethod]
        public void DeserializeTest()
        {
            var xml = new XElement("Dependency",
                new XAttribute("Guid", "1dc425b3-c27b-46ba-9623-a046d1acc754"),
                new XElement("Type", "Mandatory"),
                new XElement("MinimumSupportPlugInVersion", "0.5.0.0"),
                new XElement("MaximumSupportPlugInVersion", "0.5.0.0")
            );
            var dependencyPlugIn = xml.XElementToObject<DependencyPlugIn>();

            Assert.IsNotNull(dependencyPlugIn);
            Assert.AreEqual("1dc425b3-c27b-46ba-9623-a046d1acc754", dependencyPlugIn.PlugInGuid.ToString());
            Assert.AreEqual(DependencyType.Mandatory, dependencyPlugIn.DependencyType);
            Assert.IsFalse(dependencyPlugIn.GroupId.HasValue);
            Assert.AreEqual("0.5.0.0", dependencyPlugIn.MinimumSupportPlugInVersion.ToString());
            Assert.AreEqual("0.5.0.0", dependencyPlugIn.MaximumSupportPlugInVersion.ToString());
        }

        [TestMethod]
        public void DeserializeTest_WithGroupId()
        {
            var xml = new XElement("Dependency",
                new XAttribute("Guid", "1dc425b3-c27b-46ba-9623-a046d1acc754"),
                new XElement("Type", "OneOfGroup"),
                new XElement("Group", "1"),
                new XElement("MinimumSupportPlugInVersion", "0.5.0.0"),
                new XElement("MaximumSupportPlugInVersion", "0.5.0.0")
            );
            var dependencyPlugIn = xml.XElementToObject<DependencyPlugIn>();

            Assert.IsNotNull(dependencyPlugIn);
            Assert.AreEqual("1dc425b3-c27b-46ba-9623-a046d1acc754", dependencyPlugIn.PlugInGuid.ToString());
            Assert.AreEqual(DependencyType.OneOfGroup, dependencyPlugIn.DependencyType);
            Assert.IsTrue(dependencyPlugIn.GroupId.HasValue);
            Assert.AreEqual(1, dependencyPlugIn.GroupId.Value);
            Assert.AreEqual("0.5.0.0", dependencyPlugIn.MinimumSupportPlugInVersion.ToString());
            Assert.AreEqual("0.5.0.0", dependencyPlugIn.MaximumSupportPlugInVersion.ToString());
        }

        [TestMethod]
        public void DeserializeTest_WithoutMaximumSuportPlugInVersion()
        {
            var xml = new XElement("Dependency",
                new XAttribute("Guid", "1dc425b3-c27b-46ba-9623-a046d1acc754"),
                new XElement("Type", "Mandatory"),
                new XElement("MinimumSupportPlugInVersion", "0.5.0.0")
            );
            var dependencyPlugIn = xml.XElementToObject<DependencyPlugIn>();

            Assert.IsNotNull(dependencyPlugIn);
            Assert.AreEqual("1dc425b3-c27b-46ba-9623-a046d1acc754", dependencyPlugIn.PlugInGuid.ToString());
            Assert.AreEqual(DependencyType.Mandatory, dependencyPlugIn.DependencyType);
            Assert.IsFalse(dependencyPlugIn.GroupId.HasValue);
            Assert.AreEqual("0.5.0.0", dependencyPlugIn.MinimumSupportPlugInVersion.ToString());
            Assert.IsNull(dependencyPlugIn.MaximumSupportPlugInVersion);
        }
    }
}
