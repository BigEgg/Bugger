using BigEgg.Framework.Applications.Applications.Commands;
using Bugger.Models;
using Bugger.PlugIns.TrackingSystem;
using Bugger.PlugIns.TrackingSystems.Fake.Properties;
using Bugger.PlugIns.TrackingSystems.Fake.Services;
using Bugger.PlugIns.TrackingSystems.Fake.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Threading.Tasks;

namespace Bugger.PlugIns.TrackingSystems.Fake
{
    [Export(typeof(ITrackingSystemPlugIn)), Export(typeof(IPlugIn))]
    public class FakeTrackingSystem : PlugInBase, ITrackingSystemPlugIn
    {
        private readonly IDataService dataService;
        private readonly SettingViewModel settingViewModel;
        private readonly DelegateCommand clearBugsCommand;
        private TrackingSystemStatus status;


        [ImportingConstructor]
        public FakeTrackingSystem(CompositionContainer container, IDataService dataService)
            : base(new Guid("41090009-10c1-447f-9189-a42cd9657c29"), PlugInType.TrackingSystem)
        {
            this.dataService = dataService;

            settingViewModel = container.GetExportedValue<SettingViewModel>();

            clearBugsCommand = new DelegateCommand(() => dataService.Clear());

            status = TrackingSystemStatus.Unknown;
        }

        #region Implement PlugIn Base Class
        protected override void OnInitialize()
        {
            settingViewModel.ClearBugsCommand = clearBugsCommand;

            settingViewModel.UsersName = Settings.Default.UsersName;
            settingViewModel.BugsCountForEveryone = Settings.Default.BugsForEveryone;
            settingViewModel.BugsRefreshMinutes = Settings.Default.BugsRefreshMinutes;

            status = settingViewModel.Validate() ? TrackingSystemStatus.CanConnect : TrackingSystemStatus.ConfigurationNotValid;
        }
        #endregion


        public override PlugInSettingViewModel<IPlugInSettingView> GetSettingViewModel()
        {
            return settingViewModel;
        }

        public TrackingSystemStatus GetStatus()
        {
            return status;
        }

        public async Task<ReadOnlyCollection<Bug>> QueryAsync(List<string> teamMembers)
        {
            status = TrackingSystemStatus.Querying;
            return await Task.Factory.StartNew(() =>
            {
                var bugs = dataService.GetTeamBugs(teamMembers);
                status = TrackingSystemStatus.CanConnect;
                return bugs;
            });
        }

        public async Task<ReadOnlyCollection<Bug>> QueryAsync(string userName, bool isFilterCreatedBy = true)
        {
            status = TrackingSystemStatus.Querying;
            return await Task.Factory.StartNew(() =>
            {
                var bugs = dataService.GetBugs(userName, isFilterCreatedBy);
                status = TrackingSystemStatus.CanConnect;
                return bugs;
            });
        }
    }
}
