using Bugger.Applications.Views;
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
        public SettingDialogWindow()
        {
            InitializeComponent();
        }


        public void ShowDialog(object owner)
        {
            Show();
            Owner = owner as Window;
            this.Visibility = Visibility.Visible;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
        }
    }
}
