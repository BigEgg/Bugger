using Bugger.Models;
using Bugger.Proxy.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Bugger.Domain.Test.Models
{
    [TestClass]
    public class WorkItemTest
    {
        [TestMethod]
        public void GeneralWorkItemTest()
        {
            Bug item = new Bug("123", "Bug A", "Bug Description.", "BigEgg", "Active", DateTime.Today, "BigEgg", "High", "1");
            Assert.AreEqual("123", item.ID);
            Assert.AreEqual("Bug A", item.Title);
            Assert.AreEqual("Bug Description.", item.Description);
            Assert.AreEqual(BugType.Yellow, item.Type);
            Assert.AreEqual("BigEgg", item.AssignedTo);
            Assert.AreEqual("Active", item.State);
            Assert.AreEqual(DateTime.Today, item.ChangedDate);
            Assert.AreEqual("BigEgg", item.CreatedBy);
            Assert.AreEqual("High", item.Priority);
            Assert.AreEqual("1", item.Severity);

            item.Type = BugType.Red;
            Assert.AreEqual(BugType.Red, item.Type);
        }

        [TestMethod]
        public void EqualsTest()
        {
            Bug item1 = new Bug("123", "Bug A", "Bug Description.", "BigEgg", "Active", DateTime.Today, "BigEgg", "High", "1");
            Bug item2 = new Bug("124", "Bug B", "Bug Description.", "BigEgg", "Active", DateTime.Today, "BigEgg", "High", "1");
            Bug item3 = new Bug("124", "Bug B", "Bug Description B.", "Pupil", "Active", DateTime.Today, "BigEgg", "High", "2");

            Assert.IsFalse(item1.Equals(item2));
            Assert.IsTrue(item2.Equals(item3));
        }
    }
}
