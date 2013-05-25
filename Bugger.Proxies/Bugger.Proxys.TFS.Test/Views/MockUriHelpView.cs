using BigEgg.Framework.Applications.Views;
using Bugger.Proxys.TFS.ViewModels;
using Bugger.Proxys.TFS.Views;
using System;
using System.ComponentModel.Composition;

namespace Bugger.Proxys.TFS.Test.Views
{
    [Export(typeof(IUriHelpView))]
    public class MockUriHelpView : MockDialogViewBase, IUriHelpView
    {
        public Action<MockUriHelpView> ShowDialogAction { get; set; }
        public UriHelpViewModel ViewModel { get { return ViewHelper.GetViewModel<UriHelpViewModel>(this); } }

        protected override void OnShowDialogAction()
        {
            if (ShowDialogAction != null) { ShowDialogAction(this); }
        }
    }
}
