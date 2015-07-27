using BigEgg.Framework.Applications.UnitTesting;
using Bugger.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Bugger.Domain.Test.Models
{
    [TestClass]
    public class BugModelTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            var bug = new Bug("123", "Bug A", "Bug Description.", "BigEgg", "Active", DateTime.Today, "BigEgg", "High", "1");

            var model1 = new BugModel(bug, BugPriorityLevel.Red);
            Assert.AreEqual(bug, model1.Bug);
            Assert.AreEqual(BugPriorityLevel.Red, model1.PriorityLevel);
            Assert.AreEqual(BugModifyState.Normal, model1.ModifiedState);

            var model2 = new BugModel(bug, BugPriorityLevel.Red, (a, b) => { return 0; });
            Assert.AreEqual(bug, model2.Bug);
            Assert.AreEqual(BugPriorityLevel.Red, model2.PriorityLevel);
            Assert.AreEqual(BugModifyState.Normal, model2.ModifiedState);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTest_BugNull()
        {
            new BugModel(null, BugPriorityLevel.Red);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTest_WithPriorityCompareFunc_BugNull()
        {
            new BugModel(null, BugPriorityLevel.Red, (a, b) => { return 0; });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTest_PriorityCompareFuncNull()
        {
            var bug = new Bug("123", "Bug A", "Bug Description.", "BigEgg", "Active", DateTime.Today, "BigEgg", "High", "1");
            new BugModel(bug, BugPriorityLevel.Red, null);
        }

        [TestMethod]
        public void ModifiedStateTest()
        {
            var bug = new Bug("123", "Bug A", "Bug Description.", "BigEgg", "Active", DateTime.Today, "BigEgg", "High", "1");
            var model = new BugModel(bug, BugPriorityLevel.Red);

            AssertHelper.IsRaisePropertyChangedEvent(model, m => m.ModifiedState, () => model.ModifiedState = BugModifyState.New);
        }

        [TestMethod]
        public void CompareToTest_DifferentPriorityLavel()
        {
            var bug = new Bug("123", "Bug A", "Bug Description.", "BigEgg", "Active", DateTime.Today, "BigEgg", "High", "1");
            var model1 = new BugModel(bug, BugPriorityLevel.Red);
            var model2 = new BugModel(bug, BugPriorityLevel.Yellow);


            Assert.AreEqual(1, model1.CompareTo(model2));
            Assert.AreEqual(-1, model2.CompareTo(model1));
        }

        [TestMethod]
        public void CompareToTest_SameRedPriorityLavel_DifferentModifiedTime()
        {
            var bug1 = new Bug("123", "Bug A", "Bug Description.", "BigEgg", "Active", DateTime.Today, "BigEgg", "High", "1");
            var model1 = new BugModel(bug1, BugPriorityLevel.Red);
            var bug2 = new Bug("123", "Bug A", "Bug Description.", "BigEgg", "Active", DateTime.Today.AddDays(-1), "BigEgg", "High", "1");
            var model2 = new BugModel(bug2, BugPriorityLevel.Red);

            Assert.AreEqual(1, model1.CompareTo(model2));
            Assert.AreEqual(-1, model2.CompareTo(model1));
        }

        [TestMethod]
        public void CompareToTest_SameYellowPriorityLavel_DifferentModifiedTime()
        {
            var bug1 = new Bug("123", "Bug A", "Bug Description.", "BigEgg", "Active", DateTime.Today, "BigEgg", "High", "1");
            var model1 = new BugModel(bug1, BugPriorityLevel.Yellow);
            var bug2 = new Bug("123", "Bug A", "Bug Description.", "BigEgg", "Active", DateTime.Today.AddDays(-1), "BigEgg", "High", "1");
            var model2 = new BugModel(bug2, BugPriorityLevel.Yellow);

            Assert.AreEqual(1, model1.CompareTo(model2));
            Assert.AreEqual(-1, model2.CompareTo(model1));
        }

        [TestMethod]
        public void CompareToTest_Same_WithoutPriorityCompareFunc()
        {
            var bug1 = new Bug("123", "Bug A", "Bug Description.", "BigEgg", "Active", DateTime.Today, "BigEgg", "High", "1");
            var model1 = new BugModel(bug1, BugPriorityLevel.Yellow);
            var bug2 = new Bug("123", "Bug A", "Bug Description.", "BigEgg", "Active", DateTime.Today, "BigEgg", "Medium", "1");
            var model2 = new BugModel(bug2, BugPriorityLevel.Yellow);

            Assert.AreEqual(0, model1.CompareTo(model2));
            Assert.AreEqual(0, model2.CompareTo(model1));
        }

        [TestMethod]
        public void CompareToTest_Same_WithPriorityCompareFunc()
        {
            Func<string, string, int> func = (a, b) => { return 5; };
            var bug1 = new Bug("123", "Bug A", "Bug Description.", "BigEgg", "Active", DateTime.Today, "BigEgg", "High", "1");
            var model1 = new BugModel(bug1, BugPriorityLevel.Yellow, func);
            var bug2 = new Bug("123", "Bug A", "Bug Description.", "BigEgg", "Active", DateTime.Today, "BigEgg", "Medium", "1");
            var model2 = new BugModel(bug2, BugPriorityLevel.Yellow, func);

            Assert.AreEqual(5, model1.CompareTo(model2));
            Assert.AreEqual(5, model2.CompareTo(model1));
        }
    }
}
