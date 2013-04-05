using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Input;
using BigEgg.Framework.Applications.Commands;
using BigEgg.Framework.Applications.ViewModels;
using Bugger.Applications.Services;
using Bugger.Applications.Views;

namespace Bugger.Applications.ViewModels
{
    public class SettingsViewModel : ViewModel<ISettingsView>
    {
        #region Fields
        private const char TeamMemberSplitChar = ';';

        private readonly ObservableCollection<string> teamMembers;
        private readonly ObservableCollection<string> selectedTeamMembers;
        private readonly ReadOnlyCollection<string> proxys;
        private readonly DelegateCommand addNewTeamMemberCommand;
        private readonly DelegateCommand removeTeamMemberCommand;

        private string selectedTeamMember;
        private string activeProxy;
        private string userName;
        private string newTeamMember;
        private uint refreshMinutes;
        #endregion

        public SettingsViewModel(ISettingsView view, IProxyService proxyService, string teamMembers)
            : base(view)
        {
            this.teamMembers = new ObservableCollection<string>(teamMembers.Split(TeamMemberSplitChar));
            this.proxys = new ReadOnlyCollection<string>(proxyService.Proxys.Select(x => x.ProxyName).ToList());
            this.activeProxy = proxyService.ActiveProxy.ProxyName;

            this.addNewTeamMemberCommand = new DelegateCommand(AddNewTeamMemberCommandExecute, CanAddNewTeamMemberCommandExecute);
            this.removeTeamMemberCommand = new DelegateCommand(RemoveTeamMemberCommandExecute, CanRemoveTeamMemberCommandExecute);

            this.selectedTeamMembers = new ObservableCollection<string>();
            this.selectedTeamMember = this.teamMembers.FirstOrDefault();
            this.newTeamMember = string.Empty;
        }

        #region Properties
        public ICommand AddNewTeamMemberCommand { get { return this.addNewTeamMemberCommand; } }

        public ICommand RemoveTeamMemberCommand { get { return this.removeTeamMemberCommand; } }

        public ObservableCollection<string> TeamMembers { get { return this.teamMembers; } }

        public ObservableCollection<string> SelectedTeamMembers { get { return this.selectedTeamMembers; } }

        public string SelectedTeamMember
        {
            get { return this.selectedTeamMember; }
            set
            {
                if (this.selectedTeamMember != value)
                {
                    this.selectedTeamMember = value;
                    RaisePropertyChanged("SelectedTeamMember");
                }
            }
        }

        [Required]
        public string ActiveProxy
        {
            get { return this.activeProxy; }
            set
            {
                if (this.activeProxy != value)
                {
                    this.activeProxy = value;
                    RaisePropertyChanged("ActiveProxy");
                }
            }
        }

        [Required]
        public string UserName
        {
            get { return this.userName; }
            set
            {
                if (this.userName != value)
                {
                    this.userName = value;
                    RaisePropertyChanged("UserName");
                }
            }
        }

        public string NewTeamMember
        {
            get { return this.newTeamMember; }
            set
            {
                if (this.newTeamMember != value)
                {
                    this.newTeamMember = value;
                    RaisePropertyChanged("NewTeamMember");
                }
            }
        }

        [Range(1, 720)]
        public uint RefreshMinutes
        {
            get { return this.refreshMinutes; }
            set
            {
                if (this.refreshMinutes != value)
                {
                    this.refreshMinutes = value;
                    RaisePropertyChanged("RefreshMinutes");
                }
            }
        }
        #endregion

        #region Private
        private void AddNewTeamMemberCommandExecute()
        {

        }

        private bool CanAddNewTeamMemberCommandExecute()
        {
            return false;
        }

        private void RemoveTeamMemberCommandExecute()
        {

        }

        private bool CanRemoveTeamMemberCommandExecute()
        {
            return false;
        }

        private void UpdateCommands()
        {
            this.addNewTeamMemberCommand.RaiseCanExecuteChanged();
            this.removeTeamMemberCommand.RaiseCanExecuteChanged();
        }
        #endregion
    }
}
