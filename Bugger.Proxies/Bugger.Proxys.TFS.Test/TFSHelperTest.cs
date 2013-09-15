using BigEgg.Framework.UnitTesting;
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

        protected override void OnTestInitialize()
        {
            this.tfsHelper = new TFSHelper();
        }

        [TestMethod]
        public void TestConnnectionExceptionTest()
        {
            AssertHelper.ExpectedException<ArgumentNullException>(() => this.tfsHelper.TestConnection(null, null, null));
            AssertHelper.ExpectedException<ArgumentNullException>(() => this.tfsHelper.TestConnection(null, " ", null));
            AssertHelper.ExpectedException<ArgumentNullException>(() => this.tfsHelper.TestConnection(null, null, " "));
            AssertHelper.ExpectedException<ArgumentNullException>(() => this.tfsHelper.TestConnection(null, " ", " "));
            AssertHelper.ExpectedException<ArgumentException>(
                () => this.tfsHelper.TestConnection(new Uri("https://tfs.codeplex.com:443/tfs/TFS12"), " ", null));
            AssertHelper.ExpectedException<ArgumentException>(
                () => this.tfsHelper.TestConnection(new Uri("https://tfs.codeplex.com:443/tfs/TFS12"), null, " "));
            AssertHelper.ExpectedException<ArgumentException>(
                () => this.tfsHelper.TestConnection(new Uri("https://tfs.codeplex.com:443/tfs/TFS12"), " ", " "));
        }

        [TestMethod]
        public void TestConnectionTest()
        {
            Assert.IsFalse(this.tfsHelper.IsConnected());
            var result = this.tfsHelper.TestConnection(new Uri("https://tfs.codeplex.com:443/tfs/TFS12"), "snd\\BigEgg_cp", "123");
            Assert.IsFalse(result);
            Assert.IsFalse(this.tfsHelper.IsConnected());

            result = this.tfsHelper.TestConnection(new Uri("https://tfs.codeplex.com:443/tfs/TFS12"), "snd\\BigEgg_cp", password);
            Assert.IsTrue(result);
            Assert.IsTrue(this.tfsHelper.IsConnected());
        }

        [TestMethod]
        public void GetFieldsTest()
        {
            Assert.IsFalse(this.tfsHelper.IsConnected());
            AssertHelper.ExpectedException<InvalidOperationException>(() => this.tfsHelper.GetFields());
            this.tfsHelper.TestConnection(new Uri("https://tfs.codeplex.com:443/tfs/TFS12"), "snd\\BigEgg_cp", password);
            Assert.IsTrue(this.tfsHelper.IsConnected());

            var fields = this.tfsHelper.GetFields();
            Assert.IsNotNull(fields);
            Assert.IsTrue(fields.Any());
        }

        [TestMethod]
        public void GetBugsExceptionTest()
        {
            AssertHelper.ExpectedException<ArgumentException>(
                () => this.tfsHelper.GetBugs(null, true, null, null, null, null));
            AssertHelper.ExpectedException<ArgumentException>(
                () => this.tfsHelper.GetBugs(" ", true, null, null, null, null));
            AssertHelper.ExpectedException<ArgumentNullException>(
                () => this.tfsHelper.GetBugs("bigegg", true, null, null, null, null));
            AssertHelper.ExpectedException<ArgumentException>(
                () => this.tfsHelper.GetBugs("bigegg", true, new PropertyMappingDictionary(), null, null, null));
            AssertHelper.ExpectedException<ArgumentException>(
                () => this.tfsHelper.GetBugs("bigegg", true, new PropertyMappingDictionary(), " ", null, null));
            AssertHelper.ExpectedException<ArgumentException>(
                () => this.tfsHelper.GetBugs("bigegg", true, new PropertyMappingDictionary(), "Work Item Type", null, null));
            AssertHelper.ExpectedException<ArgumentException>(
                () => this.tfsHelper.GetBugs("bigegg", true, new PropertyMappingDictionary(), "Work Item Type", " ", null));
            AssertHelper.ExpectedException<ArgumentNullException>(
                () => this.tfsHelper.GetBugs("bigegg", true, new PropertyMappingDictionary(), "Work Item Type", "Work Item", null));
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

            Assert.IsFalse(this.tfsHelper.IsConnected());
            AssertHelper.ExpectedException<InvalidOperationException>(
                () => this.tfsHelper.GetBugs(
                    "BigEgg_cp", true, propertyMappingCollection, "Work Item Type", "Work Item", new List<string>()));
            this.tfsHelper.TestConnection(new Uri("https://tfs.codeplex.com:443/tfs/TFS12"), "snd\\BigEgg_cp", password);
            Assert.IsTrue(this.tfsHelper.IsConnected());

            var bugs = this.tfsHelper.GetBugs(
                "BigEgg_cp", true, propertyMappingCollection, "Work Item Type", "Work Item", new List<string>());

            Assert.IsNotNull(bugs);
            Assert.IsTrue(bugs.Any());
        }
    }
}
