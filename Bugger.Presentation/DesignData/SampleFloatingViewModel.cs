using BigEgg.Framework.Presentation;
using Bugger.Applications.ViewModels;
using Bugger.Applications.Views;
using System;
using System.ComponentModel;

namespace Bugger.Presentation.DesignData
{
    public class SampleFloatingViewModel : FloatingViewModel
    {
        public SampleFloatingViewModel()
            : base(new MockFloatingView(), new MockDataService(), null)
        {
        }

        private class MockFloatingView : MockView, IFloatingView
        {
            public double Left { get; set; }

            public double Top { get; set; }

            public double Width { get; set; }

            public double Height { get; set; }


            public event CancelEventHandler Closing;

            public event EventHandler Closed;


            public void Show()
            {
            }

            public void Close()
            {
            }
        }
    }
}
