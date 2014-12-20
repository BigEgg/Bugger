using System;

namespace Bugger.Base.Plugins
{
    public interface IPlugIn
    {
        /// <summary>
        /// Gets the Plug-in's unique name.
        /// </summary>
        /// <value>
        /// The Plug-in's unique name.
        /// </value>
        Guid UniqueName { get; }

        /// <summary>
        /// Gets the Plug-in name.
        /// </summary>
        /// <value>
        /// The Plug-in name.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Gets the description of the Plug-in.
        /// </summary>
        /// <value>
        /// The description of the Plug-in.
        /// </value>
        string Description { get; }

        /// <summary>
        /// Gets the Plug-in's version.
        /// </summary>
        /// <value>
        /// The Plug-in's version.
        /// </value>
        Version Version { get; }

        /// <summary>
        /// Gets the Plug-in's minimum support bugger version.
        /// </summary>
        /// <value>
        /// The Plug-in's minimum support bugger version.
        /// </value>
        Version MinimumSupportBuggerVersion { get; }

        /// <summary>
        /// Gets the Plug-in's  maximum support bugger version.
        /// </summary>
        /// <value>
        /// The Plug-in's maximum support bugger version.
        /// </value>
        Version MaximumSupportBuggerVersion { get; }

        /// <summary>
        /// Determines whether the specified bugger version is support.
        /// </summary>
        /// <param name="buggerVersion">The bugger version.</param>
        /// <returns><c>True</c> if the specified Bugger version can support this Plug-in, otherwise, <c>false</c>.</returns>
        bool isSupport(Version buggerVersion);
    }
}
