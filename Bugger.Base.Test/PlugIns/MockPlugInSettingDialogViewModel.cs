using Bugger.Base.PlugIns;

namespace Bugger.Domain.Test.PlugIns
{
    public class MockPlugInSettingDialogViewModel : PlugInSettingDialogViewModel<IPlugInSettingDialogView>
    {
        public MockPlugInSettingDialogViewModel(IPlugInSettingDialogView view)
            : base(view)
        {
            CleanUp();
        }


        public PlugInSettingDialogValidationResult ValidationResult { get; set; }

        public bool SubmitSettingChangesCoreCalled { get; set; }


        public override PlugInSettingDialogValidationResult ValidateSettings()
        {
            return ValidationResult;
        }

        protected override void SubmitSettingChangesCore()
        {
            SubmitSettingChangesCoreCalled = true;
        }

        public void CleanUp()
        {
            ValidationResult = PlugInSettingDialogValidationResult.Valid;
            SubmitSettingChangesCoreCalled = false;
        }
    }
}
