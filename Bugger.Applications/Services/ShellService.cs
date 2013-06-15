using BigEgg.Framework.Applications.ViewModels;
using System.ComponentModel.Composition;

namespace Bugger.Applications.Services
{
    [Export(typeof(IShellService)), Export]
    internal class ShellService : DataModel, IShellService
    {
        private object mainView;
        private object userBugsView;
        private object teamBugsView;

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
