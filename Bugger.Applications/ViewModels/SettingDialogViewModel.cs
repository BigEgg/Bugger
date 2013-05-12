using BigEgg.Framework.Applications.Commands;
using BigEgg.Framework.Applications.ViewModels;
using BigEgg.Framework.Foundation;
using Bugger.Applications.Properties;
using Bugger.Applications.Services;
using Bugger.Applications.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace Bugger.Applications.ViewModels
{
    public class SettingDialogViewModel : DialogViewModel<ISettingDialogView>
    {
        #region Fields
        private readonly IProxyService proxyService;
        private readonly DelegateCommand submitCommand;
        private readonly DelegateCommand cancelCommand;
        private readonly ObservableCollection<object> views;
        private SettingsViewModel settingsViewModel;
        #endregion

        public SettingDialogViewModel(ISettingDialogView view, IProxyService proxyService, SettingsViewModel settingsViewModel)
            : base(view)
        {
            this.proxyService = proxyService;
            this.settingsViewModel = settingsViewModel;
            this.submitCommand = new DelegateCommand(() => Close(true), CanSubmitSetting);
            this.cancelCommand = new DelegateCommand(() => Close(false));

            this.views = new ObservableCollection<object>();
            this.views.Add(SettingsView);

            AddWeakEventListener(settingsViewModel, SettingsViewModelPropertyChanged);
        }

        #region Properties
        public override string Title { get { return Resources.ApplicationName; } }

        public ICommand SubmitCommand { get { return this.submitCommand; } }

        public ICommand CancelCommand { get { return this.cancelCommand; } }

        public ObservableCollection<object> Views { get { return this.views; } }

        protected IProxyService ProxyService { get { return this.proxyService; } }
        #endregion

        #region Private Properties
        private object SettingsView
        {
            get { return this.settingsViewModel.View; }
        }

        private object ProxySettingView
        {
            get { return this.proxyService.ActiveProxy.SettingView; }
        }
        #endregion

        #region Methods
        #region Private Methods
        private bool CanSubmitSetting()
        {
            return string.IsNullOrEmpty(this.settingsViewModel.Validate()) 
                && this.proxyService.ActiveProxy.CanQuery();
        }

        private void SettingsViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ActiveProxy")
            {
                if (ProxySettingView != null)
                    this.views.Remove(ProxySettingView);

                this.proxyService.ActiveProxy = this.proxyService.Proxys.First(x => x.ProxyName == settingsViewModel.ActiveProxy);

                if (ProxySettingView != null)
                    this.views.Add(ProxySettingView);

                RaisePropertyChanged("Views");
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
