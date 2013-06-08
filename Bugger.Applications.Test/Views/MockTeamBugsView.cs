using BigEgg.Framework.Presentation;
using Bugger.Applications.Views;
using System.ComponentModel.Composition;

namespace Bugger.Applications.Test.Views
{
    [Export(typeof(ITeamBugsView)), Export]
    public class MockTeamBugsView : MockView, ITeamBugsView
    {
    }
}
