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
    public class FloatingViewModel : ViewModel<IFloatingView>
    {
        #region Fields
        private readonly IDataService dataService;

        private ICommand showMainWindowCommand;
        private ICommand englishCommand;
        private ICommand chineseCommand;
        private ICommand aboutCommand;
        private ICommand settingCommand;
        private ICommand exitCommand;
        #endregion

        [ImportingConstructor]
        public FloatingViewModel(IFloatingView view, IDataService dataService, IPresentationService presentationService)
            : base(view)
        {
            this.dataService = dataService;
            view.Closing += ViewClosing;
            view.Closed += ViewClosed;

            // Restore the window size when the values are valid.
            if (Settings.Default.FloatingWindowLeft >= 0 && Settings.Default.FloatingWindowTop >= 0 && Settings.Default.FloatingWindowWidth > 0 && Settings.Default.FloatingWindowHeight > 0
                && Settings.Default.FloatingWindowLeft + Settings.Default.FloatingWindowWidth <= presentationService.VirtualScreenWidth
                && Settings.Default.FloatingWindowTop + Settings.Default.FloatingWindowHeight <= presentationService.VirtualScreenHeight)
            {
                ViewCore.Left = Settings.Default.FloatingWindowLeft;
                ViewCore.Top = Settings.Default.FloatingWindowTop;
                ViewCore.Height = Settings.Default.FloatingWindowHeight;
                ViewCore.Width = Settings.Default.FloatingWindowWidth;
            }
        }

        #region Properties
        public IDataService DataService { get { return this.dataService; } }

        public ICommand ShowMainWindowCommand
        {
            get { return this.showMainWindowCommand; }
            set
            {
                if (this.showMainWindowCommand != value)
                {
                    this.showMainWindowCommand = value;
                    RaisePropertyChanged("ShowMainWindowCommand");
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
            Settings.Default.FloatingWindowLeft = ViewCore.Left;
            Settings.Default.FloatingWindowTop = ViewCore.Top;
            Settings.Default.FloatingWindowHeight = ViewCore.Height;
            Settings.Default.FloatingWindowWidth = ViewCore.Width;
        }
        #endregion
        #endregion
    }
}
