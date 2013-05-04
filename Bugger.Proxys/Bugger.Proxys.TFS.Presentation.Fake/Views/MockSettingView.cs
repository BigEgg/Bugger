using BigEgg.Framework.UnitTesting.Views;
using Bugger.Proxys.TFS.Views;
using System.ComponentModel.Composition;

namespace Bugger.Proxys.TFS.Presentation.Fake.Views
{
    [Export(typeof(ISettingView))]
    public class MockSettingView : MockView, ISettingView
    {
    }
}
