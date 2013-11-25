using BigEgg.Framework.Presentation;
using Bugger.Proxy.TFS.ViewModels;
using Bugger.Proxy.TFS.Views;

namespace Bugger.Proxy.TFS.Presentation.DesignData
{
    public class SampleUriHelpViewModel : UriHelpViewModel
    {
        public SampleUriHelpViewModel()
            : base(new MockUriHelpViewModel())
        {
            this.ServerName = "https://tfs.codeplex.com:443/tfs/TFS12";
        }


        private class MockUriHelpViewModel : MockDialogView, IUriHelpView
        {
        }
    }
}
