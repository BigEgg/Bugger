using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BigEgg.Framework.Applications.Presentation.Validations
{
    /// <summary>
    /// This class listens to the Validation.Error event of the owner (Control). When the
    /// Error event is raised then it synchronizes the errors with its internal errors list and
    /// updates the ValidationHelper.
    /// </summary>
    internal sealed class ValidationTracker
    {
        private readonly IList<Tuple<object, ValidationError>> errors;
        private readonly DependencyObject owner;

        public ValidationTracker(DependencyObject owner)
        {
            this.owner = owner;
            errors = new List<Tuple<object, ValidationError>>();

            Validation.AddErrorHandler(owner, ErrorChangedHandler);
        }

        private void ErrorChangedHandler(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                errors.Add(new Tuple<object, ValidationError>(e.OriginalSource, e.Error));
            }
            else
            {
                var error = errors.FirstOrDefault(err => err.Item1 == e.OriginalSource && err.Item2 == e.Error);
                if (error != null) { errors.Remove(error); }
            }

            ValidationHelper.InternalSetIsValid(owner, !errors.Any());
        }
    }
}
