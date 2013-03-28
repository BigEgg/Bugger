using Bugger.Proxys.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Bugger.Proxys.Test.Models
{
    [TestClass]
    public class WorkItemTest
    {
        [TestMethod]
        public void GeneralWorkItemTest()
        {
            Bug item = new Bug()
            {
                ID           = 123,
                Title        = "Bug A",
                Description  = "Bug Description.",
                AssignedTo   = "BigEgg",
                State        = "Active",
                ChangedDate  = DateTime.Today,
                CreatedBy    = "BigEgg",
                Priority     = "High",
                Severity     = "1"
            };

            Assert.AreEqual(123, item.ID);
            Assert.AreEqual("Bug A", item.Title);
            Assert.AreEqual("Bug Description.", item.Description);
            Assert.AreEqual("BigEgg", item.AssignedTo);
            Assert.AreEqual("Active", item.State);
            Assert.AreEqual(DateTime.Today, item.ChangedDate);
            Assert.AreEqual("BigEgg", item.CreatedBy);
            Assert.AreEqual("High", item.Priority);
            Assert.AreEqual("1", item.Severity);
        }

        [TestMethod]
        public void EqualsTest()
        {
            Bug item1 = new Bug()
            {
                ID = 123,
                Title = "Bug A",
                Description = "Bug Description.",
                AssignedTo = "BigEgg",
                State = "Active",
                ChangedDate = DateTime.Today,
                CreatedBy = "BigEgg",
                Priority = "High",
                Severity = "1"
            };

            Bug item2 = new Bug()
            {
                ID = 124,
                Title = "Bug A",
                Description = "Bug Description.",
                AssignedTo = "BigEgg",
                State = "Active",
                ChangedDate = DateTime.Today,
                CreatedBy = "BigEgg",
                Priority = "High",
                Severity = "1"
            };

            Bug item3 = new Bug()
            {
                ID = 124,
                Title = "Bug A",
                Description = "Bug Description.",
                AssignedTo = "BigEgg",
                State = "Active",
                ChangedDate = DateTime.Today,
                CreatedBy = "BigEgg",
                Priority = "High",
                Severity = "1"
            };

            Assert.IsFalse(item1.Equals(item2));
            Assert.IsTrue(item2.Equals(item3));
        }
    }
}
