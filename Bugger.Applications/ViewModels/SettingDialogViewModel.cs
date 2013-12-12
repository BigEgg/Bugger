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
using System.Threading.Tasks;
using System.Threading;

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
        private SettingSubmitStatus submitStatus;
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

        public SettingSubmitStatus SubmitStatus
        {
            get { return this.submitStatus; }
            private set
            {
                if (this.submitStatus != value)
                {
                    this.submitStatus = value;
                    RaisePropertyChanged("SubmitStatus");
                }
            }
        }
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

        #region Command Methods
        private void SubmitSettingCommand()
        {
            if (this.settingActiveProxy != null)
            {
                if (this.SubmitStatus == SettingSubmitStatus.ProxyBusy ||
                    this.SubmitStatus == SettingSubmitStatus.ProxyCannotConnect ||
                    this.SubmitStatus == SettingSubmitStatus.ProxyUnvalid)
                {
                    this.settingActiveProxy.AfterCloseSettingDialog(true);
                    this.proxyService.ActiveProxy = this.settingActiveProxy;
                    Close(true);
                }

                Task.Factory.StartNew(() =>
                {
                    this.SubmitStatus = SettingSubmitStatus.ValidatingProxySettings;
                    UpdateCommands();
                    var validateResult = this.settingActiveProxy.ValidateBeforeCloseSettingDialog();

                    switch (validateResult)
                    {
                        case SettingDialogValidateionResult.Busy:
                            this.SubmitStatus = SettingSubmitStatus.ProxyBusy;
                            UpdateCommands();
                            break;
                        case SettingDialogValidateionResult.ConnectFailed:
                            this.SubmitStatus = SettingSubmitStatus.ProxyCannotConnect;
                            UpdateCommands();
                            break;
                        case SettingDialogValidateionResult.UnValid:
                            this.SubmitStatus = SettingSubmitStatus.ProxyUnvalid;
                            UpdateCommands();
                            break;
                        case SettingDialogValidateionResult.Valid:
                            this.SubmitStatus = SettingSubmitStatus.ProxyValid;
                            this.settingActiveProxy.AfterCloseSettingDialog(true);
                            this.proxyService.ActiveProxy = this.settingActiveProxy;
                            Close(true);
                            break;
                    }
                }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private bool CanSubmitSetting()
        {
            return this.SubmitStatus != SettingSubmitStatus.ValidatingProxySettings &&
                   this.SubmitStatus != SettingSubmitStatus.InitiatingProxy &&
                   this.SubmitStatus != SettingSubmitStatus.InitiatingProxyFailed &&
                   string.IsNullOrEmpty(this.settingsViewModel.Validate());
        }

        private void UpdateCommands()
        {
            this.submitCommand.RaiseCanExecuteChanged();
        }
        #endregion

        #region Private Methods
        private void SettingsViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.SubmitStatus = SettingSubmitStatus.NotWorking;

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

                var newActiveProxy = this.proxyService.Proxies.First(x => x.ProxyName == settingsViewModel.ActiveProxy);
                Task.Factory.StartNew(() =>
                {
                    this.SubmitStatus = SettingSubmitStatus.InitiatingProxy;
                    UpdateCommands();

                    newActiveProxy.Initialize();
                    this.settingActiveProxy = newActiveProxy;
                    this.SubmitStatus = SettingSubmitStatus.NotWorking;

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
                }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext())
                .ContinueWith((result) =>
                {
                    this.SubmitStatus = SettingSubmitStatus.InitiatingProxyFailed;
                }, CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
            }

            UpdateCommands();
        }

        private void ActiveProxyPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.SubmitStatus = SettingSubmitStatus.NotWorking;

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
        #endregion
        #endregion
    }
}
