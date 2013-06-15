using BigEgg.Framework.Presentation;
using Bugger.Applications.ViewModels;
using Bugger.Applications.Views;

namespace Bugger.Presentation.DesignData
{
    class SampleTeamBugsViewModel : TeamBugsViewModel
    {
        public SampleTeamBugsViewModel()
            : base(new MockTeamBugsView(), new MockDataService())
        {
        }

        private class MockTeamBugsView : MockView, ITeamBugsView
        {
        }
    }
}
