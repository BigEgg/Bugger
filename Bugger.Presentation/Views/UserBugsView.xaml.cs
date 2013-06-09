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
        private ListCollectionView groupedBugs;


        public UserBugsView()
        {
            InitializeComponent();
        }


        public ListCollectionView GroupedBugs { get { return this.groupedBugs; } }
    
    
        public void CreateGroupedBugs()
        {
            var viewModel = ViewHelper.GetViewModel<UserBugsViewModel>(this);
            groupedBugs = new ListCollectionView(viewModel.Bugs);
            groupedBugs.GroupDescriptions.Add(new PropertyGroupDescription("Type"));
        }
    }
}
