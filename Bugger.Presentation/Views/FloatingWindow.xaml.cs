using BigEgg.Framework.Applications.Views;
using Bugger.Applications.ViewModels;
using Bugger.Applications.Views;
using System.Windows;

namespace Bugger.Presentation.Views
{
    /// <summary>
    /// Interaction logic for FloatingWindow.xaml
    /// </summary>
    public partial class FloatingWindow : Window, IFloatingView
    {
        private FloatingViewModel viewModel;

        public FloatingWindow()
        {
            InitializeComponent();

            this.viewModel = ViewHelper.GetViewModel<FloatingViewModel>(this);
        }

        private void window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void window_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.viewModel.ShowMainWindowCommand.Execute(null);
        }
    }
}
