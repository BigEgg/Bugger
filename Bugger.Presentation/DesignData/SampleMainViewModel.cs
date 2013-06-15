using BigEgg.Framework.Presentation;
using Bugger.Applications.ViewModels;
using Bugger.Applications.Views;
using System;
using System.ComponentModel;

namespace Bugger.Presentation.DesignData
{
    public class SampleMainViewModel : MainViewModel
    {
        public SampleMainViewModel()
            : base(new MockMainView(), new MockDataService(), new MockShellService(), null)
        {
            (this.ShellService as MockShellService).MainView = this.View;
            this.ShellService.UserBugsView = new SampleUserBugsViewModel().View;
            this.ShellService.TeamBugsView = new SampleTeamBugsViewModel().View;
        }

        private class MockMainView : MockView, IMainView
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
