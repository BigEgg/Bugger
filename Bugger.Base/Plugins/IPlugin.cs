using System;

namespace Bugger.Plugins
{
    /// <summary>
    /// The interface of the plug-in of the application.
    /// </summary>
    public interface IPlugin
    {
        #region Properties
        /// <summary>
        /// Gets the unique name of the plug-in.
        /// </summary>
        /// <value>
        /// The unique name of the plug-in.
        /// </value>
        string UniqueName { get; }

        /// <summary>
        /// Gets the name of the plug-in.
        /// </summary>
        /// <value>
        /// The name of the plug-in.
        /// </value>
        string PluginName { get; }

        /// <summary>
        /// Gets the description of the plug-in.
        /// </summary>
        /// <value>
        /// The description of the plug-in.
        /// </value>
        string Description { get; }

        /// <summary>
        /// Gets the category of the plug-in.
        /// </summary>
        /// <value>
        /// The category of the plug-in.
        /// </value>
        PluginCategory Category { get; }

        /// <summary>
        /// Gets the application's minimum version that the plug-in support.
        /// </summary>
        /// <value>
        /// The application's minimum version that the plug-in support.
        /// </value>
        Version MinimumApplicationVersion { get; }

        /// <summary>
        /// Gets the application's maximum version that the plug-in support.
        /// </summary>
        /// <value>
        /// The application's maximum version that the plug-in support.
        /// </value>
        Version MaximumApplicationVersion { get; }
        #endregion

        /// <summary>
        /// Initializes the plug-in.
        /// </summary>
        void Initialize();
    }
}
