using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;
using BigEgg.Framework.Application.Foundation.Validations;
using BigEgg.Framework.Application.Foundation;
using System.Linq;

namespace BigEgg.Framework.Application.Test.Foundation.Validations
{
    [TestClass]
    public class SameAsAttributeTest
    {
        [TestMethod]
        public void ValidationTest_Valid()
        {
            AuthenticateInfo info = new AuthenticateInfo() { Password = "abc", RepeatPassword = "abc" };
            info.Validate();
            Assert.IsFalse(info.HasErrors);
            Assert.IsFalse(info.GetErrors().Any());
        }

        [TestMethod]
        public void ValidationTest_InValid()
        {
            AuthenticateInfo info = new AuthenticateInfo() { Password = "abc", RepeatPassword = "bcd" };
            info.Validate();
            Assert.IsTrue(info.HasErrors);
            Assert.AreEqual(AuthenticateInfo.RepeatPasswordErrorMessage, info.GetErrors().Single().ErrorMessage);
            Assert.AreEqual(AuthenticateInfo.RepeatPasswordErrorMessage, info.GetErrors("RepeatPassword").Single().ErrorMessage);
        }

        [TestMethod]
        public void ValidationTest_PropertyNull()
        {
            AuthenticateInfo info = new AuthenticateInfo();
            info.Validate();
            Assert.IsFalse(info.HasErrors);
            Assert.IsFalse(info.GetErrors().Any());
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void ValidationTest_DifferentPropertyType()
        {
            WrongAuthenticateInfo info = new WrongAuthenticateInfo()
            {
                Password = false,
                RepeatPassword = "abc"
            };
            info.Validate();
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void ValidtionTest_DependentPropertyNotExist()
        {
            WrongAuthenticateInfo2 info = new WrongAuthenticateInfo2();
            info.Validate();
        }


        private class AuthenticateInfo : ValidatableModel
        {
            public const string RepeatPasswordErrorMessage = "Repeat password is not same as password.";

            public string Password { get; set; }

            [SameAs("Password", ErrorMessage = RepeatPasswordErrorMessage)]
            public string RepeatPassword { get; set; }
        }

        private class WrongAuthenticateInfo : ValidatableModel
        {
            public const string RepeatPasswordErrorMessage = "Repeat password is not same as password.";

            public bool Password { get; set; }

            [SameAs("Password", ErrorMessage = RepeatPasswordErrorMessage)]
            public string RepeatPassword { get; set; }
        }

        private class WrongAuthenticateInfo2 : ValidatableModel
        {
            public const string RepeatPasswordErrorMessage = "Repeat password is not same as password.";

            public bool Password { get; set; }

            [SameAs("WrongPropertyName", ErrorMessage = RepeatPasswordErrorMessage)]
            public string RepeatPassword { get; set; }
        }
    }
}
