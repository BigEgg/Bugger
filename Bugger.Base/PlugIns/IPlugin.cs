using Bugger.Base.PlugIns;
using System;

namespace Bugger.Base.Plugins
{
    public interface IPlugIn
    {
        #region Plug-in Properties
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
        /// Get the flag of the Initialization of the Plug-in.
        /// </summary>
        bool IsInitialized { get; }

        #region Setting View Model
        /// <summary>
        /// Gets the Plug-in setting view model.
        /// </summary>
        /// <value>
        /// The Plug-in setting view model.
        /// </value>
        ProxySettingViewModel<IPlugInSettingView> SettingViewModel { get; }
        #endregion
        #endregion

        #region Plug-in Methods
        /// <summary>
        /// Determines whether the specified bugger version is support.
        /// </summary>
        /// <param name="buggerVersion">The bugger version.</param>
        /// <returns><c>True</c> if the specified Bugger version can support this Plug-in, otherwise, <c>false</c>.</returns>
        bool isSupport(Version buggerVersion);

        /// <summary>
        /// Initializes the Plug-in.
        /// </summary>
        void Initialize();
        #endregion
    }
}
