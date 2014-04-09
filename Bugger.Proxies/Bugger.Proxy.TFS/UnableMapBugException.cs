using System;

namespace Bugger.Proxy.TFS
{
    public class UnableMapBugException : Exception
    {
        public UnableMapBugException(string message)
            : base(message)
        {
        }
    }
}
