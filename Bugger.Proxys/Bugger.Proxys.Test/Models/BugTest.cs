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
    }
}
