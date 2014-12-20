using Bugger.Applications.Services;
using Bugger.Applications.ViewModels;
using Bugger.Applications.Views;
using Bugger.Base.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Bugger.Applications.Test.ViewModels
{
    [TestClass]
    public class UserBugsViewModelTest : TestClassBase
    {
        [TestMethod]
        public void GenaralUserBugsViewModelTest()
        {
            IUserBugsView view = Container.GetExportedValue<IUserBugsView>();
            IDataService dataService = Container.GetExportedValue<IDataService>();

            UserBugsViewModel viewModel = new UserBugsViewModel(view, dataService);
            Assert.AreEqual(0, viewModel.Bugs.Count);

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
            Assert.AreEqual(2, viewModel.Bugs.Count);
            Assert.AreEqual(1, viewModel.Bugs.Count(x => x.Type == BugType.Red));
            Assert.AreEqual(1, viewModel.Bugs.Count(x => x.Type == BugType.Yellow));
        }
    }
}
