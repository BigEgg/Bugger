using System;

namespace Bugger.Plugins
{
    public abstract class PluginBase : IPlugin
    {
        #region Fields
        private string uniqueName;
        private string pluginName;
        private string description;
        private PluginCategory category;
        private Version minimumApplicationVersion;
        private Version maximumApplicationVersion;
        #endregion


        /// <summary>
        /// Initializes a new instance of the <see cref="PluginBase"/> class.
        /// </summary>
        /// <param name="uniqueName">The unique name of this plug-in.</param>
        /// <param name="pluginName">The name of this plug-in.</param>
        /// <param name="description">The description of this plug-in.</param>
        /// <param name="category">The category of this plug-in.</param>
        /// <param name="minimumApplicationVersion">The application's minimum version that this plug-in support.</param>
        /// <param name="maximumApplicationVersion">The application's maximum version that this plug-in support.</param>
        /// <exception cref="System.ArgumentNullException">
        /// uniqueName cannot be null or empty.
        /// or
        /// pluginName cannot be null or empty.
        /// </exception>
        /// <exception cref="System.ArgumentException">min version cannot larger than max version</exception>
        public PluginBase(string uniqueName,
                          string pluginName,
                          string description,
                          PluginCategory category,
                          Version minimumApplicationVersion,
                          Version maximumApplicationVersion)
        {
            if (string.IsNullOrWhiteSpace(uniqueName)) { throw new ArgumentNullException("uniqueName cannot be null or empty."); }
            if (string.IsNullOrWhiteSpace(pluginName)) { throw new ArgumentNullException("pluginName cannot be null or empty."); }
            if (minimumApplicationVersion == null) { throw new ArgumentNullException("minimumApplicationVersion cannot be null."); }
            if (maximumApplicationVersion == null) { throw new ArgumentNullException("maximumApplicationVersion cannot be null."); }

            if (minimumApplicationVersion > maximumApplicationVersion) { throw new ArgumentException("min version cannot larger than max version"); }

            this.uniqueName = uniqueName;
            this.pluginName = pluginName;
            this.description = description;
            this.category = category;
            this.minimumApplicationVersion = minimumApplicationVersion;
            this.maximumApplicationVersion = maximumApplicationVersion;

            IsInitialized = false;
        }

        #region Properties
        /// <summary>
        /// Gets the unique name of the plug-in.
        /// </summary>
        /// <value>
        /// The unique name of the plug-in.
        /// </value>
        public string UniqueName
        {
            get { return this.uniqueName; }
        }

        /// <summary>
        /// Gets the name of the plug-in.
        /// </summary>
        /// <value>
        /// The name of the plug-in.
        /// </value>
        public string PluginName
        {
            get { return this.pluginName; }
        }

        /// <summary>
        /// Gets the description of the plug-in.
        /// </summary>
        /// <value>
        /// The description of the plug-in.
        /// </value>
        public string Description
        {
            get { return this.description; }
        }

        /// <summary>
        /// Gets the category of the plug-in.
        /// </summary>
        /// <value>
        /// The category of the plug-in.
        /// </value>
        public PluginCategory Category
        {
            get { return this.category; }
        }

        /// <summary>
        /// Gets the application's minimum version that the plug-in support.
        /// </summary>
        /// <value>
        /// The application's minimum version that the plug-in support.
        /// </value>
        public Version MinimumApplicationVersion
        {
            get { return this.minimumApplicationVersion; }
        }

        /// <summary>
        /// Gets the application's maximum version that the plug-in support.
        /// </summary>
        /// <value>
        /// The application's maximum version that the plug-in support.
        /// </value>
        public Version MaximumApplicationVersion
        {
            get { return this.maximumApplicationVersion; }
        }
        #endregion

        #region Plug-in Initialized
        /// <summary>
        /// The method which will execute when the plug-in.Initialize() execute.
        /// </summary>
        protected virtual void OnInitialize()
        {
        }

        /// <summary>
        /// Initialize the plug-in.
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

        /// <summary>
        /// Get the flag of the Initialization of the plug-in.
        /// </summary>
        public bool IsInitialized { get; private set; }
        #endregion
    }
}
