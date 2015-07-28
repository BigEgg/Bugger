using System;

namespace Bugger.PlugIns
{
    /// <summary>
    /// The base class for Plug-in
    /// </summary>
    public abstract class PlugInBase : IPlugIn
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlugInBase" /> class.
        /// </summary>
        /// <param name="uniqueId">The Plug-in's unique id.</param>
        /// <param name="plugInType">Type of the Plug-in.</param>
        /// <exception cref="System.ArgumentNullException">uniqueId</exception>
        public PlugInBase(Guid uniqueId, PlugInType plugInType)
        {
            Guid = uniqueId;
            PlugInType = plugInType;

            IsInitialized = false;
        }


        /// <summary>
        /// Gets the Plug-in's unique id.
        /// </summary>
        /// <value>
        /// The Plug-in's unique id.
        /// </value>
        public Guid Guid { get; private set; }

        /// <summary>
        /// Gets the type of the Plug-in.
        /// </summary>
        /// <value>
        /// The type of the Plug-in.
        /// </value>
        public PlugInType PlugInType { get; private set; }

        /// <summary>
        /// Get the flag of the Initialization of the Plug-in.
        /// </summary>
        public bool IsInitialized { get; private set; }


        /// <summary>
        /// Gets the Plug-in setting view model when open setting dialog.
        /// </summary>
        /// <value>
        /// The Plug-in setting view model.
        /// </value>
        public abstract PlugInSettingDialogViewModel<IPlugInSettingDialogView> OpenSettingDialog();

        /// <summary>
        /// Initializes the Plug-in.
        /// </summary>
        public void Initialize()
        {
            try
            {
                if (IsInitialized)
                    return;

                OnInitialize();
                IsInitialized = true;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the Plug-in's shared data.
        /// </summary>
        /// <returns>
        /// The Plug-in's shared data.
        /// </returns>
        public virtual IPlugInSharedData GetSharedData()
        {
            return new EmptyPlugInSharedData(Guid);
        }


        /// <summary>
        /// The core function to initializes the Plug-in.
        /// </summary>
        protected virtual void OnInitialize() { }
    }
}
