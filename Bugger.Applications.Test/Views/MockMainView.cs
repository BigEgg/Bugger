using BigEgg.Framework.Presentation;
using Bugger.Applications.Views;
using System;
using System.ComponentModel;
using System.ComponentModel.Composition;

namespace Bugger.Applications.Test.Views
{
    [Export(typeof(IMainView))]
    public class MockMainView : MockView, IMainView
    {
        public bool IsVisible { get; private set; }

        public double Left { get; set; }

        public double Top { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }


        public event CancelEventHandler Closing;

        public event EventHandler Closed;


        public void Show()
        {
            IsVisible = true;
        }

        public void Close()
        {
            CancelEventArgs e = new CancelEventArgs();
            OnClosing(e);
            if (!e.Cancel)
            {
                IsVisible = false;
                OnClosed(EventArgs.Empty);
            }
        }

        public void SetNAForLocationAndSize()
        {
            Top = double.NaN;
            Left = double.NaN;
            Width = double.NaN;
            Height = double.NaN;
        }

        protected virtual void OnClosing(CancelEventArgs e)
        {
            if (Closing != null) { Closing(this, e); }
        }

        protected virtual void OnClosed(EventArgs e)
        {
            if (Closed != null) { Closed(this, e); }
        }
    }
}
