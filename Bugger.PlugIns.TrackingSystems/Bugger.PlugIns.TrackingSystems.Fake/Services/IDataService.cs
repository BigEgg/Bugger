using Bugger.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Bugger.PlugIns.TrackingSystems.Fake.Services
{
    public interface IDataService
    {
        ReadOnlyCollection<Bug> GetTeamBugs(List<string> teamMembers);

        ReadOnlyCollection<Bug> GetBugs(string userName, bool isFilterCreatedBy);
    }
}
