using BigEgg.Framework.Applications.Views;
using Bugger.Applications.ViewModels;
using Bugger.Applications.Views;
using System;
using System.ComponentModel.Composition;
using System.Windows;

namespace Bugger.Presentation.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Export(typeof(IMainView))]
    public partial class MainWindow : Window, IMainView
    {
        private Lazy<MainViewModel> viewModel;


        public MainWindow()
        {
            InitializeComponent();

            viewModel = new Lazy<MainViewModel>(() => ViewHelper.GetViewModel<MainViewModel>(this));
        }


        public MainViewModel ViewModel { get { return this.viewModel.Value; } }
        
        public new void Show()
        {
            this.Visibility = Visibility.Visible;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!ViewModel.IsShutDown)
            {
                e.Cancel = true;
                this.Visibility = Visibility.Hidden;
            }
        }
    }
}
