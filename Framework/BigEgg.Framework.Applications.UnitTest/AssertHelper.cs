using BigEgg.Framework.Applications.Foundation;
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Input;

namespace BigEgg.Framework.Applications.UnitTesting
{
    public class AssertHelper
    {
        /// <summary>
        /// Asserts that the execution of the provided action raises the property changed event.
        /// </summary>
        /// <typeparam name="T">The type of the observable.</typeparam>
        /// <param name="observable">The observable which should raise the property changed event.</param>
        /// <param name="propertySelector">A simple expression which identifies the property (e.g. x => x.Name).</param>
        /// <param name="action">An action that results in a property changed event of the observable.</param>
        /// <exception cref="AssertException">This exception is thrown when no or more than one property changed event was 
        /// raised by the observable or the sender object of the event was not the observable object.</exception>
        public static void IsRaisePropertyChangedEvent<T>(T observable, Expression<Func<T, object>> propertySelector, Action action)
            where T : class, INotifyPropertyChanged
        {
            Preconditions.NotNull(observable, "observable");
            Preconditions.NotNull(propertySelector, "propertySelector");
            Preconditions.NotNull(action, "action");

            string propertyName = GetProperty(propertySelector).Name;
            int propertyChangedCount = 0;

            PropertyChangedEventHandler handler = (sender, e) =>
            {
                if (observable != sender) { throw new SenderObservableNotSameException(); }
                if (e.PropertyName == propertyName)
                {
                    propertyChangedCount++;
                }
            };

            observable.PropertyChanged += handler;
            action();
            observable.PropertyChanged -= handler;

            if (propertyChangedCount < 1)
            {
                throw new NoEventRaiseException(string.Format(
                    "The PropertyChanged event for the property '{0}' wasn't raised.", propertyName));
            }
            if (propertyChangedCount > 1)
            {
                throw new EventRaiseMoreThanOnceException(string.Format(
                    "The PropertyChanged event for the property '{0}' was raised more than once.", propertyName));
            }
        }

        /// <summary>
        /// Asserts that the execution of the provided action raises the errors changed event.
        /// </summary>
        /// <typeparam name="T">The type of the observable.</typeparam>
        /// <param name="observable">The observable which should raise the errors changed event.</param>
        /// <param name="action">An action that results in a errors changed event of the observable.</param>
        /// <exception cref="AssertException">This exception is thrown when no or more than one errors changed event was 
        /// raised by the observable or the sender object of the event was not the observable object.</exception>
        public static void IsRaiseErrorChangedEvent<T>(T observable, Action action)
            where T : class, INotifyDataErrorInfo
        {
            Preconditions.NotNull(observable, "observable");
            Preconditions.NotNull(action, "action");

            int errorChangedCount = 0;
            EventHandler<DataErrorsChangedEventArgs> handler = (sender, e) =>
            {
                if (observable != sender) { throw new SenderObservableNotSameException(); }
                errorChangedCount++;
            };

            observable.ErrorsChanged += handler;
            action();
            observable.ErrorsChanged -= handler;

            if (errorChangedCount < 1)
            {
                throw new NoEventRaiseException("The ErrorsChanged event for the entity wasn't raised.");
            }
            if (errorChangedCount > 1)
            {
                throw new EventRaiseMoreThanOnceException("The ErrorsChanged event for the entity was raised more than once.");
            }
        }

        /// <summary>
        /// Asserts that the execution of the provided action raises the errors changed event.
        /// </summary>
        /// <typeparam name="T">The type of the observable.</typeparam>
        /// <param name="observable">The observable which should raise the errors changed event.</param>
        /// <param name="propertySelector">A simple expression which identifies the property (e.g. x => x.Name).</param>
        /// <param name="action">An action that results in a errors changed event of the observable.</param>
        /// <exception cref="AssertException">This exception is thrown when no or more than one errors changed event was 
        /// raised by the observable or the sender object of the event was not the observable object.</exception>
        public static void IsRaiseErrorChangedEvent<T>(T observable, Expression<Func<T, object>> propertySelector, Action action)
            where T : class, INotifyDataErrorInfo
        {
            Preconditions.NotNull(observable, "observable");
            Preconditions.NotNull(propertySelector, "propertySelector");
            Preconditions.NotNull(action, "action");

            string propertyName = GetProperty(propertySelector).Name;
            int errorChangedCount = 0;

            EventHandler<DataErrorsChangedEventArgs> handler = (sender, e) =>
            {
                if (observable != sender) { throw new SenderObservableNotSameException(); }
                if (e.PropertyName == propertyName)
                {
                    errorChangedCount++;
                }
            };

            observable.ErrorsChanged += handler;
            action();
            observable.ErrorsChanged -= handler;

            if (errorChangedCount < 1)
            {
                throw new NoEventRaiseException(string.Format(
                    "The ErrorsChanged event for the property '{0}' wasn't raised.", propertyName));
            }
            if (errorChangedCount > 1)
            {
                throw new EventRaiseMoreThanOnceException(string.Format(
                    "The ErrorsChanged event for the property '{0}' was raised more than once.", propertyName));
            }
        }


