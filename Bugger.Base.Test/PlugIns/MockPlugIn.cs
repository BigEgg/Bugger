using Bugger.PlugIns;
using System;
using System.Collections.Generic;

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


        public override PlugInSettingDialogViewModel<IPlugInSettingDialogView> OpenSettingDialog()
        {
            return settingViewModel;
        }

        public Action InitializeCoreAction { get; set; }

        public IDictionary<Guid, IPlugInSharedData> EnviromentSharedData { get { return environmentSharedData; } }

        protected override void OnInitialize()
        {
            InitializeCoreAction.Invoke();
        }
    }
}
