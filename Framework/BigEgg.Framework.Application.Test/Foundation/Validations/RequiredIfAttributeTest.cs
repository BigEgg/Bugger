using BigEgg.Framework.Application.Foundation;
using BigEgg.Framework.Application.Foundation.Validations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BigEgg.Framework.Application.Test.Foundation.Validations
{
    [TestClass]
    public class RequiredIfAttributeTest
    {
        [TestMethod]
        public void ValidationTest_NotMatch()
        {
            Person person = new Person();
            person.Validate();
            Assert.IsFalse(person.HasErrors);
            Assert.IsFalse(person.GetErrors().Any());
        }

        [TestMethod]
        public void ValidationTest_Match()
        {
            Person person = new Person();
            person.IsCouple = true;
            person.Validate();
            Assert.IsTrue(person.HasErrors);
            Assert.AreEqual(Person.CoupleNameErrorMessage, person.GetErrors().Single().ErrorMessage);
            Assert.AreEqual(Person.CoupleNameErrorMessage, person.GetErrors("CoupleName").Single().ErrorMessage);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void ValidtionTest_WrongDependentPropertyType()
        {
            WrongPerson person = new WrongPerson();
            person.IsCouple = "Something";
            person.Validate();
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void ValidtionTest_DependentPropertyNotExist()
        {
            WrongPerson2 person = new WrongPerson2();
            person.IsCouple = true;
            person.Validate();
        }

        private class Person : ValidatableModel
        {
            public const string CoupleNameErrorMessage = "Couple's name is mandatory.";

            public bool IsCouple { get; set; }

            [RequiredIf("IsCouple", ErrorMessage = CoupleNameErrorMessage)]
            public string CoupleName { get; set; }
        }

        private class WrongPerson : ValidatableModel
        {
            public const string CoupleNameErrorMessage = "Couple's name is mandatory.";

            public string IsCouple { get; set; }

            [RequiredIf("IsCouple", ErrorMessage = CoupleNameErrorMessage)]
            public string CoupleName { get; set; }
        }

        private class WrongPerson2 : ValidatableModel
        {
            public const string CoupleNameErrorMessage = "Couple's name is mandatory.";

            public bool IsCouple { get; set; }

            [RequiredIf("WrongPropertyName", ErrorMessage = CoupleNameErrorMessage)]
            public string CoupleName { get; set; }
        }
    }
}
