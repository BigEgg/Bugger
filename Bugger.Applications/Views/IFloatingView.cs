using BigEgg.Framework.Applications.Views;
using System;
using System.ComponentModel;

namespace Bugger.Applications.Views
{
    public interface IFloatingView : IView
    {
        double Left { get; set; }

        double Top { get; set; }


        event CancelEventHandler Closing;

        event EventHandler Closed;


        void Show();

        void Close();
    }
}
