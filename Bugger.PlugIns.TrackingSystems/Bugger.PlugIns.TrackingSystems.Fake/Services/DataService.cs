using Bugger.Models;
using Bugger.PlugIns.TrackingSystems.Fake.Models;
using Bugger.PlugIns.TrackingSystems.Fake.Properties;
using Bugger.PlugIns.TrackingSystems.Fake.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;

namespace Bugger.PlugIns.TrackingSystems.Fake.Services
{
    [Export(typeof(IDataService))]
    public class DataService : IDataService
    {
        private readonly IList<FakeBug> bugs;
        private int index;
        private DateTime nextRefreshTime;

        public DataService()
        {
            bugs = new List<FakeBug>();
            index = 1;
            nextRefreshTime = DateTime.Now;
        }

        public ReadOnlyCollection<Bug> GetBugs(string userName, bool isFilterCreatedBy)
        {
            return new ReadOnlyCollection<Bug>(
                GetCachedBugs().Where(b => userName == b.AssignedTo || (isFilterCreatedBy && b.CreatedBy == userName))
                               .Select(b => b.ToBug())
                               .ToList());
        }

        public ReadOnlyCollection<Bug> GetTeamBugs(IList<string> teamMembers)
        {
            return new ReadOnlyCollection<Bug>(
                GetCachedBugs().Where(b => teamMembers.Contains(b.AssignedTo))
                               .Select(b => b.ToBug())
                               .ToList());
        }

        public void Clear()
        {
            bugs.Clear();
        }


        private IList<FakeBug> GetCachedBugs()
        {
            if (DateTime.Now > nextRefreshTime)
            {
                var users = Settings.Default.UsersName.Split(';').Select(u => u.Trim()).ToList();
                if (bugs.Any())
                    foreach (var bug in bugs)
                        RandomUpdateBug(bug, users);
                else
                    bugs.Add(RandomCreateBug(users));

                nextRefreshTime = DateTime.Now.AddMinutes(Settings.Default.BugsRefreshMinutes);
            }

            return bugs;
        }

        private FakeBug RandomCreateBug(IList<string> users)
        {
            var titlePostfix = "";
            if (RandomNumberHelper.IsLucky(20)) { titlePostfix = " postfix"; }
            var discriptionPostfix = "";
            if (RandomNumberHelper.IsLucky(20)) { discriptionPostfix = " postfix"; }
            var assignedTo = RandomNumberHelper.GetLuckyItem(users);
            var createBy = RandomNumberHelper.GetLuckyItem(users);
            var priority = RandomNumberHelper.GetLuckyEnum<FakeBugPriority>(typeof(FakeBugPriority));
            var severity = RandomNumberHelper.GetLuckyEnum<FakeBugSeverity>(typeof(FakeBugSeverity));

            return new FakeBug()
            {
                Id = index,
                Title = string.Format("Title for Bug {0}{1}", index, titlePostfix),
                Description = string.Format("Describe for Bug {0}{1}", index++, discriptionPostfix),
                AssignedTo = assignedTo,
                State = FakeBugState.Design,
                LastChangedDate = DateTime.Now,
                CreatedBy = createBy,
                Priority = priority,
                Severity = severity
            };
        }

        private void RandomUpdateBug(FakeBug bug, IList<string> users)
        {
            if (bug.State == FakeBugState.Closed) { return; }

            var originalBug = bug.Clone();
            bug.Title += RandomNumberHelper.IsLucky(5) ? " more postfix" : "";
            bug.Description += RandomNumberHelper.IsLucky(20) ? " more postfix" : "";
            bug.AssignedTo = RandomNumberHelper.IsLucky(20) ? RandomNumberHelper.GetLuckyItem(users) : bug.AssignedTo;
            bug.State = RandomNumberHelper.IsLucky(10) ? bug.State + 1 : bug.State;
            bug.Priority = RandomNumberHelper.IsLucky(5) ? RandomNumberHelper.GetLuckyEnum<FakeBugPriority>(typeof(FakeBugPriority)) : bug.Priority;
            bug.Severity = RandomNumberHelper.IsLucky(5) ? RandomNumberHelper.GetLuckyEnum<FakeBugSeverity>(typeof(FakeBugSeverity)) : bug.Severity;

            if (bug != originalBug)
            {
                bug.LastChangedDate = DateTime.Now;
            }
        }
    }
}
