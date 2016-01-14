using Bugger.Models;
using Bugger.PlugIns.TrackingSystems.Fake.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Threading;

namespace Bugger.PlugIns.TrackingSystems.Fake.Test.Services
{

    [Export(typeof(IDataService))]
    public class MockDataService : IDataService
    {
        public bool GetBugsCalled { get; private set; }

        public bool GetTeamBugsCalled { get; private set; }

        public bool ClearCalled { get; private set; }


        public ReadOnlyCollection<Bug> GetBugs(string userName, bool isFilterCreatedBy)
        {
            GetBugsCalled = true;
            Thread.Sleep(500 * 1000);
            return null;
        }

        public ReadOnlyCollection<Bug> GetTeamBugs(IList<string> teamMembers)
        {
            GetTeamBugsCalled = true;
            Thread.Sleep(500 * 1000);
            return null;
        }

        public void Clear()
        {
            ClearCalled = true;
        }

        public void Reset()
        {
            GetBugsCalled = false;
            GetTeamBugsCalled = false;
            ClearCalled = false;
        }
    }
}
