using BigEgg.Framework.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BigEgg.Framework.Applications.Foundation
{
    /// <summary>
    /// Defines a base class for a model that supports validation.
    /// </summary>
    [Serializable]
    public abstract class ValidatableModel : Model, INotifyDataErrorInfo, IValidatableObject
    {
        private static readonly ValidationResult[] NO_ERROR = new ValidationResult[0];

        [NonSerialized]
        private readonly Dictionary<string, IList<ValidationResult>> errors;
        [NonSerialized]
        private bool hasErrors;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatableModel"/> class.
        /// </summary>
        protected ValidatableModel()
        {
            this.errors = new Dictionary<string, IList<ValidationResult>>();
        }

        #region Implement INotifyDataErrorInfo Interface
        /// <summary>
        /// Occurs when the validation errors have changed for a property or for the entire entity.
        /// </summary>
        [field: NonSerialized]
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <summary>
        /// Gets a value that indicates whether the entity has validation errors.
        /// </summary>
        public bool HasErrors
        {
            get { return hasErrors; }
            private set { SetProperty(ref hasErrors, value); }
        }

        IEnumerable INotifyDataErrorInfo.GetErrors(string propertyName)
        {
            return GetErrors(propertyName);
        }
        #endregion

        #region Implement IValidatableObject Interface
        /// <summary>
        /// Determines whether the specified object is valid.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>A collection that holds failed-validation information.</returns>
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
        #endregion

        #region RaiseErrorsChanged
        /// <summary>
        /// Raises the <see cref="E:ErrorsChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.ComponentModel.DataErrorsChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnErrorsChanged(DataErrorsChangedEventArgs e)
        {
            EventHandler<DataErrorsChangedEventArgs> handler = ErrorsChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void RaiseErrorsChanged(string propertyName = "")
        {
            HasErrors = errors.Any();
            OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
        }
        #endregion

        #region GetErrors
        /// <summary>
        /// Gets the validation errors for the entire entity.
        /// </summary>
        /// <returns>The validation errors for the entity.</returns>
        public IEnumerable<ValidationResult> GetErrors()
        {
            return GetErrors(null);
        }

        /// <summary>
        /// Gets the validation errors for a specified property or for the entire entity.
        /// </summary>
        /// <param name="propertyName">The name of the property to retrieve validation errors for; 
        /// or null or String.Empty or white spaces, to retrieve entity-level errors.</param>
        /// <returns>The validation errors for the property or entity.</returns>
        public IEnumerable<ValidationResult> GetErrors(string propertyName)
        {
            if (!string.IsNullOrWhiteSpace(propertyName))
            {
                IList<ValidationResult> result;
                return errors.TryGetValue(propertyName, out result)
                    ? result.ToArray()
                    : NO_ERROR;
            }
            else
            {
                return errors.Values.SelectMany(x => x).Distinct().ToArray();
            }
        }
        #endregion

        #region Models
        /// <summary>
        /// Validates the object and all its properties. The validation results are stored and can be retrieved by the 
        /// GetErrors method. If the validation results are changing then the ErrorsChanged event will be raised.
        /// </summary>
        /// <returns>True if the object is valid, otherwise false.</returns>
        public bool Validate()
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(this, new ValidationContext(this), validationResults, true);
            if (validationResults.Any())
            {
                errors.Clear();
                validationResults.ForEach(validationResult =>
                {
                    var propertyNames = validationResult.MemberNames.Any()
                        ? validationResult.MemberNames.ToList()
                        : new List<string>() { "" };
                    propertyNames.ForEach(propertyName => errors.AddOrUpdate(propertyName, validationResult));
                });
                RaiseErrorsChanged();
                return false;
            }
            else
            {
                if (errors.Any())
                {
                    errors.Clear();
                    RaiseErrorsChanged();
                }
            }
            return true;
        }

        /// <summary>
        /// Set the property with the specified value and validate the property. If the value is not equal with the field then the field is
        /// set, a PropertyChanged event is raised, the property is validated and it returns true.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="field">Reference to the backing field of the property.</param>
        /// <param name="value">The new value for the property.</param>
        /// <param name="propertyName">The property name. This optional parameter can be skipped
        /// because the compiler is able to create it automatically.</param>
        /// <returns>True if the value has changed, false if the old and new value were equal.</returns>
        /// <exception cref="ArgumentException">The argument propertyName must not be null or empty.</exception>
        protected bool SetPropertyAndValidate<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            Preconditions.NotNullOrWhiteSpace(propertyName, "The argument propertyName must not be null or empty.");

            if (SetProperty(ref field, value, propertyName))
            {
                ValidateProperty(value, propertyName);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Validates the property with the specified value. The validation results are stored and can be retrieved by the 
        /// GetErrors method. If the validation results are changing then the ErrorsChanged event will be raised.
        /// </summary>
        /// <param name="value">The value of the property.</param>
        /// <param name="propertyName">The property name. This optional parameter can be skipped
        /// because the compiler is able to create it automatically.</param>
        /// <returns>True if the property value is valid, otherwise false.</returns>
        /// <exception cref="ArgumentException">The argument propertyName must not be null or empty.</exception>
        protected bool ValidateProperty(object value, [CallerMemberName] string propertyName = null)
        {
            Preconditions.NotNullOrWhiteSpace(propertyName, "The argument propertyName must not be null or empty.");

            IList<ValidationResult> validationResults = new List<ValidationResult>();
            Validator.TryValidateProperty(value, new ValidationContext(this) { MemberName = propertyName }, validationResults);
            if (validationResults.Any())
            {
                errors[propertyName] = validationResults;
                RaiseErrorsChanged(propertyName);
                return false;
            }
            else
            {
                if (errors.Remove(propertyName))
                {
                    RaiseErrorsChanged(propertyName);
                }
            }
            return true;
        }
        #endregion
    }
}
