using Bugger.Proxys.TFS.Views;
using System.ComponentModel.Composition;
using System.Windows;

namespace Bugger.Proxys.TFS.Presentation
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
            ShowDialog();
        }
    }
}