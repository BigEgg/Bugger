using BigEgg.Framework.Applications.Views;
using Bugger.Applications.ViewModels;
using Bugger.Applications.Views;
using System;
using System.ComponentModel.Composition;
using System.Windows;

namespace Bugger.Presentation.Views
{
    /// <summary>
    /// Interaction logic for SettingDialogWindow.xaml
    /// </summary>
    [Export(typeof(ISettingDialogView))]
    public partial class SettingDialogWindow : Window, ISettingDialogView
    {
        private Lazy<SettingDialogViewModel> viewModel;


        public SettingDialogWindow()
        {
            InitializeComponent();

            viewModel = new Lazy<SettingDialogViewModel>(() => ViewHelper.GetViewModel<SettingDialogViewModel>(this));
        }


        public void ShowDialog(object owner)
        {
            Owner = owner as Window;
            this.Visibility = Visibility.Visible;
            ShowDialog();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
            viewModel.Value.OnCancelSettings();
        }
    }
}
