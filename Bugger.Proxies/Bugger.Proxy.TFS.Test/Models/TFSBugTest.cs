using BigEgg.Framework.UnitTesting;
using Bugger.Domain.Models;
using Bugger.Proxy.TFS.ViewModels;
using Bugger.Proxy.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Bugger.Proxy.TFS.Test.Models
{
    [TestClass]
    public class TFSBugTest
    {
        [TestMethod]
        public void GeneralTFSBugTest()
        {
            TFSBug bug = new TFSBug()
            {
                ID = 1,
                Title = "Bug1",
                Description = "Description for Bug1.",
                Type = BugType.Red,
                AssignedTo = "BigEgg",
                State = "Design",
                ChangedDate = new DateTime(2013, 4, 10),
                CreatedBy = "BigEgg",
                Priority = "High",
                Severity = ""
            };

            Assert.AreEqual(1, bug.ID);
            Assert.AreEqual("Bug1", bug.Title);
            Assert.AreEqual("Description for Bug1.", bug.Description);
            Assert.AreEqual(BugType.Red, bug.Type);
            Assert.AreEqual("BigEgg", bug.AssignedTo);
            Assert.AreEqual("Design", bug.State);
            Assert.AreEqual(new DateTime(2013, 4, 10), bug.ChangedDate);
            Assert.AreEqual("BigEgg", bug.CreatedBy);
            Assert.AreEqual("High", bug.Priority);
            Assert.AreEqual("", bug.Severity);
            Assert.IsFalse(bug.IsUpdate);
        }

        [TestMethod]
        public void CheckIsUpdateExeptionTest()
        {
            TFSBug bug1 = new TFSBug()
            {
                ID = 1,
                Title = "Bug1",
                Description = "Description for Bug1.",
                Type = BugType.Red,
                AssignedTo = "BigEgg",
                State = "Design",
                ChangedDate = new DateTime(2013, 4, 10),
                CreatedBy = "BigEgg",
                Priority = "High",
                Severity = ""
            };
            TFSBug bug2 = new TFSBug()
            {
                ID = 2,
                Title = "Bug2",
                Description = "Description for Bug2.",
                Type = BugType.Red,
                AssignedTo = "BigEgg",
                State = "Design",
                ChangedDate = new DateTime(2013, 4, 10),
                CreatedBy = "BigEgg",
                Priority = "High",
                Severity = ""
            };
            TFSBug alteredBug = new TFSBug()
            {
                ID = 1,
                Title = "AlteredBug",
                Description = "Description for Bug1.",
                Type = BugType.Red,
                AssignedTo = "BigEgg",
                State = "Design",
                ChangedDate = new DateTime(2013, 4, 10),
                CreatedBy = "BigEgg",
                Priority = "High",
                Severity = ""
            };

            AssertHelper.ExpectedException<ArgumentNullException>(() => bug1.CheckIsUpdate(null));
            AssertHelper.ExpectedException<ArgumentException>(() => bug1.CheckIsUpdate(new MockBug()));
            AssertHelper.ExpectedException<ArgumentException>(() => bug1.CheckIsUpdate(bug2));
        }

        [TestMethod]
        public void CheckIsUpdateTest()
        {
            TFSBug bug = new TFSBug()
            {
                ID = 1,
                Title = "Bug1",
                Description = "Description for Bug1.",
                Type = BugType.Red,
                AssignedTo = "BigEgg",
                State = "Design",
                ChangedDate = new DateTime(2013, 4, 10),
                CreatedBy = "BigEgg",
                Priority = "High",
                Severity = ""
            };
            TFSBug alteredBug = new TFSBug()
            {
                ID = 1,
                Title = "Bug1",
                Description = "Description for Bug1.",
                Type = BugType.Red,
                AssignedTo = "BigEgg",
                State = "Design",
                ChangedDate = new DateTime(2013, 4, 10),
                CreatedBy = "BigEgg",
                Priority = "High",
                Severity = ""
            };

            Assert.IsFalse(alteredBug.IsUpdate);
            alteredBug.CheckIsUpdate(bug);
            Assert.IsFalse(alteredBug.IsUpdate);
        }

        [TestMethod]
        public void CheckIsUpdateTest_Title()
        {
            TFSBug bug = new TFSBug()
            {
                ID = 1,
                Title = "Bug1",
                Description = "Description for Bug1.",
                Type = BugType.Red,
                AssignedTo = "BigEgg",
                State = "Design",
                ChangedDate = new DateTime(2013, 4, 10),
                CreatedBy = "BigEgg",
                Priority = "High",
                Severity = ""
            };
            TFSBug alteredBug = new TFSBug()
            {
                ID = 1,
                Title = "AlteredBug",
                Description = "Description for Bug1.",
                Type = BugType.Red,
                AssignedTo = "BigEgg",
                State = "Design",
                ChangedDate = new DateTime(2013, 4, 10),
                CreatedBy = "BigEgg",
                Priority = "High",
                Severity = ""
            };

            Assert.IsFalse(alteredBug.IsUpdate);
            alteredBug.CheckIsUpdate(bug);
            Assert.IsTrue(alteredBug.IsUpdate);
        }

        [TestMethod]
        public void CheckIsUpdateTest_Description()
        {
            TFSBug bug = new TFSBug()
            {
                ID = 1,
                Title = "Bug1",
                Description = "Description for Bug1.",
                Type = BugType.Red,
                AssignedTo = "BigEgg",
                State = "Design",
                ChangedDate = new DateTime(2013, 4, 10),
                CreatedBy = "BigEgg",
                Priority = "High",
                Severity = ""
            };
            TFSBug alteredBug = new TFSBug()
            {
                ID = 1,
                Title = "Bug1",
                Description = "Description for Altered Bug.",
                Type = BugType.Red,
                AssignedTo = "BigEgg",
                State = "Design",
                ChangedDate = new DateTime(2013, 4, 10),
                CreatedBy = "BigEgg",
                Priority = "High",
                Severity = ""
            };

            Assert.IsFalse(alteredBug.IsUpdate);
            alteredBug.CheckIsUpdate(bug);
            Assert.IsTrue(alteredBug.IsUpdate);
        }

        [TestMethod]
        public void CheckIsUpdateTest_AssignedTo()
        {
            TFSBug bug = new TFSBug()
            {
                ID = 1,
                Title = "Bug1",
                Description = "Description for Bug1.",
                Type = BugType.Red,
                AssignedTo = "BigEgg",
                State = "Design",
                ChangedDate = new DateTime(2013, 4, 10),
                CreatedBy = "BigEgg",
                Priority = "High",
                Severity = ""
            };
            TFSBug alteredBug = new TFSBug()
            {
                ID = 1,
                Title = "Bug1",
                Description = "Description for Bug1.",
                Type = BugType.Red,
                AssignedTo = "Pupil",
                State = "Design",
                ChangedDate = new DateTime(2013, 4, 10),
                CreatedBy = "BigEgg",
                Priority = "High",
                Severity = ""
            };

            Assert.IsFalse(alteredBug.IsUpdate);
            alteredBug.CheckIsUpdate(bug);
            Assert.IsTrue(alteredBug.IsUpdate);
        }

        [TestMethod]
        public void CheckIsUpdateTest_State()
        {
            TFSBug bug = new TFSBug()
            {
                ID = 1,
                Title = "Bug1",
                Description = "Description for Bug1.",
                Type = BugType.Red,
                AssignedTo = "BigEgg",
                State = "Design",
                ChangedDate = new DateTime(2013, 4, 10),
                CreatedBy = "BigEgg",
                Priority = "High",
                Severity = ""
            };
            TFSBug alteredBug = new TFSBug()
            {
                ID = 1,
                Title = "Bug1",
                Description = "Description for Bug1.",
                Type = BugType.Red,
                AssignedTo = "BigEgg",
                State = "Resolve",
                ChangedDate = new DateTime(2013, 4, 10),
                CreatedBy = "BigEgg",
                Priority = "High",
                Severity = ""
            };

            Assert.IsFalse(alteredBug.IsUpdate);
            alteredBug.CheckIsUpdate(bug);
            Assert.IsTrue(alteredBug.IsUpdate);
        }

        [TestMethod]
        public void CheckIsUpdateTest_ChangedDate()
        {
            TFSBug bug = new TFSBug()
            {
                ID = 1,
                Title = "Bug1",
                Description = "Description for Bug1.",
                Type = BugType.Red,
                AssignedTo = "BigEgg",
                State = "Design",
                ChangedDate = new DateTime(2013, 4, 10),
                CreatedBy = "BigEgg",
                Priority = "High",
                Severity = ""
            };
            TFSBug alteredBug = new TFSBug()
            {
                ID = 1,
                Title = "Bug1",
                Description = "Description for Bug1.",
                Type = BugType.Red,
                AssignedTo = "BigEgg",
                State = "Design",
                ChangedDate = new DateTime(2014, 2, 16),
                CreatedBy = "BigEgg",
                Priority = "High",
                Severity = ""
            };

            Assert.IsFalse(alteredBug.IsUpdate);
            alteredBug.CheckIsUpdate(bug);
            Assert.IsTrue(alteredBug.IsUpdate);
        }

        [TestMethod]
        public void CheckIsUpdateTest_CreatedBy()
        {
            TFSBug bug = new TFSBug()
            {
                ID = 1,
                Title = "Bug1",
                Description = "Description for Bug1.",
                Type = BugType.Red,
                AssignedTo = "BigEgg",
                State = "Design",
                ChangedDate = new DateTime(2013, 4, 10),
                CreatedBy = "BigEgg",
                Priority = "High",
                Severity = ""
            };
            TFSBug alteredBug = new TFSBug()
            {
                ID = 1,
                Title = "Bug1",
                Description = "Description for Bug1.",
                Type = BugType.Red,
                AssignedTo = "BigEgg",
                State = "Design",
                ChangedDate = new DateTime(2013, 4, 10),
                CreatedBy = "Pupil",
                Priority = "High",
                Severity = ""
            };

            Assert.IsFalse(alteredBug.IsUpdate);
            alteredBug.CheckIsUpdate(bug);
            Assert.IsTrue(alteredBug.IsUpdate);
        }

        [TestMethod]
        public void CheckIsUpdateTest_Priority()
        {
            TFSBug bug = new TFSBug()
            {
                ID = 1,
                Title = "Bug1",
                Description = "Description for Bug1.",
                Type = BugType.Red,
                AssignedTo = "BigEgg",
                State = "Design",
                ChangedDate = new DateTime(2013, 4, 10),
                CreatedBy = "BigEgg",
                Priority = "High",
                Severity = ""
            };
            TFSBug alteredBug = new TFSBug()
            {
                ID = 1,
                Title = "Bug1",
                Description = "Description for Bug1.",
                Type = BugType.Red,
                AssignedTo = "BigEgg",
                State = "Design",
                ChangedDate = new DateTime(2013, 4, 10),
                CreatedBy = "BigEgg",
                Priority = "Low",
                Severity = ""
            };

            Assert.IsFalse(alteredBug.IsUpdate);
            alteredBug.CheckIsUpdate(bug);
            Assert.IsTrue(alteredBug.IsUpdate);
        }

        [TestMethod]
        public void CheckIsUpdateTest_Severity()
        {
            TFSBug bug = new TFSBug()
            {
                ID = 1,
                Title = "Bug1",
                Description = "Description for Bug1.",
                Type = BugType.Red,
                AssignedTo = "BigEgg",
                State = "Design",
                ChangedDate = new DateTime(2013, 4, 10),
                CreatedBy = "BigEgg",
                Priority = "High",
                Severity = ""
            };
            TFSBug alteredBug = new TFSBug()
            {
                ID = 1,
                Title = "Bug1",
                Description = "Description for Bug1.",
                Type = BugType.Red,
                AssignedTo = "BigEgg",
                State = "Design",
                ChangedDate = new DateTime(2013, 4, 10),
                CreatedBy = "BigEgg",
                Priority = "High",
                Severity = "High"
            };

            Assert.IsFalse(alteredBug.IsUpdate);
            alteredBug.CheckIsUpdate(bug);
            Assert.IsTrue(alteredBug.IsUpdate);
        }


        private class MockBug : BugBase
        {
            public override void CheckIsUpdate(IBug oldModel)
            {
            }
        }
    }
}
