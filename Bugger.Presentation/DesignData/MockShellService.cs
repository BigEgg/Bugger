using BigEgg.Framework.Foundation;
using Bugger.Applications.Services;

namespace Bugger.Presentation.DesignData
{
    public class MockShellService : Model, IShellService
    {
        public object MainView { get; set; }

        public object UserBugsView { get; set; }

        public object TeamBugsView { get; set; }
    }
}
