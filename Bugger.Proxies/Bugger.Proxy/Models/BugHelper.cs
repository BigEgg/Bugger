using Bugger.Proxy.Models.Attributes;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Bugger.Proxy.Models
{
    public class BugHelper
    {
        private static IList<string> propertyNamesCache;


        /// <summary>
        /// Gets the property names of the IBug interface.
        /// </summary>
        /// <returns>The property names</returns>
        public static IList<string> GetPropertyNames()
        {
            GetNamesIfNotGet();

            return propertyNamesCache;
        }

        /// <summary>
        /// Gets the new property mapping dictionary of the IBug interface.
        /// </summary>
        /// <param name="bug">The bug interface.</param>
        /// <returns>The new property mapping dictionary.</returns>
        public static PropertyMappingDictionary GetPropertyMappingDictionary()
        {
            GetNamesIfNotGet();

            var result = new PropertyMappingDictionary();
            foreach (var name in propertyNamesCache)
            {
                result.Add(name, string.Empty);
            }
            return result;
        }


        /// <summary>
        /// Gets the property names if not get.
        /// </summary>
        private static void GetNamesIfNotGet()
        {
            if (propertyNamesCache == null)
            {
                IgnoreMappingAttribute ignore = new IgnoreMappingAttribute() { Ignore = true };
                propertyNamesCache = TypeDescriptor.GetProperties(typeof(Bug))
                                                   .Cast<PropertyDescriptor>()
                                                   .Where(propertyInfo => !propertyInfo.Attributes.Contains(ignore))
                                                   .Select(x => x.Name)
                                                   .ToList();
            }
        }
    }
}
