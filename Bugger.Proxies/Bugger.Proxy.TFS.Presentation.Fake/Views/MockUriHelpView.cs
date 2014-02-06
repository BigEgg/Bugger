using BigEgg.Framework.Applications.Views;
using BigEgg.Framework.UnitTesting.Views;
using Bugger.Proxy.TFS.ViewModels;
using Bugger.Proxy.TFS.Views;
using System;
using System.ComponentModel.Composition;

namespace Bugger.Proxy.TFS.Presentation.Fake.Views
{
    [Export(typeof(IUriHelpView))]
    public class MockUriHelpView : MockDialogViewBase, IUriHelpView
    {
        public Action<MockUriHelpView> ShowDialogAction { get; set; }
        public UriHelperDialogViewModel ViewModel { get { return ViewHelper.GetViewModel<UriHelperDialogViewModel>(this); } }

        protected override void OnShowDialogAction()
        {
            if (ShowDialogAction != null) { ShowDialogAction(this); }
        }
    }
}
