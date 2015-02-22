using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BigEgg.Framework.Applications.Foundation.Validations
{
    /// <summary>
    /// Specifies that a data field value should be same as another property's value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class SameAsAttribute : ValidationAttribute
    {
        private string dependentPropertyName;

        /// <summary>
        /// Initializes a new instance of the <see cref="SameAsAttribute"/> class with the target property name.
        /// </summary>
        /// <param name="dependentPropertyName"></param>
        public SameAsAttribute(string dependentPropertyName)
        {
            this.dependentPropertyName = dependentPropertyName;
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

            var dependentPropertyValue = dependentProperty.GetValue(validationContext.ObjectInstance);
            if (dependentPropertyValue != null && value != null && dependentPropertyValue.GetType() != value.GetType())
            {
                throw new ValidationException("The property type is not same as dependent property.");
            }

            if (dependentPropertyValue != value)
            {
                return new ValidationResult(ErrorMessageString, new List<string> { validationContext.MemberName });
            }
            return ValidationResult.Success;
        }
    }
}
