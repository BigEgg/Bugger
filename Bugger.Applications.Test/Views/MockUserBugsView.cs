using BigEgg.Framework.Presentation;
using Bugger.Applications.Views;
using System.ComponentModel.Composition;

namespace Bugger.Applications.Test.Views
{
    [Export(typeof(IUserBugsView)), Export]
    public class MockUserBugsView : MockView, IUserBugsView
    {
    }
}
