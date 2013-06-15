using BigEgg.Framework.Applications.Views;
using Bugger.Applications.ViewModels;
using Bugger.Applications.Views;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using System.Windows.Data;

namespace Bugger.Presentation.Views
{
    /// <summary>
    /// Interaction logic for UserBugsView.xaml
    /// </summary>
    [Export(typeof(IUserBugsView))]
    public partial class UserBugsView : UserControl, IUserBugsView
    {
        public UserBugsView()
        {
            InitializeComponent();
        }
    }
}
