using BigEgg.Framework.Applications.Applications.ViewModels;
using System;

namespace Bugger.PlugIns
{
    /// <summary>
    /// The view model used to show settings of the Plug-in.
    /// </summary>
    /// <typeparam name="TView">The type of the Plug-in setting view.</typeparam>
    public abstract class PlugInSettingViewModel<TView> : ViewModel<TView> where TView : IPlugInSettingView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlugInSettingViewModel{TView}"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        protected PlugInSettingViewModel(TView view)
            : base(view)
        { }


        /// <summary>
        /// Validates the settings.
        /// </summary>
        /// <returns></returns>
        public abstract PlugInSettingValidationResult ValidateSettings();

        /// <summary>
        /// Submits the settings changes.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Cannot submit setting changes when validate is failed.</exception>
        public void SubmitSettingChanges()
        {
            if (ValidateSettings() != PlugInSettingValidationResult.Valid) { throw new InvalidOperationException("Cannot submit setting changes when validate is failed."); }

            SubmitSettingChangesCore();
        }


        /// <summary>
        /// Submits the setting changes core.
        /// </summary>
        protected abstract void SubmitSettingChangesCore();
    }
}
