using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace BigEgg.Framework.Application.Foundation.Validations
{
    /// <summary>
    /// Specifies that a data field value is required when another property's value is equal to some specific data.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class RequiredIfAttribute : ValidationAttribute
    {
        private string dependentPropertyName;
        private bool validateValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequiredIfAttribute"/> class with the specified property name
        /// and the value which should be equal to.
        /// </summary>
        /// <param name="dependentPropertyName">The specified property name which need to be compared.</param>
        /// <param name="validateValue">The specified value to compare.</param>
        public RequiredIfAttribute(string dependentPropertyName, bool validateValue = true)
        {
            Preconditions.NotNullOrWhiteSpace(dependentPropertyName, "dependentPropertyNamw");

            this.dependentPropertyName = dependentPropertyName;
            this.validateValue = validateValue;
        }

        /// <summary>
        /// Validates the specified value with respect to the current validation attribute.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>An instance of the <see cref="ValidationResult"/> class.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Type type = validationContext.ObjectType;
            PropertyInfo dependentProperty = type.GetProperty(dependentPropertyName);
            if (dependentProperty == null) { throw new ValidationException("Cannot find the specified property."); }

            var currentValue = dependentProperty.GetValue(validationContext.ObjectInstance);
            if (!(currentValue is bool)) { throw new ValidationException("The dependent property is not an bool type."); }

            if ((bool)currentValue == validateValue && string.IsNullOrWhiteSpace(value as string))
            {
                return new ValidationResult(ErrorMessageString, new List<string>() { validationContext.MemberName });
            }
            return ValidationResult.Success;
        }
    }
}
