using BigEgg.Framework.UnitTesting;
using Bugger.Proxy.TFS.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bugger.Proxy.TFS.Test.Models
{
    [TestClass]
    public class CheckStringTest
    {
        [TestMethod]
        public void GeneralCheckStringTest()
        {
            CheckString checkString = new CheckString("High");
            Assert.AreEqual("High", checkString.Name);
            Assert.IsFalse(checkString.IsChecked);

            checkString.IsChecked = true;
            Assert.IsTrue(checkString.IsChecked);
        }

        [TestMethod]
        public void ConstructorTest()
        {
            AssertHelper.ExpectedException<ArgumentException>(() => new CheckString(null));
            AssertHelper.ExpectedException<ArgumentException>(() => new CheckString(""));
            AssertHelper.ExpectedException<ArgumentException>(() => new CheckString(" "));
        }

        [TestMethod]
        public void PropertiesWithNotification()
        {
            CheckString checkString = new CheckString("High");

            Assert.IsFalse(checkString.IsChecked);
            AssertHelper.PropertyChangedEvent(checkString, x => x.IsChecked, () => checkString.IsChecked = true);
            Assert.IsTrue(checkString.IsChecked);
        }
    }
}
