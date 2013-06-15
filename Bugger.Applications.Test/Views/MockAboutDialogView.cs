using BigEgg.Framework.Applications.Views;
using BigEgg.Framework.UnitTesting.Views;
using Bugger.Applications.ViewModels;
using Bugger.Applications.Views;
using System;
using System.ComponentModel.Composition;

namespace Bugger.Applications.Test.Views
{
    [Export(typeof(IAboutDialogView)), Export]
    public class MockAboutDialogView : MockDialogViewBase, IAboutDialogView
    {
        public Action<MockAboutDialogView> ShowDialogAction { get; set; }
        public AboutDialogViewModel ViewModel { get { return ViewHelper.GetViewModel<AboutDialogViewModel>(this); } }


        protected override void OnShowDialogAction()
        {
            if (ShowDialogAction != null) { ShowDialogAction(this); }
        }
    }
}
