using BigEgg.Framework.Applications.Views;
using Bugger.Proxys.TFS.ViewModels;
using Bugger.Proxys.TFS.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bugger.Proxys.TFS.Presentation.Views
{
    [Export(typeof(ITFSSettingView))]
    public partial class TFSSettingView : UserControl, ITFSSettingView
    {
        private readonly Lazy<TFSSettingViewModel> viewModel;
        
        
        public TFSSettingView()
        {
            InitializeComponent();

            viewModel = new Lazy<TFSSettingViewModel>(() => ViewHelper.GetViewModel<TFSSettingViewModel>(this));
            Loaded += LoadedHandler;
            Unloaded += UnloadedHandler;
        }


        private TFSSettingViewModel ViewModel { get { return viewModel.Value; } }


        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            ViewModel.Settings.PropertyChanged += SettingsPropertyChanged;
            tfsName.Focus();
        }

        private void UnloadedHandler(object sender, RoutedEventArgs e)
        {
            ViewModel.Settings.PropertyChanged -= SettingsPropertyChanged;
        }

        private void SettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Password")
            {
                password.Password = ViewModel.Settings.Password;
            }
        }

        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = (PasswordBox)sender;
            ViewModel.Settings.Password = passwordBox.Password;
        }
    }
}
