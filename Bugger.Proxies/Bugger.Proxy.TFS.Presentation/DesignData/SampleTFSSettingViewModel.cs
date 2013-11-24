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
            : base(new MockTFSSettingView(), new MockUriHelperVIew())
        {
            this.ConnectUri = new Uri("https://tfs.codeplex.com:443/tfs/TFS12");
            this.BugFilterField = "Work Item Type";
            this.BugFilterValue = "Bugs";
            this.UserName = "BigEgg";
            this.Password = "Password";
            this.PriorityRed = "High;Medium";

            var field = new TFSField("Work Item Type");
            field.AllowedValues.Add("Work Item");
            field.AllowedValues.Add("Bugs");

            this.TFSFields.Add(field);
            this.BugFilterFields.Add(field);
            this.ProgressType = ProgressTypes.SuccessWithError;
            this.ProgressValue = 100;
        }

        private class MockTFSSettingView : MockView, ITFSSettingView
        {
            public string Title { get { return Resources.SettingViewTitle; } }
        }

        private class MockUriHelperVIew : MockDialogView, IUriHelpView
        {

        }
    }
}
