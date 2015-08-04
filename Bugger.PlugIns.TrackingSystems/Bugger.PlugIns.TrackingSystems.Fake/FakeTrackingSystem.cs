using Bugger.Models;
using Bugger.PlugIns.TrackingSystem;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Bugger.PlugIns.TrackingSystems.Fake
{
    public class FakeTrackingSystem : PlugInBase, ITrackingSystemPlugIn
    {
        public FakeTrackingSystem()
            : base(new Guid("868CC6F0-8332-4A6B-8669-7B1881811AA1"), PlugInType.TrackingSystem)
        { }


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
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<Bug> Query(string userName, bool isFilterCreatedBy = true)
        {
            throw new NotImplementedException();
        }
    }
}
