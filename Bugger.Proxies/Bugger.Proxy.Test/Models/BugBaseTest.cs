using BigEgg.Framework.UnitTesting;
using Bugger.Domain.Models;
using Bugger.Proxy.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bugger.Proxy.Test.Models
{
    [TestClass]
    public class BugBaseTest
    {
        [TestMethod]
        public void GeneralBugTest()
        {
            MockBug bug = new MockBug();

            Assert.AreEqual(BugType.Yellow, bug.Type);
            Assert.IsFalse(bug.IsUpdate);

            AssertHelper.PropertyChangedEvent(bug, x => x.Type, () => bug.Type = BugType.Red);
            AssertHelper.PropertyChangedEvent(bug, x => x.IsUpdate, () => bug.PubIsUpdate = true);

            Assert.AreEqual(BugType.Red, bug.Type);
            Assert.IsTrue(bug.IsUpdate);
        }


        private class MockBug : BugBase
        {
            /// <summary>
            /// Gets or sets a value indicating whether [pub is update].
            /// </summary>
            /// <value>
            ///   <c>true</c> if [pub is update]; otherwise, <c>false</c>.
            /// </value>
            public bool PubIsUpdate
            {
                get { return this.IsUpdate; }
                set { this.IsUpdate = value; }
            }

            /// <summary>
            /// Checks is the bug had been the updated.
            /// If true, set the IsUpdate property to <c>true</c>.
            /// </summary>
            /// <param name="oldModel">The old bug.</param>
            public override void CheckIsUpdate(IBug oldModel)
            {
            }
        }
    }
}
