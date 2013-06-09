using BigEgg.Framework.Applications.ViewModels;
using Bugger.Applications.Properties;
using Bugger.Applications.Services;
using Bugger.Applications.Views;
using Bugger.Domain.Models;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;

namespace Bugger.Applications.ViewModels
{ 
    [Export]
    public class UserBugsViewModel : ViewModel<IUserBugsView>
    {
        #region Fields
        private readonly IDataService dataService;
        #endregion

        [ImportingConstructor]
        public UserBugsViewModel(IUserBugsView view, IDataService dataService)
            : base(view)
        {
            this.dataService = dataService;
        }

        #region Properties
        public ObservableCollection<Bug> RedBugs { get { return this.dataService.UserRedBugs; } }

        public ObservableCollection<Bug> YellowBugs { get { return this.dataService.UserYellowBugs; } }
        #endregion
    }
}
