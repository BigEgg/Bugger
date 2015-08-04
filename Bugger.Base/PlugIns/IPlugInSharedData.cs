using System;

namespace Bugger.PlugIns
{
    /// <summary>
    /// The interface of Plug-in's shared data
    /// </summary>
    public interface IPlugInSharedData
    {
        /// <summary>
        /// Gets the Plug-in's unique id.
        /// </summary>
        /// <value>
        /// The Plug-in's unique id.
        /// </value>
        Guid PlugInGuid { get; }
    }
}
