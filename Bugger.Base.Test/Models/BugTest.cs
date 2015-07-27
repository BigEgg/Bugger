using Bugger.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Bugger.Base.Test.Models
{
    [TestClass]
    public class BugTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            var bug = new Bug("123", "Bug A", "Bug Description.", "BigEgg", "Active", DateTime.Today, "BigEgg", "High", "1");

            Assert.AreEqual("123", bug.ID);
            Assert.AreEqual("Bug A", bug.Title);
            Assert.AreEqual("Bug Description.", bug.Description);
            Assert.AreEqual("BigEgg", bug.AssignedTo);
            Assert.AreEqual("Active", bug.State);
            Assert.AreEqual(DateTime.Today, bug.LastChangedDate);
            Assert.AreEqual("BigEgg", bug.CreatedBy);
            Assert.AreEqual("High", bug.Priority);
            Assert.AreEqual("1", bug.Severity);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorTest_IDNotValid()
        {
            new Bug("  ", "Bug A", "Bug Description.", "BigEgg", "Active", DateTime.Today, "BigEgg", "High", "1");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorTest_TitleNotValid()
        {
            new Bug("123", "  ", "Bug Description.", "BigEgg", "Active", DateTime.Today, "BigEgg", "High", "1");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorTest_DescriptionNotValid()
        {
            new Bug("123", "Bug A", "  ", "BigEgg", "Active", DateTime.Today, "BigEgg", "High", "1");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorTest_AssignedToNotValid()
        {
            new Bug("123", "Bug A", "Bug Description.", "  ", "Active", DateTime.Today, "BigEgg", "High", "1");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorTest_StateNotValid()
        {
            new Bug("123", "Bug A", "Bug Description.", "BigEgg", "  ", DateTime.Today, "BigEgg", "High", "1");
        }

        [TestMethod]
        public void EqualsTest()
        {
            var bug = new Bug("123", "Bug A", "Bug Description.", "BigEgg", "Active", DateTime.Today, "BigEgg", "High", "1");
            var bug1 = new Bug("123", "Bug A", "Bug Description.", "BigEgg", "Active", DateTime.Today, "BigEgg", "High", "1");
            var bug2 = new Bug("124", "Bug A", "Bug Description.", "BigEgg", "Active", DateTime.Today, "BigEgg", "High", "1");
            var bug3 = new Bug("123", "Bug B", "Bug Description.", "BigEgg", "Active", DateTime.Today, "BigEgg", "High", "1");
            var bug4 = new Bug("123", "Bug A", "Bug new description.", "BigEgg", "Active", DateTime.Today, "BigEgg", "High", "1");
            var bug5 = new Bug("123", "Bug A", "Bug Description.", "Sophia", "Active", DateTime.Today, "BigEgg", "High", "1");
            var bug6 = new Bug("123", "Bug A", "Bug Description.", "BigEgg", "Done", DateTime.Today, "BigEgg", "High", "1");
            var bug7 = new Bug("123", "Bug A", "Bug Description.", "BigEgg", "Active", DateTime.Today.AddDays(-1), "BigEgg", "High", "1");
            var bug8 = new Bug("123", "Bug A", "Bug Description.", "BigEgg", "Active", DateTime.Today, "Sophia", "High", "1");
            var bug9 = new Bug("123", "Bug A", "Bug Description.", "BigEgg", "Active", DateTime.Today, "BigEgg", "Medium", "1");
            var bug10 = new Bug("123", "Bug A", "Bug Description.", "BigEgg", "Active", DateTime.Today, "BigEgg", "High", "2");

            Assert.IsFalse(bug1.Equals(bug2));
            Assert.IsFalse(bug1.Equals(bug3));
            Assert.IsFalse(bug1.Equals(bug4));
            Assert.IsFalse(bug1.Equals(bug5));
            Assert.IsFalse(bug1.Equals(bug6));
            Assert.IsFalse(bug1.Equals(bug7));
            Assert.IsFalse(bug1.Equals(bug8));
            Assert.IsFalse(bug1.Equals(bug9));
            Assert.IsFalse(bug1.Equals(bug10));
            Assert.IsTrue(bug1.Equals(bug));
        }
    }
}
