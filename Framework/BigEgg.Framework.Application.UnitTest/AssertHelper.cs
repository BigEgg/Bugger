using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace BigEgg.Framework.Application.UnitTesting
{
    public class AssertHelper
    {
        /// <summary>
        /// Asserts that the execution of the provided action raises the property changed event.
        /// </summary>
        /// <typeparam name="T">The type of the observable.</typeparam>
        /// <param name="observable">The observable which should raise the property changed event.</param>
        /// <param name="expression">A simple expression which identifies the property (e.g. x => x.Name).</param>
        /// <param name="raisePropertyChanged">An action that results in a property changed event of the observable.</param>
        /// <exception cref="AssertException">This exception is thrown when no or more than one property changed event was 
        /// raised by the observable or the sender object of the event was not the observable object.</exception>
        public static void PropertyChangedEvent<T>(T observable, Expression<Func<T, object>> expression, Action raisePropertyChanged)
            where T : class, INotifyPropertyChanged
        {
            if (observable == null) { throw new ArgumentNullException("observable"); }
            if (expression == null) { throw new ArgumentNullException("expression"); }
            if (raisePropertyChanged == null) { throw new ArgumentNullException("raisePropertyChanged"); }

            string propertyName = GetProperty(expression).Name;
            int propertyChangedCount = 0;

            observable.PropertyChanged += (object sender, PropertyChangedEventArgs e) =>
            {
                if (observable != sender)
                {
                    throw new AssertException("The sender object of the event isn't the observable");
                }

                if (e.PropertyName == propertyName)
                {
                    propertyChangedCount++;
                }
            };

            raisePropertyChanged();

            if (propertyChangedCount < 1)
            {
                throw new AssertException(string.Format(
                    "The PropertyChanged event for the property '{0}' wasn't raised.", propertyName));
            }
            else if (propertyChangedCount > 1)
            {
                throw new AssertException(string.Format(
                    "The PropertyChanged event for the property '{0}' was raised more than once.", propertyName));
            }
        }


        private static PropertyInfo GetProperty<TType>(Expression<Func<TType, object>> propertySelector)
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
