using Bugger.Applications.Views;
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
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
