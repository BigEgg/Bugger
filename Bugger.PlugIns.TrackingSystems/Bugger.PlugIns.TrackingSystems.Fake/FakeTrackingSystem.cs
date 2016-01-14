using BigEgg.Framework.Applications.Applications.Commands;
using Bugger.Models;
using Bugger.PlugIns.TrackingSystem;
using Bugger.PlugIns.TrackingSystems.Fake.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace Bugger.PlugIns.TrackingSystems.Fake
{
    [Export(typeof(ITrackingSystemPlugIn)), Export(typeof(IPlugIn))]
    public class FakeTrackingSystem : PlugInBase, ITrackingSystemPlugIn
    {
        private readonly IDataService dataService;
        private readonly DelegateCommand clearBugsCommand;
        private TrackingSystemStatus status;


        [ImportingConstructor]
        public FakeTrackingSystem(IDataService dataService)
            : base(new Guid("41090009-10c1-447f-9189-a42cd9657c29"), PlugInType.TrackingSystem)
        {
            this.dataService = dataService;

            clearBugsCommand = new DelegateCommand(() => dataService.Clear());

            status = TrackingSystemStatus.Unknown;
        }


        public override PlugInSettingViewModel<IPlugInSettingView> GetSettingViewModel()
        {
            throw new NotImplementedException();
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
