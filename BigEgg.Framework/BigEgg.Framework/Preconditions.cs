using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigEgg.Framework
{
    /// <summary>
    /// Hold all the precondition logics.
    /// </summary>
    public class Preconditions
    {
        /// <summary>
        /// Throw an <see cref="ArgumentNullException"> if the object is null.
        /// </summary>
        /// <param name="obj">The object which need to check.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        public static void NotNull(object obj, string paramName = "")
        {
            if (obj == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        /// <summary>
        /// Throw an <see cref="ArgumentNullException"> if a specified string is null, 
        /// empty, or consists only of white-space characters.
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public static void NotNullOrWhiteSpace(string value, string message = "")
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(message);
            }
        }

        /// <summary>
        /// Throw an <see cref="ArgumentNullException"> if the condition is false.
        /// </summary>
        /// <param name="condition">The check condition.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public static void Check(Func<bool> condition, string message = "")
        {
            if (!condition.Invoke())
            {
                throw new ArgumentException(message);
            }
        }
    }
}
