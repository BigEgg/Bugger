using BigEgg.Framework.Presentation;
using Bugger.Applications.ViewModels;
using Bugger.Applications.Views;

namespace Bugger.Presentation.DesignData
{
    public class SampleSettingsViewModel : SettingsViewModel
    {
        public SampleSettingsViewModel()
            : base(new MockSettingsView(), new MockProxyService(), "BigEgg;Pupil")
        {

        }

        private class MockSettingsView : MockView, ISettingsView
        {
            public string Title { get { return Properties.Resources.SettingsViewTitle; } }
        }
    }
}
