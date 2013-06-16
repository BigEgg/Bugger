using BigEgg.Framework.Presentation;
using Bugger.Proxy.TFS.Models;
using Bugger.Proxy.TFS.Presentation.Properties;
using Bugger.Proxy.TFS.ViewModels;
using Bugger.Proxy.TFS.Views;
using System;

namespace Bugger.Proxy.TFS.Presentation.DesignData
{
    public class SampleTFSSettingViewModel : TFSSettingViewModel
    {
        public SampleTFSSettingViewModel()
            : base(new MockTFSSettingView())
        {
            this.Settings = new Documents.SettingDocument();
            this.Settings.ConnectUri = new Uri("https://tfs.codeplex.com:443/tfs/TFS12");
            this.Settings.BugFilterField = "Work Item Type";
            this.Settings.BugFilterValue = "Bugs";
            this.Settings.UserName = "BigEgg";
            this.Settings.Password = "Password";
            this.Settings.PriorityRed = "1,2";

            var field = new TFSField("Work Item Type");
            field.AllowedValues.Add("Work Item");
            field.AllowedValues.Add("Bugs");

            this.TFSFields.Add(field);
        }

        private class MockTFSSettingView : MockView, ITFSSettingView
        {
            public string Title { get { return Resources.SettingViewTitle; } }
        }
    }
}
