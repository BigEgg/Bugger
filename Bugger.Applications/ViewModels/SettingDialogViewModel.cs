using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using BigEgg.Framework.Applications.Commands;
using BigEgg.Framework.Applications.Services;
using BigEgg.Framework.Applications.ViewModels;
using BigEgg.Framework.Foundation;
using Bugger.Applications.Models;
using Bugger.Applications.Properties;
using Bugger.Applications.Services;
using Bugger.Applications.Views;
using Bugger.Proxy;

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
        private SettingDialogStatus settingDialogStatus;
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

        public SettingDialogStatus SettingDialogStatus
        {
            get { return this.settingDialogStatus; }
            set
            {
                if (this.settingDialogStatus != value)
                {
                    this.settingDialogStatus = value;
                    RaisePropertyChanged("SettingDialogStatus");
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
            if (this.SettingDialogStatus == SettingDialogStatus.ProxyBusy ||
                this.SettingDialogStatus == SettingDialogStatus.ProxyCannotConnect ||
                this.SettingDialogStatus == SettingDialogStatus.ProxyUnvalid)
            {
                this.settingActiveProxy.AfterCloseSettingDialog(true);
                this.proxyService.ActiveProxy = this.settingActiveProxy;
                Close(true);
            }

            this.SettingDialogStatus = SettingDialogStatus.ValidatingProxySettings;
            UpdateCommands();
            Task.Factory.StartNew(() =>
            {
                return this.settingActiveProxy.ValidateBeforeCloseSettingDialog();
            })
            .ContinueWith(task =>
            {
                switch (task.Result)
                {
                    case SettingDialogValidateionResult.Busy:
                        this.SettingDialogStatus = SettingDialogStatus.ProxyBusy;
                        return false;
                    case SettingDialogValidateionResult.ConnectFailed:
                        this.SettingDialogStatus = SettingDialogStatus.ProxyCannotConnect;
                        return false;
                    case SettingDialogValidateionResult.UnValid:
                        this.SettingDialogStatus = SettingDialogStatus.ProxyUnvalid;
                        return false;
                    case SettingDialogValidateionResult.Valid:
                        this.SettingDialogStatus = SettingDialogStatus.ProxyValid;
                        return true;
                }
                return false;
            }, CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.FromCurrentSynchronizationContext())
            .ContinueWith(task =>
            {
                if (task.Result)
                {
                    this.settingActiveProxy.AfterCloseSettingDialog(true);
                    this.proxyService.ActiveProxy = this.settingActiveProxy;
                    Close(true);
                }
                else
                {
                    UpdateCommands();
                }
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        private bool CanSubmitSetting()
        {
            return this.SettingDialogStatus != SettingDialogStatus.ValidatingProxySettings &&
                   this.SettingDialogStatus != SettingDialogStatus.InitiatingProxy &&
                   this.SettingDialogStatus != SettingDialogStatus.InitiatingProxyFailed &&
                   string.IsNullOrEmpty(this.settingsViewModel.Validate()) &&
                   this.settingActiveProxy != null;
        }

        private void UpdateCommands()
        {
            this.submitCommand.RaiseCanExecuteChanged();
        }
        #endregion

        #region Private Methods
        private void SettingsViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.SettingDialogStatus = SettingDialogStatus.NotWorking;

            if (e.PropertyName == "ActiveProxy")
            {
                var newActiveProxy = this.proxyService.Proxies.First(x => x.ProxyName == settingsViewModel.ActiveProxy);
                if (newActiveProxy == null) { return; }

                this.SettingDialogStatus = SettingDialogStatus.InitiatingProxy;
                UpdateCommands();

                var InitializationTask = Task.Factory.StartNew(() =>
                {
                    newActiveProxy.Initialize();
                });

                InitializationTask.ContinueWith(task =>
                {
                    this.SettingDialogStatus = SettingDialogStatus.InitiatingProxyFailed;
                }, CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());

                InitializationTask.ContinueWith(task =>
                {
                    this.SettingDialogStatus = SettingDialogStatus.NotWorking;
                }, CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.FromCurrentSynchronizationContext())
                .ContinueWith(task =>
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

                    this.settingActiveProxy = newActiveProxy;
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
                }, TaskContinuationOptions.OnlyOnRanToCompletion);
            }

            UpdateCommands();
        }

        private void ActiveProxyPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.SettingDialogStatus = SettingDialogStatus.NotWorking;

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
