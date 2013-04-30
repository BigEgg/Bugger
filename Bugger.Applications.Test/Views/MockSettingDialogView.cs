using BigEgg.Framework.Applications.Views;
using Bugger.Applications.ViewModels;
using Bugger.Applications.Views;
using System;
using System.ComponentModel.Composition;

namespace Bugger.Applications.Test.Views
{
    [Export(typeof(ISettingDialogView)), Export]
    public class MockSettingDialogView : MockDialogViewBase, ISettingDialogView
    {
        public Action<MockSettingDialogView> ShowDialogAction { get; set; }
        public SettingDialogViewModel ViewModel { get { return ViewHelper.GetViewModel<SettingDialogViewModel>(this); } }


        protected override void OnShowDialogAction()
        {
            if (ShowDialogAction != null) { ShowDialogAction(this); }
        }
    }
}