        /// <summary>
        /// Asserts that the execution of the provided action raises both the errors changed event and property changed event.
        /// </summary>
        /// <typeparam name="T">The type of the observable.</typeparam>
        /// <param name="observable">The observable which should raise both the errors changed event and property changed event.</param>
        /// <param name="propertySelector">A simple expression which identifies the property (e.g. x => x.Name).</param>
        /// <param name="action">An action that results in a errors changed event and a property changed event of the observable.</param>
        /// <exception cref="AssertException">This exception is thrown when no or more than one errors changed event or 
        /// property changed event was raised by the observable or the sender object of the event was not the observable object.</exception>
        public static void IsRaiseBothErrorChangedEventAndPropertyChangedEvent<T>(T observable, Expression<Func<T, object>> propertySelector, Action action)
            where T : ValidatableModel
        {
            Preconditions.NotNull(observable, "observable");
            Preconditions.NotNull(propertySelector, "propertySelector");
            Preconditions.NotNull(action, "action");

            string propertyName = GetProperty(propertySelector).Name;
            int errorChangedCount = 0;
            int propertyChangedCount = 0;

            EventHandler<DataErrorsChangedEventArgs> errorChangedHandler = (sender, e) =>
            {
                if (observable != sender) { throw new SenderObservableNotSameException(); }
                if (e.PropertyName == propertyName)
                {
                    errorChangedCount++;
                }
            };
            PropertyChangedEventHandler propertyChangedHandler = (sender, e) =>
            {
                if (observable != sender) { throw new SenderObservableNotSameException(); }
                if (e.PropertyName == propertyName)
                {
                    propertyChangedCount++;
                }
            };

            observable.ErrorsChanged += errorChangedHandler;
            observable.PropertyChanged += propertyChangedHandler;
            action();
            observable.PropertyChanged -= propertyChangedHandler;
            observable.ErrorsChanged -= errorChangedHandler;

            if (propertyChangedCount < 1)
            {
                throw new NoEventRaiseException(string.Format(
                    "The PropertyChanged event for the property '{0}' wasn't raised.", propertyName));
            }
            if (propertyChangedCount > 1)
            {
                throw new EventRaiseMoreThanOnceException(string.Format(
                    "The PropertyChanged event for the property '{0}' was raised more than once.", propertyName));
            }
            if (errorChangedCount < 1)
            {
                throw new NoEventRaiseException(string.Format(
                    "The ErrorsChanged event for the property '{0}' wasn't raised.", propertyName));
            }
            if (errorChangedCount > 1)
            {
                throw new EventRaiseMoreThanOnceException(string.Format(
                    "The ErrorsChanged event for the property '{0}' was raised more than once.", propertyName));
            }
        }


        /// <summary>
        /// Asserts that the execution of the provided action raises the CanExecuteChanged event of the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="raiseCanExecuteChanged">An action that results in a CanExecuteChanged event of the command.</param>
        /// <exception cref="AssertException">This exception is thrown when no or more than one CanExecuteChanged event was 
        /// raised by the command or the sender object of the event was not the command object.</exception>
        public static void CanExecuteChangedEvent(ICommand command, Action raiseCanExecuteChanged)
        {
            Preconditions.NotNull(command, "command");
            Preconditions.NotNull(raiseCanExecuteChanged, "raiseCanExecuteChanged");

            int canExecuteChangedCount = 0;

            command.CanExecuteChanged += (object sender, EventArgs e) =>
            {
                if (command != sender) { throw new SenderCommandNotSameException(); }
                canExecuteChangedCount++;
            };

            raiseCanExecuteChanged();

            if (canExecuteChangedCount < 1)
            {
                throw new NoEventRaiseException("The CanExecuteChanged event wasn't raised.");
            }
            if (canExecuteChangedCount > 1)
            {
                throw new EventRaiseMoreThanOnceException("The CanExecuteChanged event was raised more than once.");
            }
        }



        /// <summary>
        /// Get the model's property info.
        /// </summary>
        /// <typeparam name="T">The type of the model.</typeparam>
        /// <param name="propertySelector">A simple expression which identifies the property (e.g. x => x.Name).</param>
        /// <returns>The property info.</returns>
        /// <exception cref="ArgumentException">This exception si thrown when the expression isn't valid, or is the 
        /// parameter itself, or cannot get the property.</exception>
        public static PropertyInfo GetProperty<T>(Expression<Func<T, object>> propertySelector)
        {
            Expression expression = propertySelector.Body;

            // If the Property returns a ValueType then a Convert is required => Remove it
            if (expression.NodeType == ExpressionType.Convert || expression.NodeType == ExpressionType.ConvertChecked)
            {
                expression = ((UnaryExpression)expression).Operand;
            }

            // If this isn't a member access expression then the expression isn't valid
            MemberExpression memberExpression = expression as MemberExpression;
            if (memberExpression == null)
            {
                ThrowExpressionArgumentException("propertySelector");
            }

            expression = memberExpression.Expression;

            // If the Property returns a ValueType then a Convert is required => Remove it
            if (expression.NodeType == ExpressionType.Convert || expression.NodeType == ExpressionType.ConvertChecked)
            {
                expression = ((UnaryExpression)expression).Operand;
            }

            // Check if the expression is the parameter itself
            if (expression.NodeType != ExpressionType.Parameter)
            {
                ThrowExpressionArgumentException("propertySelector");
            }

            // Finally retrieve the MemberInfo
            PropertyInfo propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo == null)
            {
                ThrowExpressionArgumentException("propertySelector");
            }

            return propertyInfo;
        }

        private static void ThrowExpressionArgumentException(string argumentName)
        {
            throw new ArgumentException("It's just the simple expression 'x => x.Property' allowed.", argumentName);
        }
    }
}
