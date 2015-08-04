using Bugger.PlugIns.TrackingSystems.Fake.Properties;
using Bugger.PlugIns.TrackingSystems.Fake.Views;
using System;
using System.ComponentModel.DataAnnotations;

namespace Bugger.PlugIns.TrackingSystems.Fake.ViewModels
{
    public class SettingViewModel : PlugInSettingViewModel<ISettingView>
    {
        private string usersName;
        private int bugsCountForEveryone;
        private int bugsCacheMinutes;


        public SettingViewModel(ISettingView view)
            : base(view)
        {

        }


        [Required(ErrorMessageResourceName = "UsersNameMandatory", ErrorMessageResourceType = typeof(Resources))]
        public string UsersName
        {
            get { return usersName; }
            set
            {
                SetProperty(ref usersName, value);
            }
        }

        [Range(1, 10, ErrorMessageResourceName = "BugsCountRange", ErrorMessageResourceType = typeof(Resources))]
        public int BugsCountForEveryone
        {
            get { return bugsCountForEveryone; }
            set
            {
                SetProperty(ref bugsCountForEveryone, value);
            }
        }

        [Range(1, 30, ErrorMessageResourceName = "BugsCacheMinutes", ErrorMessageResourceType = typeof(Resources))]
        public int BugsCacheMinutes
        {
            get { return bugsCacheMinutes; }
            set
            {
                SetProperty(ref bugsCacheMinutes, value);
            }
        }


        public override PlugInSettingValidationResult ValidateSettings()
        {
            return HasErrors ? PlugInSettingValidationResult.Invalid : PlugInSettingValidationResult.Valid;
        }

        protected override void SubmitSettingChangesCore()
        {
            throw new NotImplementedException();
        }
    }
}
