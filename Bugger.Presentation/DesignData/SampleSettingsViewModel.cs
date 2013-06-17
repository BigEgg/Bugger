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
            this.FilterStatusValues = "High; Low";
        }

        private class MockSettingsView : MockView, ISettingsView
        {
            public string Title { get { return Properties.Resources.SettingsViewTitle; } }
        }
    }
}
