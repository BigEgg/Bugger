using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bugger.Applications.Models;
using Bugger.Presentation.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bugger.Presentation.Test.Conveters
{
    [TestClass]
    public class SettingDialogStatusToIndeterminateConverterTest
    {
        [TestMethod]
        public void ConvertTest()
        {
            var converter = SettingDialogStatusToIndeterminateConverter.Default;

            var result = (bool)converter.Convert(SettingDialogStatus.InitiatingProxyFailed, null, null, null);
            Assert.IsFalse(result);

            result = (bool)converter.Convert(SettingDialogStatus.InitiatingProxy, null, null, null);
            Assert.IsTrue(result);

            result = (bool)converter.Convert(SettingDialogStatus.NotWorking, null, null, null);
            Assert.IsFalse(result);

            result = (bool)converter.Convert(SettingDialogStatus.ProxyValid, null, null, null);
            Assert.IsFalse(result);

            result = (bool)converter.Convert(SettingDialogStatus.ValidatingProxySettings, null, null, null);
            Assert.IsTrue(result);

            result = (bool)converter.Convert(SettingDialogStatus.ProxyBusy, null, null, null);
            Assert.IsFalse(result);

            result = (bool)converter.Convert(SettingDialogStatus.ProxyCannotConnect, null, null, null);
            Assert.IsFalse(result);

            result = (bool)converter.Convert(SettingDialogStatus.ProxyUnvalid, null, null, null);
            Assert.IsFalse(result);
        }
    }
}
