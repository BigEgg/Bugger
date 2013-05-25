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


        private class MockAboutDialog : MockDialogView, IAboutDialogView
        {
        }
    }
}
