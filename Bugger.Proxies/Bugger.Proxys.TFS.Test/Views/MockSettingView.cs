using Bugger.Proxys.TFS.Views;
using System.ComponentModel.Composition;

namespace Bugger.Proxys.TFS.Test.Views
{
    [Export(typeof(ISettingView))]
    public class MockSettingView : MockView, ISettingView
    {
    }
}
