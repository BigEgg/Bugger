using BigEgg.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bugger.PlugIns
{
    /// <summary>
    /// The base class for Plug-in
    /// </summary>
    public abstract class PlugInBase : IPlugIn
    {
        /// <summary>
        /// The environment's shared data
        /// </summary>
        protected IDictionary<Guid, IPlugInSharedData> environmentSharedData;


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
            environmentSharedData = new Dictionary<Guid, IPlugInSharedData>();
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
        /// Gets the Plug-in setting view model when open setting.
        /// </summary>
        /// <value>
        /// The Plug-in setting view model.
        /// </value>
        public abstract PlugInSettingViewModel<IPlugInSettingView> GetSettingViewModel();

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
        /// Sets the environment's shared data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <exception cref="ArgumentNullException">data</exception>
        public virtual void SetSharedData(IEnumerable<IPlugInSharedData> data)
        {
            Preconditions.NotNull(data);

            environmentSharedData = data.ToDictionary(sharedData => sharedData.PlugInGuid, sharedData => sharedData);
        }


        /// <summary>
        /// The core function to initializes the Plug-in.
        /// </summary>
        protected virtual void OnInitialize() { }
    }
}
