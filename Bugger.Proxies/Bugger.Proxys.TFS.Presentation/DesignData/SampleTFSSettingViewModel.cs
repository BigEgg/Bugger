using Bugger.Proxy.TFS.ViewModels;
using Bugger.Proxy.TFS.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Bugger.Proxy.TFS.Presentation.DesignData
{
    public class SampleTFSSettingViewModel : TFSSettingViewModel
    {
        public SampleTFSSettingViewModel()
            : base(new MockTFSSettingView(), null)
        {
            this.Settings = new Documents.SettingDocument();
            this.Settings.ConnectUri = new Uri("https://tfs.codeplex.com:443/tfs/TFS12");
            this.Settings.BugFilterField = "Work Item Type";
            this.Settings.BugFilterValue = "Bugs";
            this.Settings.UserName = "BigEgg";
            this.Settings.Password = "Password";
            this.Settings.PriorityRed = "1,2";
        }

        private class MockTFSSettingView : MockView, ITFSSettingView
        {
        }
    }
}
