using System;

namespace Bugger.PlugIns
{
    /// <summary>
    /// The attribute to indicate where that methods need environment's shared data
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class NeedSharedDataAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NeedSharedDataAttribute"/> class.
        /// </summary>
        public NeedSharedDataAttribute()
        { }
    }
}
