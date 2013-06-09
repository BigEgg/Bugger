using BigEgg.Framework.Presentation;
using Bugger.Applications.ViewModels;
using Bugger.Applications.Views;
using Bugger.Presentation.Services;
using System;
using System.ComponentModel;

namespace Bugger.Presentation.DesignData
{
    public class SampleFloatingViewModel : FloatingViewModel
    {
        public SampleFloatingViewModel()
            : base(new MockFloatingView(), new MockDataService(), new PresentationService())
        {
        }

        private class MockFloatingView : MockView, IFloatingView
        {
            public double Left { get; set; }

            public double Top { get; set; }


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
