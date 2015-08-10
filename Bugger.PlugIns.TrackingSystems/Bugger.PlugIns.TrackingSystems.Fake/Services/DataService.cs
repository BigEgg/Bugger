using Bugger.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;

namespace Bugger.PlugIns.TrackingSystems.Fake.Services
{
    [Export(typeof(IDataService))]
    public class DataService : IDataService
    {
        public ReadOnlyCollection<Bug> GetBugs(string userName, bool isFilterCreatedBy)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<Bug> GetTeamBugs(List<string> teamMembers)
        {
            throw new NotImplementedException();
        }
    }
}
