using BigEgg.Framework.Applications.ViewModels;
using System.ComponentModel.Composition;

namespace Bugger.Applications.Services
{
    /// <summary>
    /// The service class that contains all the data that related with the views in the application.
    /// </summary>
    [Export(typeof(IShellService)), Export]
    internal class ShellService : DataModel, IShellService
    {
        private object mainView;
        private object userBugsView;
        private object teamBugsView;

        /// <summary>
        /// Gets the main view.
        /// </summary>
        /// <value>
        /// The main view.
        /// </value>
        public object MainView
        {
            get { return this.mainView; }
            set
            {
                if (this.mainView != value)
                {
                    this.mainView = value;
                    RaisePropertyChanged("MainView");
                }
            }
        }

        /// <summary>
        /// Gets or sets the user bugs view.
        /// </summary>
        /// <value>
        /// The user bugs view.
        /// </value>
        public object UserBugsView
        {
            get { return this.userBugsView; }
            set
            {
                if (this.userBugsView != value)
                {
                    this.userBugsView = value;
                    RaisePropertyChanged("UserBugsView");
                }
            }
        }

        /// <summary>
        /// Gets or sets the team bugs view.
        /// </summary>
        /// <value>
        /// The team bugs view.
        /// </value>
        public object TeamBugsView
        {
            get { return this.teamBugsView; }
            set
            {
                if (this.teamBugsView != value)
                {
                    this.teamBugsView = value;
                    RaisePropertyChanged("TeamBugsView");
                }
            }
        }
    }
}
