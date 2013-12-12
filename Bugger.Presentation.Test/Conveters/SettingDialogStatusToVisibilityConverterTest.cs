using System.Windows;
using Bugger.Applications.Models;
using Bugger.Presentation.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bugger.Presentation.Test.Conveters
{
    [TestClass]
    public class SettingDialogStatusToVisibilityConverterTest
    {
        [TestMethod]
        public void ConvertTest()
        {
            var converter = SettingDialogStatusToVisibilityConverter.Default;

            var result = (Visibility)converter.Convert(SettingDialogStatus.InitiatingProxyFailed, null, null, null);
            Assert.AreEqual(Visibility.Visible, result);

            result = (Visibility)converter.Convert(SettingDialogStatus.InitiatingProxy, null, null, null);
            Assert.AreEqual(Visibility.Visible, result);

            result = (Visibility)converter.Convert(SettingDialogStatus.NotWorking, null, null, null);
            Assert.AreEqual(Visibility.Collapsed, result);

            result = (Visibility)converter.Convert(SettingDialogStatus.ProxyValid, null, null, null);
            Assert.AreEqual(Visibility.Visible, result);

            result = (Visibility)converter.Convert(SettingDialogStatus.ValidatingProxySettings, null, null, null);
            Assert.AreEqual(Visibility.Visible, result);

            result = (Visibility)converter.Convert(SettingDialogStatus.ProxyBusy, null, null, null);
            Assert.AreEqual(Visibility.Visible, result);

            result = (Visibility)converter.Convert(SettingDialogStatus.ProxyCannotConnect, null, null, null);
            Assert.AreEqual(Visibility.Visible, result);

            result = (Visibility)converter.Convert(SettingDialogStatus.ProxyUnvalid, null, null, null);
            Assert.AreEqual(Visibility.Visible, result);
        }
    }
}
