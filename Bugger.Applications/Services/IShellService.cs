using System.ComponentModel;

namespace Bugger.Applications.Services
{
    /// <summary>
    /// The interface that define all the data that related with the views in the application
    /// which the service should contains.
    /// </summary>
    public interface IShellService : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the main view.
        /// </summary>
        /// <value>
        /// The main view.
        /// </value>
        object MainView { get; }

        /// <summary>
        /// Gets or sets the user bugs view.
        /// </summary>
        /// <value>
        /// The user bugs view.
        /// </value>
        object UserBugsView { get; set; }

        /// <summary>
        /// Gets or sets the team bugs view.
        /// </summary>
        /// <value>
        /// The team bugs view.
        /// </value>
        object TeamBugsView { get; set; }
    }
}
