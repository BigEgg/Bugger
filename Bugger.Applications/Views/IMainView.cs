using System;
using System.ComponentModel;
using BigEgg.Framework.Applications.Views;

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
