using Bugger.Applications.Views;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace Bugger.Presentation.Views
{
    /// <summary>
    /// Interaction logic for TeamBugsView.xaml
    /// </summary>
    [Export(typeof(ITeamBugsView))]
    public partial class TeamBugsView : UserControl, ITeamBugsView
    {
        public TeamBugsView()
        {
            InitializeComponent();
        }
    }
}
