using Bugger.Applications.Views;
using System.ComponentModel.Composition;
using System.Windows;

namespace Bugger.Presentation.Views
{
    /// <summary>
    /// Interaction logic for AboutDialogWindow.xaml
    /// </summary>
    [Export(typeof(IAboutDialogView))]
    public partial class AboutDialogWindow : Window, IAboutDialogView
    {
        public AboutDialogWindow()
        {
            InitializeComponent();
        }

        public void ShowDialog(object owner)
        {
            Owner = owner as Window;
            ShowDialog();
        }
    }
}
