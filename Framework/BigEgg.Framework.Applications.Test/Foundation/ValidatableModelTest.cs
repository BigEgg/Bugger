using BigEgg.Framework.Applications.Foundation;
using BigEgg.Framework.Applications.UnitTesting;
using BigEgg.Framework.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace BigEgg.Framework.Applications.Test.Foundation
{
    [TestClass]
    public class ValidatableModelTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            Person person = new Person();
            Assert.IsFalse(person.HasErrors);
            Assert.IsFalse(person.GetErrors().Any());
        }

        [TestMethod]
        public void ValidateTest_Entity()
        {
            Person person = new Person();

            AssertHelper.IsRaisePropertyChangedEvent(person, x => x.HasErrors, () =>
                AssertHelper.IsRaiseErrorChangedEvent(person, () => person.Validate()));
            Assert.IsTrue(person.HasErrors);
            Assert.AreEqual(Person.NameMadatoryErrorMessage, person.GetErrors().Single().ErrorMessage);
            Assert.AreEqual(Person.NameMadatoryErrorMessage, person.GetErrors("Name").Single().ErrorMessage);
        }

        [TestMethod]
        public void EntityValidationErrorTest()
        {
            Person person = new Person() { Name = "Bill" };
            person.Validate();
            Assert.IsFalse(person.HasErrors);

            var entityError = new ValidationResult("My entity error");
            person.EntityError = entityError;

            AssertHelper.IsRaisePropertyChangedEvent(person, x => x.HasErrors, () =>
                AssertHelper.IsRaiseErrorChangedEvent(person, () => person.Validate()));
            Assert.IsTrue(person.HasErrors);
            Assert.AreEqual(entityError, person.GetErrors().Single());
            Assert.AreEqual(entityError, person.GetErrors("").Single());
            Assert.AreEqual(entityError, person.GetErrors(null).Single());
        }

        [TestMethod]
        [ExpectedException(typeof(NoEventRaiseException))]
        public void NotRaiseErrorsChanged_SameValue()
        {
            Person person = new Person() { Name = "Bill" };
            AssertHelper.IsRaiseErrorChangedEvent(person, x => x.Name, () => person.Name = "Bill");
        }

        [TestMethod]
        [ExpectedException(typeof(NoEventRaiseException))]
        public void NotRaiseErrorsChanged_ValidData()
        {
            Person person = new Person() { Name = "Bill" };
            AssertHelper.IsRaiseErrorChangedEvent(person, x => x.Name, () => person.Name = "Luke");
        }

        [TestMethod]
        [ExpectedException(typeof(NoEventRaiseException))]
        public void NotRaiseErrosChanged_WhenValid()
        {
            Person person = new Person() { Name = "Bill" };
            AssertHelper.IsRaiseErrorChangedEvent(person, () => person.Validate());
        }

        [TestMethod]
        public void SetPropertyAndValidateTest()
        {
            Person person = new Person();

            string name = null;
            Assert.IsTrue(person.SetPropertyAndValidate(ref name, "Bill", "Name"));
            Assert.IsFalse(person.SetPropertyAndValidate(ref name, "Bill", "Name"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetPropertyAndValidateTest_PropertyNameNull()
        {
            Person person = new Person();

            string name = null;
            person.SetPropertyAndValidate(ref name, "Bill", null);
        }

        [TestMethod]
        public void SetPropertyAndValidateTest_SingleError()
        {
            Person person = new Person();
            person.Validate();

            AssertHelper.IsRaiseBothErrorChangedEventAndPropertyChangedEvent(person, x => x.Name, () => person.Name = "Bill");
            Assert.IsFalse(person.HasErrors);
            Assert.IsFalse(person.GetErrors().Any());
            Assert.IsFalse(person.GetErrors("Name").Any());
            Assert.IsFalse(((INotifyDataErrorInfo)person).GetErrors("Name").Cast<object>().Any());

            AssertHelper.IsRaiseBothErrorChangedEventAndPropertyChangedEvent(person, x => x.Name, () => person.Name = null);
            Assert.IsTrue(person.HasErrors);
            Assert.IsTrue(person.GetErrors().Any());
            Assert.IsTrue(person.GetErrors("Name").Any());
            Assert.IsTrue(((INotifyDataErrorInfo)person).GetErrors("Name").Cast<object>().Any());
            Assert.AreEqual(Person.NameMadatoryErrorMessage, person.GetErrors().Single().ErrorMessage);
            Assert.AreEqual(Person.NameMadatoryErrorMessage, person.GetErrors("Name").Single().ErrorMessage);
        }

        [TestMethod]
        public void SetPropertyAndValidateTest_2Errors()
        {
            Person person = new Person() { Name = "Bill" };
            person.Validate();

            AssertHelper.IsRaiseBothErrorChangedEventAndPropertyChangedEvent(person, x => x.Email, () => person.Email = 'A'.Build(65));
            Assert.IsTrue(person.HasErrors);
            Assert.AreEqual(2, person.GetErrors().Count());
            Assert.AreEqual(2, person.GetErrors("Email").Count());
            Assert.IsTrue(person.GetErrors("Email").Any(x => x.ErrorMessage == Person.EmailInvalidErrorMessage));
            Assert.IsTrue(person.GetErrors("Email").Any(x => x.ErrorMessage == Person.EmailLengthErrorMessage));
            Assert.IsTrue(((INotifyDataErrorInfo)person).GetErrors("Email").Cast<object>().Any());

            AssertHelper.IsRaiseBothErrorChangedEventAndPropertyChangedEvent(person, x => x.Email, () => person.Email = "BigEgg@BigEgg.com");
            Assert.IsFalse(person.HasErrors);
            Assert.IsFalse(person.GetErrors().Any());
            Assert.IsFalse(person.GetErrors("Email").Any());
            Assert.IsFalse(((INotifyDataErrorInfo)person).GetErrors("Email").Cast<object>().Any());
        }

        [TestMethod]
        public void ValidatePropertyTest()
        {
            Person person = new Person();

            person.Name = "Bill";
            Assert.IsTrue(person.ValidateProperty(person.Name, "Name"));
            person.Name = null;
            Assert.IsFalse(person.ValidateProperty(person.Name, "Name"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidatePropertyTest_PropertyNameNull()
        {
            Person person = new Person();
            person.ValidateProperty("Bill", null);
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
        public void SerializationWithDCSTest()
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


        [Serializable]
        private class Person : ValidatableModel
        {
            public const string NameMadatoryErrorMessage = "The Name field is madatory.";
            public const string EmailInvalidErrorMessage = "The Email field is not a valid e-mail address.";
            public const string EmailLengthErrorMessage = "The field Email must be a string with a maximum length of 64.";
            private string name;
            private string email;
            [NonSerialized]
            private ValidationResult entityError;

            [Required(ErrorMessage = NameMadatoryErrorMessage)]
            public string Name
            {
                get { return name; }
                set { SetPropertyAndValidate(ref name, value); }
            }

            [EmailAddress(ErrorMessage = EmailInvalidErrorMessage)]
            [StringLength(64, ErrorMessage = EmailLengthErrorMessage)]
            public string Email
            {
                get { return email; }
                set { SetPropertyAndValidate(ref email, value); }
            }

            public ValidationResult EntityError
            {
                get { return entityError; }
                set { entityError = value; }
            }


            public new bool ValidateProperty(object value, string propertyName)
            {
                return base.ValidateProperty(value, propertyName);
            }

            public new bool SetPropertyAndValidate<T>(ref T field, T value, string propertyName)
            {
                return base.SetPropertyAndValidate(ref field, value, propertyName);
            }

            public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                var validationResults = new List<ValidationResult>();
                if (EntityError != null) { validationResults.Add(EntityError); }
                return validationResults;
            }
        }
    }
}
