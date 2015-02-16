using BigEgg.Framework.Application.Foundation;
using BigEgg.Framework.Application.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace BigEgg.Framework.Application.Test.Foundation
{
    [TestClass]
    public class ModelTest
    {
        [TestMethod]
        public void RaisePropertyChangedTest()
        {
            Person luke = new Person();

            AssertHelper.PropertyChangedEvent(luke, x => x.Name, () => luke.Name = "Luke");
            Assert.AreEqual("Luke", luke.Name);

            AssertHelper.PropertyChangedEvent(luke, x => x.Name, () => luke.Name = "Skywalker");
            Assert.AreEqual("Skywalker", luke.Name);

            AssertHelper.PropertyChangedEvent(luke, x => x.Email, () => luke.Email = "luke.skywalker@tatooine.com");
            Assert.AreEqual("luke.skywalker@tatooine.com", luke.Email);

            AssertHelper.PropertyChangedEvent(luke, x => x.Phone, () => luke.Phone = "42");
            Assert.AreEqual("42", luke.Phone);
        }

        [TestMethod]
        [ExpectedException(typeof(AssertException))]
        public void RaisePropertyChangedTest_NotRaiseWhenSameValue()
        {
            Person luke = new Person();

            AssertHelper.PropertyChangedEvent(luke, x => x.Name, () => luke.Name = "Luke");
            AssertHelper.PropertyChangedEvent(luke, x => x.Name, () => luke.Name = "Luke");
        }

        [TestMethod]
        public void AddAndRemoveEventHandler()
        {
            Person luke = new Person();
            bool eventRaised;

            PropertyChangedEventHandler eventHandler = (sender, e) =>
            {
                eventRaised = true;
            };

            eventRaised = false;
            luke.PropertyChanged += eventHandler;
            luke.Name = "Luke";
            Assert.IsTrue(eventRaised, "The property changed event needs to be raised");

            eventRaised = false;
            luke.PropertyChanged -= eventHandler;
            luke.Name = "Luke Skywalker";
            Assert.IsFalse(eventRaised, "The event handler must not be called because it was removed from the event.");
        }

        [TestMethod]
        public void SerializationTest()
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (MemoryStream stream = new MemoryStream())
            {
                Person person = new Person() { Name = "Hugo" };
                formatter.Serialize(stream, person);

                stream.Position = 0;
                Person newPerson = (Person)formatter.Deserialize(stream);
                Assert.AreEqual(person.Name, newPerson.Name);
            }
        }

        [TestMethod]
        public void SerializationWithDcsTest()
        {
            var serializer = new DataContractSerializer(typeof(Person));

            using (MemoryStream stream = new MemoryStream())
            {
                Person person = new Person() { Name = "Hugo" };
                serializer.WriteObject(stream, person);

                stream.Position = 0;
                Person newPerson = (Person)serializer.ReadObject(stream);
                Assert.AreEqual(person.Name, newPerson.Name);
            }
        }

        [TestMethod]
        public void SetPropertyTest()
        {
            Person person = new Person();

            string name = null;
            Assert.IsTrue(person.SetProperty(ref name, "Bill", "Name"));
            Assert.AreEqual("Bill", name);
            Assert.IsFalse(person.SetProperty(ref name, "Bill", "Name")); // Value has not been changed
        }


        [Serializable]
        private class Person : Model
        {
            private string name;
            private string email;
            private string phone;

            public string Name
            {
                get { return name; }
                set { SetProperty(ref name, value); }
            }

            public string Email
            {
                get { return email; }
                set
                {
                    if (email != value)
                    {
                        email = value;
                        RaisePropertyChanged();
                    }
                }
            }

            public string Phone
            {
                get { return phone; }
                set
                {
                    if (phone != value)
                    {
                        phone = value;
                        RaisePropertyChanged("Phone");
                    }
                }
            }

            public new bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
            {
                return base.SetProperty(ref field, value, propertyName);
            }
        }
    }
}
