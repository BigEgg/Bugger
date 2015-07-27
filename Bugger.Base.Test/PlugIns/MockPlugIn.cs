using Bugger.Base.PlugIns;
using System;

namespace Bugger.Domain.Test.PlugIns
{
    public class MockPlugIn : PlugInBase
    {
        private PlugInSettingDialogViewModel<IPlugInSettingDialogView> settingViewModel;

        public MockPlugIn(Guid guid, PlugInType plugInType)
            : base(guid, plugInType)
        {
            var view = new MockPlugInSettingDialogView();
            settingViewModel = new MockPlugInSettingDialogViewModel(view);
        }


        public override PlugInSettingDialogViewModel<IPlugInSettingDialogView> SettingViewModel { get { return settingViewModel; } }

        public Action InitializeCoreAction { get; set; }

        protected override void OnInitialize()
        {
            InitializeCoreAction.Invoke();
        }
    }
}
