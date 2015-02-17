using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigEgg.Framework.Utils
{
    public static class StringExtension
    {
        /// <summary>
        /// Create a string with multiple character.
        /// </summary>
        /// <param name="character">The specific character.</param>
        /// <param name="count">The time of the character in the string.</param>
        /// <returns>The string.</returns>
        public static string Build(this char character, int count)
        {
            StringBuilder builder = new StringBuilder(count);
            for (int i = 0; i < count; i++)
            {
                builder.Append(character);
            }
            return builder.ToString();
        }
    }
}
