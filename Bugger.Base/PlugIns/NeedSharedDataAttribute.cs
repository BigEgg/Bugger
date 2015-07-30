using System;

namespace Bugger.PlugIns
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class NeedSharedDataAttribute : Attribute
    {
        public NeedSharedDataAttribute()
        { }
    }
}
