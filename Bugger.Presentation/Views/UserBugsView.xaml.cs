using Bugger.Applications.Views;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace Bugger.Presentation.Views
{
    /// <summary>
    /// Interaction logic for UserBugsView.xaml
    /// </summary>
    [Export(typeof(ITeamBugsView))]
    public partial class UserBugsView : UserControl, IUserBugsView
    {
        public UserBugsView()
        {
            InitializeComponent();
        }
    }
}
