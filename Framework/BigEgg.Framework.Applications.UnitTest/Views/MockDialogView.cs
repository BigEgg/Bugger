using BigEgg.Framework.Applications.Applications.Views;
using System;

namespace BigEgg.Framework.Applications.UnitTesting.Views
{
    public abstract class MockDialogView : MockView, IDialogView
    {
        public bool IsVisible { get; private set; }
        public object Owner { get; private set; }
        public Action<MockDialogView> ShowDialogAction { get; set; }


        public void ShowDialog(object owner)
        {
            Owner = owner;
            IsVisible = true;
            OnShowDialogAction();
            IsVisible = false;
            Owner = null;
        }

        public void Close()
        {
            IsVisible = false;
            Owner = null;
        }


        protected void OnShowDialogAction()
        {
            if (ShowDialogAction != null) { ShowDialogAction(this); }
        }
    }
}
