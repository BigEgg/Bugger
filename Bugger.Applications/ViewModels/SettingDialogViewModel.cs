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
            this.submitCommand = new DelegateCommand(SubmitSettingCommand, CanSubmitSetting);
            this.cancelCommand = new DelegateCommand(() => Close(false));

            this.views = new ObservableCollection<object>();
            this.views.Add(this.settingsViewModel.View);
            if (this.proxyService.ActiveProxy != null && this.proxyService.ActiveProxy.SettingView != null)
                this.views.Add(this.proxyService.ActiveProxy.SettingView);

            SelectView = this.settingsViewModel.View;

            AddWeakEventListener(settingsViewModel, SettingsViewModelPropertyChanged);
        }

        #region Properties
        public override string Title { get { return Resources.ApplicationName; } }

        public ICommand SubmitCommand { get { return this.submitCommand; } }

        public ICommand CancelCommand { get { return this.cancelCommand; } }

        public ObservableCollection<object> Views { get { return this.views; } }

        public object SelectView { get; set; }
        #endregion

        #region Methods
        #region Private Methods
        private void SubmitSettingCommand()
        {
            if (this.proxyService.ActiveProxy != null)
                this.proxyService.ActiveProxy.SaveSettings();
            
            Close(true);
        }

        private bool CanSubmitSetting()
        {
            return string.IsNullOrEmpty(this.settingsViewModel.Validate()) 
                && this.proxyService.ActiveProxy.CanQuery;
        }

        private void SettingsViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ActiveProxy")
            {
                if (this.proxyService.ActiveProxy != null && this.proxyService.ActiveProxy.SettingView != null)
                    this.views.Remove(this.proxyService.ActiveProxy.SettingView);

                this.proxyService.ActiveProxy = this.proxyService.Proxys.First(x => x.ProxyName == settingsViewModel.ActiveProxy);

                if (this.proxyService.ActiveProxy != null && this.proxyService.ActiveProxy.SettingView != null)
                    this.views.Add(this.proxyService.ActiveProxy.SettingView);

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
