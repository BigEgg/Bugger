using BigEgg.Framework.Applications.Presentation.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace BigEgg.Framework.Applications.Test.Presentation.Converters
{
    [TestClass]
    public class ValidationErrorsConverterTest
    {
        [TestMethod]
        public void DefaultTest()
        {
            var converter = ValidationErrorsConverter.Default;
            Assert.AreEqual(converter, ValidationErrorsConverter.Default);
        }

        [TestMethod]
        public void ConvertTest()
        {
            var converter = ValidationErrorsConverter.Default;

            Assert.AreEqual(DependencyProperty.UnsetValue, converter.Convert((object[])null, null, null, null));
            Assert.AreEqual(DependencyProperty.UnsetValue, converter.Convert(new[] { "WrongType" }, null, null, null));

            List<ValidationError> validationErrors = new List<ValidationError>();
            Assert.AreEqual("", converter.Convert(new[] { validationErrors }, null, null, null));

            ExceptionValidationRule rule = new ExceptionValidationRule();
            validationErrors.Add(new ValidationError(rule, new object(), "First error message", null));
            Assert.AreEqual("First error message", converter.Convert(new[] { validationErrors }, null, null, null));

            validationErrors.Add(new ValidationError(rule, new object(), "Second error message", null));
            Assert.AreEqual("First error message" + Environment.NewLine + "Second error message",
                converter.Convert(new[] { validationErrors }, null, null, null));
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ConvertBackTest()
        {
            var converter = ValidationErrorsConverter.Default;
            converter.ConvertBack(null, null, null, null);
        }
    }
}
