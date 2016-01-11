using Bugger.PlugIns.TrackingSystems.Fake.Properties;
using Bugger.PlugIns.TrackingSystems.Fake.Views;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace Bugger.PlugIns.TrackingSystems.Fake.ViewModels
{
    [Export]
    public class SettingViewModel : PlugInSettingViewModel<ISettingView>
    {
        private string usersName;
        private int bugsCountForEveryone;
        private int bugsRefreshMinutes;
        private ICommand clearBugsCommand;


        [ImportingConstructor]
        public SettingViewModel(ISettingView view)
            : base(view)
        { }


        [Required(ErrorMessageResourceName = "UsersNameMandatory", ErrorMessageResourceType = typeof(Resources))]
        public string UsersName
        {
            get { return usersName; }
            set
            {
                SetPropertyAndValidate(ref usersName, value);
            }
        }

        [Range(1, 10, ErrorMessageResourceName = "BugsCountRange", ErrorMessageResourceType = typeof(Resources))]
        public int BugsCountForEveryone
        {
            get { return bugsCountForEveryone; }
            set
            {
                SetPropertyAndValidate(ref bugsCountForEveryone, value);
            }
        }

        [Range(1, 30, ErrorMessageResourceName = "BugsRefreshMinutes", ErrorMessageResourceType = typeof(Resources))]
        public int BugsRefreshMinutes
        {
            get { return bugsRefreshMinutes; }
            set
            {
                SetPropertyAndValidate(ref bugsRefreshMinutes, value);
            }
        }

        public ICommand ClearBugsCommand
        {
            get { return clearBugsCommand; }
            set
            {
                SetProperty(ref clearBugsCommand, value);
            }
        }


        public override PlugInSettingValidationResult ValidateSettings()
        {
            return Validate() ? PlugInSettingValidationResult.Valid : PlugInSettingValidationResult.Invalid;
        }

        protected override void SubmitSettingChangesCore()
        {
            Settings.Default.BugsRefreshMinutes = bugsRefreshMinutes;
            Settings.Default.BugsForEveryone = BugsCountForEveryone;
            Settings.Default.UsersName = UsersName;

            Settings.Default.Save();
        }
    }
}
