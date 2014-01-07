using Bugger.Proxy.TFS.Views;
using System.ComponentModel.Composition;
using System.Windows;

namespace Bugger.Proxy.TFS.Presentation
{
    [Export(typeof(IUriHelpView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class UriHelpWindow : Window, IUriHelpView
    {
        public UriHelpWindow()
        {
            this.InitializeComponent();
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
        }
    }
}