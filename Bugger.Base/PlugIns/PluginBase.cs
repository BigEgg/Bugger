using Bugger.Base.PlugIns;
using System;

namespace Bugger.Base.Plugins
{
    public abstract class PlugInBase : IPlugIn
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="PlugInBase"/> class.
        /// </summary>
        /// <param name="uniqueName">The Plug-in's unique name.</param>
        /// <param name="name">The Plug-in name.</param>
        /// <param name="description">The description of the Plug-in.</param>
        /// <exception cref="System.ArgumentNullException">name</exception>
        /// <exception cref="System.ArgumentException">name</exception>
        public PlugInBase(Guid uniqueName, string name, string description)
            : this(uniqueName, name, description, null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlugInBase"/> class.
        /// </summary>
        /// <param name="uniqueName">The Plug-in's unique name.</param>
        /// <param name="name">The Plug-in name.</param>
        /// <param name="description">The description of the Plug-in.</param>
        /// <param name="minimumSupportBuggerVersion">The Plug-in's minimum support bugger version.</param>
        /// <exception cref="System.ArgumentNullException">name</exception>
        /// <exception cref="System.ArgumentException">name</exception>
        public PlugInBase(Guid uniqueName, string name, string description, Version minimumSupportBuggerVersion)
            : this(uniqueName, name, description, minimumSupportBuggerVersion, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlugInBase"/> class.
        /// </summary>
        /// <param name="uniqueName">The Plug-in's unique name.</param>
        /// <param name="name">The Plug-in name.</param>
        /// <param name="description">The description of the Plug-in.</param>
        /// <param name="minimumSupportBuggerVersion">The Plug-in's minimum support bugger version.</param>
        /// <param name="maximumSupportBuggerVersion">The Plug-in's maximum support bugger version.</param>
        /// <exception cref="System.ArgumentNullException">name</exception>
        /// <exception cref="System.ArgumentException">name</exception>
        public PlugInBase(Guid uniqueName, string name, string description, Version minimumSupportBuggerVersion, Version maximumSupportBuggerVersion)
        {
            if (uniqueName == null) { throw new ArgumentNullException("name"); }
            if (string.IsNullOrWhiteSpace(name)) { throw new ArgumentException("name"); }

            this.UniqueName = uniqueName;
            this.Name = name;
            this.Description = description;
            //  TO DO: Get the version.
            //this.Version = Assembly.GetAssembly()
            this.MinimumSupportBuggerVersion = minimumSupportBuggerVersion;
            this.MaximumSupportBuggerVersion = maximumSupportBuggerVersion;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the Plug-in's unique name.
        /// </summary>
        /// <value>
        /// The Plug-in's unique name.
        /// </value>
        public Guid UniqueName { get; private set; }

        /// <summary>
        /// Gets the Plug-in name.
        /// </summary>
        /// <value>
        /// The Plug-in name.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the description of the Plug-in.
        /// </summary>
        /// <value>
        /// The description of the Plug-in.
        /// </value>
        public string Description { get; private set; }

        /// <summary>
        /// Gets the Plug-in's version.
        /// </summary>
        /// <value>
        /// The Plug-in's version.
        /// </value>
        public Version Version { get; private set; }

        /// <summary>
        /// Gets the Plug-in's minimum support bugger version.
        /// </summary>
        /// <value>
        /// The Plug-in's minimum support bugger version.
        /// </value>
        public Version MinimumSupportBuggerVersion { get; private set; }

        /// <summary>
        /// Gets the Plug-in's  maximum support bugger version.
        /// </summary>
        /// <value>
        /// The Plug-in's maximum support bugger version.
        /// </value>
        public Version MaximumSupportBuggerVersion { get; private set; }

        /// <summary>
        /// Get the flag of the Initialization of the Plug-in.
        /// </summary>
        public bool IsInitialized { get; private set; }

        #region Setting View Model
        /// <summary>
        /// Gets the Plug-in setting view model.
        /// </summary>
        /// <value>
        /// The Plug-in setting view model.
        /// </value>
        public abstract ProxySettingViewModel<IPlugInSettingView> SettingViewModel { get; }
        #endregion
        #endregion

        #region Plug-in Methods
        /// <summary>
        /// Determines whether the specified bugger version is support.
        /// </summary>
        /// <param name="buggerVersion">The bugger version.</param>
        /// <returns><c>True</c> if the specified Bugger version can support this Plug-in, otherwise, <c>false</c>.</returns>
        public bool isSupport(Version buggerVersion)
        {
            if ((MinimumSupportBuggerVersion != null && buggerVersion < MinimumSupportBuggerVersion) ||
                (MaximumSupportBuggerVersion != null && buggerVersion <= MaximumSupportBuggerVersion))
            {
                return false;
            }

            return true;
        }


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
                IsInitialized = false;
                throw;
            }
        }

        protected abstract void OnInitialize();
        #endregion
    }
}
