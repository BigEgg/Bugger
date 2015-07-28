using System;

namespace Bugger.PlugIns
{
    /// <summary>
    /// The empty Plug-in shared data
    /// </summary>
    public class EmptyPlugInSharedData : IPlugInSharedData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyPlugInSharedData" /> class.
        /// </summary>
        /// <param name="uniqueId">The Plug-in's unique id.</param>
        public EmptyPlugInSharedData(Guid uniqueId)
        {
            PlugInGuid = uniqueId;
        }

        /// <summary>
        /// Gets the Plug-in's unique id.
        /// </summary>
        /// <value>
        /// The Plug-in's unique id.
        /// </value>
        public Guid PlugInGuid { get; private set; }
    }
}
