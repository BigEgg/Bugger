using BigEgg.Framework.Applications.Views;
using Bugger.Applications.ViewModels;
using Bugger.Applications.Views;
using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;

namespace Bugger.Presentation.Views
{
    /// <summary>
    /// Interaction logic for FloatingWindow.xaml
    /// </summary>
    [Export(typeof(IFloatingView))]
    public partial class FloatingWindow : Window, IFloatingView
    {
        private readonly Lazy<FloatingViewModel> viewModel;


        public FloatingWindow()
        {
            InitializeComponent();

            this.viewModel = new Lazy<FloatingViewModel>(() => ViewHelper.GetViewModel<FloatingViewModel>(this));
        }


        private FloatingViewModel ViewModel { get { return viewModel.Value; } }


        private void window_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.ViewModel.ShowMainWindowCommand.Execute(null);
        }

        private void window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }
    }
}
