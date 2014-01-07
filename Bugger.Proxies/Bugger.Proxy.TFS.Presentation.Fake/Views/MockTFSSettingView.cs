using BigEgg.Framework.Presentation;
using Bugger.Proxy.TFS.Properties;
using Bugger.Proxy.TFS.Views;
using System.ComponentModel.Composition;

namespace Bugger.Proxy.TFS.Presentation.Fake.Views
{
    [Export(typeof(ITFSSettingView))]
    public class MockTFSSettingView : MockView, ITFSSettingView
    {
        public string Title { get { return Resources.UriHelpDialogTitle; } }
    }
}
