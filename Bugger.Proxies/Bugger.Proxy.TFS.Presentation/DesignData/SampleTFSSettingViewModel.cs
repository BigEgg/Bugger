using BigEgg.Framework.Presentation;
using Bugger.Proxy.TFS.Models;
using Bugger.Proxy.TFS.Presentation.Properties;
using Bugger.Proxy.TFS.ViewModels;
using Bugger.Proxy.TFS.Views;
using System;
using System.Linq;

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

            this.PriorityValues.Add(new CheckString("High"));
            this.PriorityValues.Add(new CheckString("Medium"));
            this.PriorityValues.Add(new CheckString("Low"));
            var values = this.PriorityValues.Where(x => x.Name == "High" || x.Name == "Medium");
            foreach (var value in values)
            {
                value.IsChecked = true;
            }
            this.PriorityRed = "High;Medium";

            var field = new TFSField("Work Item Type");
            field.AllowedValues.Add("Work Item");
            field.AllowedValues.Add("Bugs");

            var idField = new TFSField("ID");

            this.TFSFields.Add(idField);
            this.TFSFields.Add(field);

            this.PropertyMappingCollection["ID"] = "ID";

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
