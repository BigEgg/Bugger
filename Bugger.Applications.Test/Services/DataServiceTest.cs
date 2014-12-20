using BigEgg.Framework.UnitTesting;
using Bugger.Applications.Models;
using Bugger.Applications.Services;
using Bugger.Base.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Bugger.Applications.Test.Services
{
    [TestClass]
    public class DataServiceTest : TestClassBase
    {
        [TestMethod]
        public void GeneralDataServiceTest()
        {
            IDataService dataService = Container.GetExportedValue<IDataService>();

            Assert.AreEqual(dataService.UserBugs.Count, 0);
            Assert.AreEqual(dataService.TeamBugs.Count, 0);
            Assert.AreEqual(dataService.UserBugsProgressValue, 0);
            Assert.AreEqual(dataService.UserBugsQueryState, QueryStatus.NotWorking);
            Assert.AreEqual(dataService.TeamBugsProgressValue, 0);
            Assert.AreEqual(dataService.TeamBugsQueryState, QueryStatus.NotWorking);

            dataService.UserBugs.Add(
                new Bug()
                {
                    ID = 1,
                    Title = "Bug1",
                    Description = "Description for Bug1.",
                    Type = BugType.Red,
                    AssignedTo = "BigEgg",
                    State = "Implement",
                    ChangedDate = new DateTime(2013, 4, 10),
                    CreatedBy = "BigEgg",
                    Priority = "High",
                    Severity = ""
                }
            );

            Assert.AreEqual(dataService.UserBugs.Count, 1);

            dataService.UserBugs.Add(
                new Bug()
                {
                    ID = 6,
                    Title = "Bug6",
                    Description = "Description for Bug6.",
                    AssignedTo = "Pupil",
                    State = "Closed",
                    ChangedDate = new DateTime(2013, 4, 11),
                    CreatedBy = "Pupil",
                    Priority = "High",
                    Severity = "High"
                }
            );

            Assert.AreEqual(dataService.UserBugs.Count, 2);
            Assert.AreEqual(dataService.UserBugs.Count(x => x.Type == BugType.Red), 1);
            Assert.AreEqual(dataService.UserBugs.Count(x => x.Type == BugType.Yellow), 1);

            dataService.TeamBugs.Add(
                new Bug()
                {
                    ID = 5,
                    Title = "Bug5",
                    Description = "Description for Bug5.",
                    Type = BugType.Red,
                    AssignedTo = "BigEgg",
                    State = "Implement",
                    ChangedDate = new DateTime(2013, 4, 11),
                    CreatedBy = "Pupil",
                    Priority = "High",
                    Severity = "High"
                }
            );

            Assert.AreEqual(dataService.TeamBugs.Count, 1);

            DateTime time = new DateTime(2013, 5, 8, 11, 00, 00);
            dataService.RefreshTime = time;
            Assert.AreEqual(time, dataService.RefreshTime);
        }

        [TestMethod]
        public void PropertiesWithNotification()
        {
            IDataService dataService = Container.GetExportedValue<IDataService>();
            DateTime time = new DateTime(2013, 5, 8, 11, 00, 00);

            AssertHelper.PropertyChangedEvent(dataService, x => x.RefreshTime, () =>
                dataService.RefreshTime = time
            );

            Assert.AreEqual(time, dataService.RefreshTime);
        }
    }
}
