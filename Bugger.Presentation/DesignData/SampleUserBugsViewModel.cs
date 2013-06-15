using BigEgg.Framework.Presentation;
using Bugger.Applications.ViewModels;
using Bugger.Applications.Views;

namespace Bugger.Presentation.DesignData
{
    public class SampleUserBugsViewModel : UserBugsViewModel
    {
        public SampleUserBugsViewModel()
            : base(new MockUserBugsView(), new MockDataService())
        {
        }

        private class MockUserBugsView : MockView, IUserBugsView
        {
        }
    }
}
