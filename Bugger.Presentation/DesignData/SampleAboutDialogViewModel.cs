using BigEgg.Framework.Presentation;
using Bugger.Applications.ViewModels;
using Bugger.Applications.Views;

namespace Bugger.Presentation.DesignData
{
    public class SampleAboutDialogViewModel : AboutDialogViewModel
    {
        public SampleAboutDialogViewModel()
            : base(new MockAboutDialog())
        {
        }


        public new string Version { get { return "1.0.0.0 (Design Time)"; } }


        private class MockAboutDialog : MockDialogView, IAboutDialogView
        {
        }
    }
}
