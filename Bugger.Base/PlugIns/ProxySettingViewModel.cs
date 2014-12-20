using BigEgg.Framework.Applications.ViewModels;
using System;

namespace Bugger.Base.PlugIns
{
    public abstract class ProxySettingViewModel<TView> : ViewModel<TView> where TView : IPlugInSettingView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProxySettingViewModel{TView}"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        protected ProxySettingViewModel(TView view)
            : base(view, false)
        { }

        #region Properties
        /// <summary>
        /// Gets the validate result.
        /// </summary>
        /// <value>
        /// The validate result.
        /// </value>
        public SettingDialogValidateionResult ValidateResult { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Validates the settings.
        /// </summary>
        /// <returns></returns>
        public abstract SettingDialogValidateionResult ValidateSettings();

        /// <summary>
        /// Submits the settings modify.
        /// </summary>
        /// <exception cref="System.InvalidOperationException"></exception>
        public void SubmitSettingModify()
        {
            if (ValidateResult != SettingDialogValidateionResult.Valid) { throw new InvalidOperationException(); }
            SubmitSettingModifyCore();
        }

        /// <summary>
        /// Submits the setting modify core.
        /// </summary>
        protected abstract void SubmitSettingModifyCore();
        #endregion
    }
}
