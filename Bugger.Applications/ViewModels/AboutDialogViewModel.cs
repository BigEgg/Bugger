using BigEgg.Framework.Applications;
using BigEgg.Framework.Applications.Commands;
using BigEgg.Framework.Applications.ViewModels;
using Bugger.Applications.Properties;
using Bugger.Applications.Views;
using System.Windows.Input;

namespace Bugger.Applications.ViewModels
{
    public class AboutDialogViewModel : DialogViewModel<IAboutDialogView>
    {
        #region Fields
        private readonly DelegateCommand okCommand;
        #endregion

        public AboutDialogViewModel(IAboutDialogView view)
            : base(view)
        {
            this.okCommand = new DelegateCommand(() => Close(true));
        }

        #region Properties
        public override string Title { get { return Resources.ApplicationName; } }

        public string ProductName { get { return Resources.ApplicationName; } }

        public string Version { get { return ApplicationInfo.Version; } }

        public ICommand OKCommand { get { return this.okCommand; } }
        #endregion
    }
}
