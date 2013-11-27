using BigEgg.Framework.Applications.Commands;
using BigEgg.Framework.Applications.ViewModels;
using BigEgg.Framework.Foundation;
using Bugger.Applications.Models;
using Bugger.Applications.Properties;
using Bugger.Applications.Services;
using Bugger.Applications.Views;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Bugger.Proxy;
using BigEgg.Framework.Applications.Services;
using System.Globalization;

namespace Bugger.Applications.ViewModels
{
    public class SettingDialogViewModel : DialogViewModel<ISettingDialogView>
    {
        #region Fields
        private readonly IProxyService proxyService;
        private readonly IMessageService messageService;
        private readonly DelegateCommand submitCommand;
        private readonly DelegateCommand cancelCommand;
        private readonly ObservableCollection<object> views;
        private SettingsViewModel settingsViewModel;

        private ITracingSystemProxy settingActiveProxy;
        #endregion

        public SettingDialogViewModel(ISettingDialogView view, IProxyService proxyService, IMessageService messageService, SettingsViewModel settingsViewModel)
            : base(view)
        {
            this.proxyService = proxyService;
            this.settingsViewModel = settingsViewModel;
            this.messageService = messageService;

            this.submitCommand = new DelegateCommand(SubmitSettingCommand, CanSubmitSetting);
            this.cancelCommand = new DelegateCommand(() => Close(false));

            this.views = new ObservableCollection<object>();
            this.views.Add(this.settingsViewModel.View);
            if (this.proxyService.ActiveProxy != null)
            {
                var settingView = this.proxyService.ActiveProxy.InitializeSettingDialog();
                if (settingView != null)
                {
                    this.views.Add(settingView);
                }
            }
            SelectView = this.settingsViewModel.View;

            AddWeakEventListener(this.settingsViewModel, SettingsViewModelPropertyChanged);

            if (!string.IsNullOrWhiteSpace(this.settingsViewModel.ActiveProxy))
                this.settingActiveProxy = this.proxyService.Proxies.First(x => x.ProxyName == settingsViewModel.ActiveProxy);
            if (this.settingActiveProxy != null)
            {
                AddWeakEventListener(this.settingActiveProxy, ActiveProxyPropertyChanged);
                AddWeakEventListener(this.settingActiveProxy.StateValues, StateValuesCollectionChanged);
                StateValuesCollectionChanged(null, null);
            }
        }

        #region Properties
        public override string Title { get { return Resources.ApplicationName; } }

        public ICommand SubmitCommand { get { return this.submitCommand; } }

        public ICommand CancelCommand { get { return this.cancelCommand; } }

        public ObservableCollection<object> Views { get { return this.views; } }

        public object SelectView { get; set; }
        #endregion

        #region Methods
        #region Public Methods
        public void OnCancelSettings()
        {
            if (this.settingActiveProxy != null)
            {
                this.settingActiveProxy.AfterCloseSettingDialog(false);
            }
        }
        #endregion

        #region Private Methods
        private void SubmitSettingCommand()
        {
            if (this.settingActiveProxy != null)
            {
                var validateResult = this.settingActiveProxy.ValidateBeforeCloseSettingDialog();

                bool result;
                switch (validateResult)
                {
                    case SettingDialogValidateionResult.Busy:
                        messageService.ShowWarning(this.View, string.Format(CultureInfo.CurrentCulture, Resources.ProxySettingBusy));
                        break;
                    case SettingDialogValidateionResult.ConnectFailed:
                        result = messageService.ShowYesNoQuestion(this.View, string.Format(CultureInfo.CurrentCulture, Resources.ProxySettingCannotConnect));
                        if (result)
                        {
                            this.settingActiveProxy.AfterCloseSettingDialog(true);
                            this.proxyService.ActiveProxy = this.settingActiveProxy;
                            Close(true);
                        }
                        break;
                    case SettingDialogValidateionResult.UnValid:
                        result = messageService.ShowYesNoQuestion(this.View, string.Format(CultureInfo.CurrentCulture, Resources.ProxySettingUnValid));
                        if (result)
                        {
                            this.settingActiveProxy.AfterCloseSettingDialog(true);
                            this.proxyService.ActiveProxy = this.settingActiveProxy;
                            Close(true);
                        }
                        break;
                    case SettingDialogValidateionResult.Valid:
                        this.settingActiveProxy.AfterCloseSettingDialog(true);
                        this.proxyService.ActiveProxy = this.settingActiveProxy;
                        Close(true);
                        break;
                }
            }

        }

        private bool CanSubmitSetting()
        {
            return string.IsNullOrEmpty(this.settingsViewModel.Validate());
        }

        private void SettingsViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ActiveProxy")
            {
                if (this.settingActiveProxy != null)
                {
                    RemoveWeakEventListener(this.settingActiveProxy, ActiveProxyPropertyChanged);
                    RemoveWeakEventListener(this.settingActiveProxy.StateValues, StateValuesCollectionChanged);

                    var settingView = this.settingActiveProxy.InitializeSettingDialog();
                    if (settingView != null)
                    {
                        this.views.Remove(settingView);
                    }
                }

                this.settingActiveProxy = this.proxyService.Proxies.First(x => x.ProxyName == settingsViewModel.ActiveProxy);

                StateValuesCollectionChanged(null, null);

                if (this.settingActiveProxy != null)
                {
                    AddWeakEventListener(this.settingActiveProxy, ActiveProxyPropertyChanged);
                    AddWeakEventListener(this.settingActiveProxy.StateValues, StateValuesCollectionChanged);
                    var settingView = this.settingActiveProxy.InitializeSettingDialog();
                    if (settingView != null)
                    {
                        this.views.Add(settingView);
                    }
                }
            }

            UpdateCommands();
        }

        private void ActiveProxyPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateCommands();
        }

        private void StateValuesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.settingsViewModel.StatusValues.Clear();

            foreach (var value in this.settingActiveProxy.StateValues)
            {
                CheckString checkValue = new CheckString(value);
                checkValue.IsChecked = string.IsNullOrWhiteSpace(this.settingsViewModel.FilterStatusValues) ||
                                       Settings.Default.FilterStatusValues.Contains(value);

                AddWeakEventListener(checkValue, StatusValuePropertyChanged);

                this.settingsViewModel.StatusValues.Add(checkValue);
            }

            this.settingsViewModel.FilterStatusValues = string.Join(
                "; ",
                this.settingsViewModel.StatusValues.Where(x => x.IsChecked).Select(x => x.Name));
        }

        private void StatusValuePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.settingsViewModel.FilterStatusValues = string.Join(
                "; ",
                this.settingsViewModel.StatusValues.Where(x => x.IsChecked).Select(x => x.Name));
        }

        private void UpdateCommands()
        {
            this.submitCommand.RaiseCanExecuteChanged();
        }
        #endregion
        #endregion
    }
}
