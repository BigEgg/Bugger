using BigEgg.Framework.Applications.UnitTesting.Views;
using Bugger.PlugIns.TrackingSystems.Fake.Views;
using System.ComponentModel.Composition;

namespace Bugger.PlugIns.TrackingSystems.Fake.Test.Views
{
    [Export(typeof(ISettingView))]
    public class MockSettingView : MockView, ISettingView
    { }
}
