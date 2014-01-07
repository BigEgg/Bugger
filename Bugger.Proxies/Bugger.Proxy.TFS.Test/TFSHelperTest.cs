using BigEgg.Framework.UnitTesting;
using Microsoft.TeamFoundation.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bugger.Proxy.TFS.Test
{
    [TestClass]
    public class TFSHelperTest : TestClassBase
    {
        private TFSHelper tfsHelper;
        TfsTeamProjectCollection tpc;


        public TFSHelperTest()
        {
            this.tfsHelper = new TFSHelper();
            this.tfsHelper.TryConnection(new Uri(TheCodePlexUri), TheUsername, ThePassword, out tpc);
        }

        [TestMethod]
        public void TryConnnectionExceptionTest()
        {
            TfsTeamProjectCollection tpc = null;
            AssertHelper.ExpectedException<ArgumentNullException>(() => this.tfsHelper.TryConnection(null, null, null, out tpc));
            AssertHelper.ExpectedException<ArgumentNullException>(() => this.tfsHelper.TryConnection(null, " ", null, out tpc));
            AssertHelper.ExpectedException<ArgumentNullException>(() => this.tfsHelper.TryConnection(null, null, " ", out tpc));
            AssertHelper.ExpectedException<ArgumentNullException>(() => this.tfsHelper.TryConnection(null, " ", " ", out tpc));
            AssertHelper.ExpectedException<ArgumentNullException>(
                () => this.tfsHelper.TryConnection(new Uri(TheCodePlexUri), " ", null, out tpc));
            AssertHelper.ExpectedException<ArgumentNullException>(
                () => this.tfsHelper.TryConnection(new Uri(TheCodePlexUri), null, " ", out tpc));
            AssertHelper.ExpectedException<ArgumentNullException>(
                () => this.tfsHelper.TryConnection(new Uri(TheCodePlexUri), " ", " ", out tpc));
            AssertHelper.ExpectedException<ArgumentNullException>(
                () => this.tfsHelper.TryConnection(new Uri(TheCodePlexUri), TheUsername, null, out tpc));
        }

        [TestMethod]
        public void TryConnectionTest()
        {
            TfsTeamProjectCollection tpc = null;
            var result = this.tfsHelper.TryConnection(new Uri(TheCodePlexUri), TheUsername, "123", out tpc);
            Assert.IsFalse(result);
            Assert.IsNull(tpc);

            result = this.tfsHelper.TryConnection(new Uri(TheCodePlexUri), TheUsername, ThePassword, out tpc);
            Assert.IsTrue(result);
            Assert.IsNotNull(tpc);
        }

        [TestMethod]
        public void GetFiledsExceptionTest()
        {
            AssertHelper.ExpectedException<ArgumentNullException>(() => this.tfsHelper.GetFields(null));
        }

        [TestMethod]
        public void GetFieldsTest()
        {
            var fields = this.tfsHelper.GetFields(tpc);
            Assert.IsNotNull(fields);
            Assert.IsTrue(fields.Any());
        }

        [TestMethod]
        public void GetBugsExceptionTest()
        {
            AssertHelper.ExpectedException<ArgumentNullException>(
                () => this.tfsHelper.GetBugs(null, null, true, null, null, null, null));
            AssertHelper.ExpectedException<ArgumentNullException>(
                () => this.tfsHelper.GetBugs(tpc, null, true, null, null, null, null));
            AssertHelper.ExpectedException<ArgumentNullException>(
                () => this.tfsHelper.GetBugs(tpc, " ", true, null, null, null, null));
            AssertHelper.ExpectedException<ArgumentNullException>(
                () => this.tfsHelper.GetBugs(tpc, "bigegg", true, null, null, null, null));
            AssertHelper.ExpectedException<ArgumentNullException>(
                () => this.tfsHelper.GetBugs(tpc, "bigegg", true, new PropertyMappingDictionary(), null, null, null));
            AssertHelper.ExpectedException<ArgumentNullException>(
                () => this.tfsHelper.GetBugs(tpc, "bigegg", true, new PropertyMappingDictionary(), " ", null, null));
            AssertHelper.ExpectedException<ArgumentNullException>(
                () => this.tfsHelper.GetBugs(tpc, "bigegg", true, new PropertyMappingDictionary(), "Work Item Type", null, null));
            AssertHelper.ExpectedException<ArgumentNullException>(
                () => this.tfsHelper.GetBugs(tpc, "bigegg", true, new PropertyMappingDictionary(), "Work Item Type", " ", null));
            AssertHelper.ExpectedException<ArgumentNullException>(
                () => this.tfsHelper.GetBugs(tpc, "bigegg", true, new PropertyMappingDictionary(), "Work Item Type", "Work Item", null));
        }

        [TestMethod]
        public void GetBugsTest()
        {
            var propertyMappingCollection = new PropertyMappingDictionary();
            propertyMappingCollection.Add("ID", "ID");
            propertyMappingCollection.Add("Title", "Title");
            propertyMappingCollection.Add("Description", "Description");
            propertyMappingCollection.Add("AssignedTo", "Assigned To");
            propertyMappingCollection.Add("State", "State");
            propertyMappingCollection.Add("ChangedDate", "Changed Date");
            propertyMappingCollection.Add("CreatedBy", "Created By");
            propertyMappingCollection.Add("Priority", "Code Studio Rank");
            propertyMappingCollection.Add("Severity", string.Empty);

            var bugs = this.tfsHelper.GetBugs(
                tpc, "BigEgg_cp", true, propertyMappingCollection, "Work Item Type", "Work Item", new List<string>());
            Assert.IsNotNull(bugs);
            Assert.IsTrue(bugs.Any());
        }
    }
}
