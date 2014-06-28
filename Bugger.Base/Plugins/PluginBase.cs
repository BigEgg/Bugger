using BigEgg.Framework.Applications.ViewModels;
using System;

namespace Bugger.Plugins
{
    public abstract class PluginBase : DataModel, IPlugin
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PluginBase"/> class.
        /// </summary>
        /// <param name="uniqueName">The unique name of this plug-in.</param>
        /// <param name="pluginName">The name of this plug-in.</param>
        /// <param name="description">The description of this plug-in.</param>
        /// <param name="category">The category of this plug-in.</param>
        /// <param name="minimumApplicationVersion">The application's minimum version that this plug-in support.</param>
        /// <exception cref="System.ArgumentNullException">
        /// uniqueName cannot be null or empty.
        /// or
        /// pluginName cannot be null or empty.
        /// or
        /// minimumApplicationVersion cannot be null.
        /// </exception>
        /// <exception cref="System.ArgumentException">min version cannot larger than max version</exception>
        public PluginBase(string uniqueName,
                          string pluginName,
                          string description,
                          PluginCategory category,
                          Version minimumApplicationVersion)
            : this(uniqueName, pluginName, description, category, minimumApplicationVersion, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginBase" /> class.
        /// </summary>
        /// <param name="uniqueName">The unique name of this plug-in.</param>
        /// <param name="pluginName">The name of this plug-in.</param>
        /// <param name="description">The description of this plug-in.</param>
        /// <param name="category">The category of this plug-in.</param>
        /// <param name="minimumApplicationVersion">The application's minimum version that this plug-in support.</param>
        /// <param name="maximumApplicationVersion">The maximum application version.</param>
        /// <exception cref="System.ArgumentNullException">
        /// uniqueName cannot be null or empty.
        /// or
        /// pluginName cannot be null or empty.
        /// or
        /// minimumApplicationVersion cannot be null.
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

            if (maximumApplicationVersion != null)
            {
                if (minimumApplicationVersion > maximumApplicationVersion)
                {
                    throw new ArgumentException("min version cannot larger than max version");
                }
            }

            this.UniqueName = uniqueName;
            this.PluginName = pluginName;
            this.Description = description;
            this.Category = category;
            this.MinimumApplicationVersion = minimumApplicationVersion;
            this.MaximumApplicationVersion = maximumApplicationVersion;

            this.IsInitialized = false;
        }


        #region Properties
        /// <summary>
        /// Gets the unique name of the plug-in.
        /// </summary>
        /// <value>
        /// The unique name of the plug-in.
        /// </value>
        public string UniqueName { get; private set; }

        /// <summary>
        /// Gets the name of the plug-in.
        /// </summary>
        /// <value>
        /// The name of the plug-in.
        /// </value>
        public string PluginName { get; private set; }

        /// <summary>
        /// Gets the description of the plug-in.
        /// </summary>
        /// <value>
        /// The description of the plug-in.
        /// </value>
        public string Description { get; private set; }

        /// <summary>
        /// Gets the category of the plug-in.
        /// </summary>
        /// <value>
        /// The category of the plug-in.
        /// </value>
        public PluginCategory Category { get; private set; }

        /// <summary>
        /// Gets the application's minimum version that the plug-in support.
        /// </summary>
        /// <value>
        /// The application's minimum version that the plug-in support.
        /// </value>
        public Version MinimumApplicationVersion { get; private set; }

        /// <summary>
        /// Gets the application's maximum version that the plug-in support.
        /// </summary>
        /// <value>
        /// The application's maximum version that the plug-in support.
        /// </value>
        public Version MaximumApplicationVersion { get; private set; }
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
