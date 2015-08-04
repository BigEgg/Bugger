using Bugger.PlugIns.TrackingSystems.Fake.Views;
using System;

namespace Bugger.PlugIns.TrackingSystems.Fake.ViewModels
{
    public class SettingViewModel : PlugInSettingViewModel<ISettingView>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingViewModel"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        public SettingViewModel(ISettingView view)
            : base(view)
        {

        }


        public override PlugInSettingValidationResult ValidateSettings()
        {
            return PlugInSettingValidationResult.Valid;
        }

        protected override void SubmitSettingChangesCore()
        {
            throw new NotImplementedException();
        }
    }
}
