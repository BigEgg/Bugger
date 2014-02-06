using System.Collections.Generic;
using System.ComponentModel;

namespace Bugger.Proxy.TFS.Models
{
    internal static class TFSBugHelper
    {
        private static IList<string> propertyNamesCache;


        /// <summary>
        /// Gets the new property mapping dictionary of the ITFSBug interface.
        /// </summary>
        /// <param name="bug">The bug interface.</param>
        /// <returns>The new property mapping dictionary.</returns>
        public static PropertyMappingDictionary GetPropertyNames()
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
                propertyNamesCache = new List<string>();

                PropertyDescriptorCollection propertyDescriptorCollection = TypeDescriptor.GetProperties(typeof(ITFSBug));
                foreach (PropertyDescriptor propertyDescriptor in propertyDescriptorCollection)
                {
                    propertyNamesCache.Add(propertyDescriptor.Name);
                }
            }
        }
    }
}
