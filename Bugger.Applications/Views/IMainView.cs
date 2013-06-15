using BigEgg.Framework.Applications.Views;
using System;
using System.ComponentModel;

namespace Bugger.Applications.Views
{
    public interface IMainView : IView
    {
        double Left { get; set; }

        double Top { get; set; }

        double Width { get; set; }

        double Height { get; set; }


        event CancelEventHandler Closing;

        event EventHandler Closed;


        void Show();

        void Close();
    }
}
