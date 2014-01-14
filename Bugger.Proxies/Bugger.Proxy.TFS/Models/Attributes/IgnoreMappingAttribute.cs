using System;

namespace Bugger.Proxy.TFS.Models.Attributes
{
    /// <summary>
    /// The attribute that indicate whether ignore the property while create the mapping list.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreMappingAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets a value indicating whether ignore the property while create the mapping list.
        /// </summary>
        /// <value>
        ///   <c>true</c> if ignore the property; otherwise, <c>false</c>.
        /// </value>
        public bool Ignore { get; set; }
    }
}
