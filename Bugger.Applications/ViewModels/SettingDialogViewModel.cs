using System.ComponentModel;
using System.Windows.Input;
using System.Linq;
using BigEgg.Framework.Applications.Commands;
using BigEgg.Framework.Applications.ViewModels;
using Bugger.Applications.Services;
using Bugger.Applications.Views;
using BigEgg.Framework.Foundation;
using Bugger.Applications.Properties;

namespace Bugger.Applications.ViewModels
{
    public class SettingDialogViewModel : DialogViewModel<ISettingDialogView>
    {
        #region Fields
        private readonly IProxyService proxyService;
        private readonly DelegateCommand submitCommand;
        private readonly DelegateCommand cancelCommand;
        private SettingsViewModel settingsViewModel;
        #endregion

        public SettingDialogViewModel(ISettingDialogView view, IProxyService proxyService, SettingsViewModel settingsViewModel)
            : base(view)
        {
            this.proxyService = proxyService;
            this.settingsViewModel = settingsViewModel;
            this.submitCommand = new DelegateCommand(() => Close(true), CanSubmitSetting);
            this.cancelCommand = new DelegateCommand(() => Close(false));

            AddWeakEventListener(settingsViewModel, SettingsViewModelPropertyChanged);
        }

        #region Properties
        public override string Title { get { return Resources.ApplicationName; } }

        public ICommand SubmitCommand { get { return this.submitCommand; } }

        public ICommand CancelCommand { get { return this.cancelCommand; } }

        public object SettingsView
        {
            get { return this.settingsViewModel.View; }
        }

        public object ProxySettingView
        {
            get { return this.proxyService.ActiveProxy.SettingViewModel.View; }
        }
        #endregion

        #region Methods
        #region Private Methods
        private bool CanSubmitSetting()
        {
            return false;
        }

        private void SettingsViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ActiveProxy")
            {
                this.proxyService.ActiveProxy = this.proxyService.Proxys.First(x => x.ProxyName == settingsViewModel.ActiveProxy);

                RaisePropertyChanged("ProxySettingView");
            }
            UpdateCommands();
        }

        private void UpdateCommands()
        {
            this.submitCommand.RaiseCanExecuteChanged();
        }
        #endregion
        #endregion
    }
}
