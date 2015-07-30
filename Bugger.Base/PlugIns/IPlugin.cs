using System;
using System.Collections.Generic;

namespace Bugger.PlugIns
{
    /// <summary>
    /// The Plug-in interface
    /// </summary>
    public interface IPlugIn
    {
        /// <summary>
        /// Gets the Plug-in's unique id.
        /// </summary>
        /// <value>
        /// The Plug-in's unique id.
        /// </value>
        Guid Guid { get; }

        /// <summary>
        /// Gets the type of the Plug-in.
        /// </summary>
        /// <value>
        /// The type of the Plug-in.
        /// </value>
        PlugInType PlugInType { get; }

        /// <summary>
        /// Get the flag of the Initialization of the Plug-in.
        /// </summary>
        bool IsInitialized { get; }


        /// <summary>
        /// Gets the Plug-in setting view model when open setting dialog.
        /// </summary>
        /// <value>
        /// The Plug-in setting view model.
        /// </value>
        PlugInSettingDialogViewModel<IPlugInSettingDialogView> OpenSettingDialog();

        /// <summary>
        /// Initializes the Plug-in.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Gets the Plug-in's shared data.
        /// </summary>
        /// <returns>The Plug-in's shared data.</returns>
        IPlugInSharedData GetSharedData();

        /// <summary>
        /// Sets the environment's shared data.
        /// </summary>
        /// <param name="data">The data.</param>
        void SetSharedData(IEnumerable<IPlugInSharedData> data);
    }
}
