using BigEgg.Framework.Applications.ViewModels;
using Bugger.Applications.Properties;
using Bugger.Applications.Services;
using Bugger.Applications.Views;
using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows.Input;

namespace Bugger.Applications.ViewModels
{
    [Export]
    public class MainViewModel : ViewModel<IMainView>
    {
        #region Fields
        private readonly IDataService dataService;
        private readonly IShellService shellService;

        private ICommand refreshBugsCommand;
        private ICommand englishCommand;
        private ICommand chineseCommand;
        private ICommand aboutCommand;
        private ICommand settingCommand;
        private ICommand exitCommand;
        #endregion

        [ImportingConstructor]
        public MainViewModel(IMainView view, IDataService dataService, IShellService shellService, IPresentationService presentationService)
            : base(view)
        {
            this.shellService = shellService;
            this.dataService = dataService;
            view.Closing += ViewClosing;
            view.Closed += ViewClosed;

            // Restore the window size when the values are valid.
            if (Settings.Default.MainWindowLeft >= 0 && Settings.Default.MainWindowTop >= 0 && Settings.Default.MainWindowWidth > 0 && Settings.Default.MainWindowHeight > 0
                && Settings.Default.MainWindowLeft + Settings.Default.MainWindowWidth <= presentationService.VirtualScreenWidth
                && Settings.Default.MainWindowTop + Settings.Default.MainWindowHeight <= presentationService.VirtualScreenHeight)
            {
                ViewCore.Left = Settings.Default.MainWindowLeft;
                ViewCore.Top = Settings.Default.MainWindowTop;
                ViewCore.Height = Settings.Default.MainWindowHeight;
                ViewCore.Width = Settings.Default.MainWindowWidth;
            }

            this.IsShutDown = false;
        }

        #region Properties
        public IDataService DataService { get { return this.dataService; } }

        public string Title { get { return Resources.ApplicationName; } }

        public bool IsShutDown { get; internal set; }

        public IShellService ShellService { get { return this.shellService; } }

        public DateTime RefreshTime { get { return this.dataService.RefreshTime; } }

        public ICommand RefreshBugsCommand
        {
            get { return this.refreshBugsCommand; }
            set
            {
                if (this.refreshBugsCommand != value)
                {
                    this.refreshBugsCommand = value;
                    RaisePropertyChanged("RefreshBugsCommand");
                }
            }
        }

        public ICommand ExitCommand
        {
            get { return this.exitCommand; }
            set
            {
                if (this.exitCommand != value)
                {
                    this.exitCommand = value;
                    RaisePropertyChanged("ExitCommand");
                }
            }
        }

        public ICommand SettingCommand
        {
            get { return this.settingCommand; }
            set
            {
                if (this.settingCommand != value)
                {
                    this.settingCommand = value;
                    RaisePropertyChanged("SettingCommand");
                }
            }
        }

        public ICommand AboutCommand
        {
            get { return this.aboutCommand; }
            set
            {
                if (this.aboutCommand != value)
                {
                    this.aboutCommand = value;
                    RaisePropertyChanged("AboutCommand");
                }
            }
        }

        public ICommand EnglishCommand
        {
            get { return this.englishCommand; }
            set
            {
                if (this.englishCommand != value)
                {
                    this.englishCommand = value;
                    RaisePropertyChanged("EnglishCommand");
                }
            }
        }

        public ICommand ChineseCommand
        {
            get { return this.chineseCommand; }
            set
            {
                if (this.chineseCommand != value)
                {
                    this.chineseCommand = value;
                    RaisePropertyChanged("ChineseCommand");
                }
            }
        }
        #endregion

        public event CancelEventHandler Closing;

        #region Methods
        #region Private Methods
        public void Show()
        {
            ViewCore.Show();
        }

        public void Close()
        {
            ViewCore.Close();
        }

        protected virtual void OnClosing(CancelEventArgs e)
        {
            if (Closing != null) { Closing(this, e); }
        }

        private void ViewClosing(object sender, CancelEventArgs e)
        {
            OnClosing(e);
        }

        private void ViewClosed(object sender, EventArgs e)
        {
            Settings.Default.MainWindowLeft = ViewCore.Left;
            Settings.Default.MainWindowTop = ViewCore.Top;
            Settings.Default.MainWindowHeight = ViewCore.Height;
            Settings.Default.MainWindowWidth = ViewCore.Width;
        }
        #endregion
        #endregion
    }
}
