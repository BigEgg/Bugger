using Bugger.Models;
using Bugger.PlugIns.TrackingSystems.Fake.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;

namespace Bugger.PlugIns.TrackingSystems.Fake.Test.Services
{

    [Export(typeof(IDataService))]
    public class MockDataService : IDataService
    {
        public bool GetBugsCalled { get; private set; }

        public bool GetTeamBugsCalled { get; private set; }


        public ReadOnlyCollection<Bug> GetBugs(string userName, bool isFilterCreatedBy)
        {
            GetBugsCalled = true;
            return null;
        }

        public ReadOnlyCollection<Bug> GetTeamBugs(List<string> teamMembers)
        {
            GetTeamBugsCalled = true;
            return null;
        }

        public void Clear()
        {
            GetBugsCalled = false;
            GetTeamBugsCalled = false;
        }
    }
}
