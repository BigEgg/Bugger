using BigEgg.Framework.Presentation;
using Bugger.Applications.ViewModels;
using Bugger.Applications.Views;

namespace Bugger.Presentation.DesignData
{
    public class SampleSettingDialogViewModel : SettingDialogViewModel
    {
        public SampleSettingDialogViewModel()
            : base(new MockSettingDialogWindow(), new MockProxyService(), new SampleSettingsViewModel())
        {

        }

        private class MockSettingDialogWindow : MockDialogView, ISettingDialogView
        {}
    }
}
