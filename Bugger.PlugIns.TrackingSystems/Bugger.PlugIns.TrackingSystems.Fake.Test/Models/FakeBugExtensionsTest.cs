using Bugger.PlugIns.TrackingSystems.Fake.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Bugger.PlugIns.TrackingSystems.Fake.Test.Models
{
    [TestClass]
    public class FakeBugExtensionsTest
    {
        [TestMethod]
        public void ToBugTest()
        {
            var changedDate = DateTime.Now;

            var fakeBug = new FakeBug()
            {
                Id = 1,
                Title = "Some thing failed",
                Description = "How to reproduce it",
                AssignedTo = "BigEgg",
                State = FakeBugState.Implement,
                LastChangedDate = changedDate,
                CreatedBy = "BigEgg",
                Priority = FakeBugPriority.High,
                Severity = FakeBugSeverity.Low
            };

            var bug = fakeBug.ToBug();
            Assert.AreEqual("1", bug.ID);
            Assert.AreEqual("Some thing failed", bug.Title);
            Assert.AreEqual("How to reproduce it", bug.Description);
            Assert.AreEqual("BigEgg", bug.AssignedTo);
            Assert.AreEqual("Implement", bug.State);
            Assert.AreEqual(changedDate, bug.LastChangedDate);
            Assert.AreEqual("BigEgg", bug.CreatedBy);
            Assert.AreEqual("High", bug.Priority);
            Assert.AreEqual("Low", bug.Severity);
        }
    }
}
