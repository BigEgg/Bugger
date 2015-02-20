using BigEgg.Framework.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace BigEgg.Framework.Test.Utils
{
    [TestClass]
    public class XmlSerializeExtensionTest
    {
        [TestMethod]
        public void ObjectToXElementTest()
        {
            var person = new Person()
            {
                Name = "Bill",
                Age = 20,
                Email = "abc@test.com"
            };

            var root = person.ObjectToXElement();
            Assert.AreEqual("Person", root.Name);
            Assert.AreEqual("Bill", root.Attribute("FullName").Value);
            Assert.IsTrue(root.HasElements);
            Assert.AreEqual(20, int.Parse(root.Element("Age").Value));
            Assert.IsFalse(root.Elements().Any(x => x.Name == "Email"));
        }

        [TestMethod]
        public void XElementToObjectTest()
        {
            XElement root = new XElement("Person",
                new XAttribute("FullName", "Bill"),
                new XElement("Age", 20),
                new XElement("Email", "abc@test.com")
                );

            var person = root.XElementToObject<Person>();
            Assert.IsNotNull(person);
            Assert.AreEqual("Bill", person.Name);
            Assert.AreEqual(20, person.Age);
            Assert.IsTrue(string.IsNullOrWhiteSpace(person.Email));
        }

        [XmlRoot]
        public class Person
        {
            [XmlAttribute("FullName")]
            public string Name { get; set; }

            [XmlElement]
            public int Age { get; set; }

            [XmlIgnore]
            public string Email { get; set; }
        }
    }
}
