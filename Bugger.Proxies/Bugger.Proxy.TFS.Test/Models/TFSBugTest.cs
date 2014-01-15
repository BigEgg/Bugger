using BigEgg.Framework.UnitTesting;
using Bugger.Domain.Models;
using Bugger.Proxy.TFS.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Bugger.Proxy.TFS.Test.Models
{
    [TestClass]
    public class TFSBugTest
    {
        [TestMethod]
        public void GeneralWorkItemTest()
        {
            var defaultItem = new TFSBug();
            Assert.AreEqual(BugType.Yellow, defaultItem.Type);

            var item = new TFSBug()
            {
                ID = 123,
                Title = "Bug A",
                Description = "Bug Description.",
                Type = BugType.Red,
                AssignedTo = "BigEgg",
                State = "Active",
                ChangedDate = DateTime.Today,
                CreatedBy = "BigEgg",
                Priority = "High",
                Severity = "1"
            };

            Assert.AreEqual(123, item.ID);
            Assert.AreEqual("Bug A", item.Title);
            Assert.AreEqual("Bug Description.", item.Description);
            Assert.AreEqual(BugType.Red, item.Type);
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
            var item1 = new TFSBug()
            {
                ID = 123,
                Title = "Bug A",
                Description = "Bug Description.",
                Type = BugType.Red,
                AssignedTo = "BigEgg",
                State = "Active",
                ChangedDate = DateTime.Today,
                CreatedBy = "BigEgg",
                Priority = "High",
                Severity = "1"
            };

            var item2 = new TFSBug()
            {
                ID = 124,
                Title = "Bug A",
                Description = "Bug Description.",
                Type = BugType.Red,
                AssignedTo = "BigEgg",
                State = "Active",
                ChangedDate = DateTime.Today,
                CreatedBy = "BigEgg",
                Priority = "High",
                Severity = "1"
            };

            var item3 = new TFSBug()
            {
                ID = 124,
                Title = "Bug A",
                Description = "Bug Description.",
                Type = BugType.Red,
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

        [TestMethod]
        public void CheckUpdateTest()
        {
            var item1 = new TFSBug()
            {
                ID = 123,
                Title = "Bug A",
                Description = "Bug Description.",
                Type = BugType.Red,
                AssignedTo = "BigEgg",
                State = "Active",
                ChangedDate = DateTime.Today,
                CreatedBy = "BigEgg",
                Priority = "High",
                Severity = "1"
            };

            var item2 = new TFSBug()
            {
                ID = 124,
                Title = "Bug A",
                Description = "Bug Description.",
                Type = BugType.Red,
                AssignedTo = "BigEgg",
                State = "Active",
                ChangedDate = DateTime.Today,
                CreatedBy = "BigEgg",
                Priority = "High",
                Severity = "1"
            };

            var item3 = new TFSBug()
            {
                ID = 124,
                Title = "Bug A",
                Description = "Bug Description Changed.",
                Type = BugType.Red,
                AssignedTo = "BigEgg",
                State = "Active",
                ChangedDate = DateTime.Today,
                CreatedBy = "BigEgg",
                Priority = "High",
                Severity = "1"
            };

            Assert.IsFalse(item3.IsUpdate);
            AssertHelper.ExpectedException<ArgumentNullException>(() => item3.CheckUpdate(null));
            AssertHelper.ExpectedException<NotSupportedException>(() => item3.CheckUpdate(new MockBug()));
            AssertHelper.ExpectedException<ArgumentException>(() => item3.CheckUpdate(item2));

            item3.CheckUpdate(item1);
            Assert.IsTrue(item3.IsUpdate);
        }

        private class MockBug : IBug
        {
            /// <summary>
            /// Gets or sets the type of this bug.
            /// </summary>
            /// <value>
            /// The type of this bug.
            /// </value>
            public BugType Type { get; set; }

            /// <summary>
            /// Checks is the new bug model had been the updated.
            /// If true, set the IsUpdate property to <c>true</c>.
            /// </summary>
            /// <param name="oldModel">The old bug model.</param>
            /// <exception cref="System.NotImplementedException"></exception>
            public void CheckUpdate(IBug oldModel)
            {
                throw new NotImplementedException();
            }
        }
    }
}
