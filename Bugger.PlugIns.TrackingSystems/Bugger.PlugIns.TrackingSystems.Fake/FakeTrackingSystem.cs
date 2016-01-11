using BigEgg.Framework.Applications.Applications.Commands;
using Bugger.Models;
using Bugger.PlugIns.TrackingSystem;
using Bugger.PlugIns.TrackingSystems.Fake.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;

namespace Bugger.PlugIns.TrackingSystems.Fake
{
    [Export(typeof(ITrackingSystemPlugIn)), Export(typeof(IPlugIn))]
    public class FakeTrackingSystem : PlugInBase, ITrackingSystemPlugIn
    {
        private readonly IDataService dataService;
        private readonly DelegateCommand clearBugsCommand;


        [ImportingConstructor]
        public FakeTrackingSystem(IDataService dataService)
            : base(new Guid("41090009-10c1-447f-9189-a42cd9657c29"), PlugInType.TrackingSystem)
        {
            this.dataService = dataService;

            clearBugsCommand = new DelegateCommand(() => dataService.Clear());
        }


        public override PlugInSettingViewModel<IPlugInSettingView> GetSettingViewModel()
        {
            throw new NotImplementedException();
        }

        public TrackingSystemStatus GetStatus()
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<Bug> Query(List<string> teamMembers)
        {
            return dataService.GetTeamBugs(teamMembers);
        }

        public ReadOnlyCollection<Bug> Query(string userName, bool isFilterCreatedBy = true)
        {
            return dataService.GetBugs(userName, isFilterCreatedBy);
        }
    }
}
