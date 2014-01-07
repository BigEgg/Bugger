using Bugger.Applications.Models;
using Bugger.Presentation.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Media;

namespace Bugger.Presentation.Test.Conveters
{
    [TestClass]
    public class SettingDialogStatusToColorConverterTest
    {
        [TestMethod]
        public void ConvertTest()
        {
            var converter = SettingDialogStatusToColorConverter.Default;

            var result = converter.Convert(SettingDialogStatus.InitiatingProxyFailed, null, null, null);
            Assert.AreEqual(Brushes.Red, result);

            result = converter.Convert(SettingDialogStatus.InitiatingProxy, null, null, null);
            Assert.AreEqual(Brushes.Green, result);

            result = converter.Convert(SettingDialogStatus.NotWorking, null, null, null);
            Assert.AreEqual(Brushes.Green, result);

            result = converter.Convert(SettingDialogStatus.ProxyValid, null, null, null);
            Assert.AreEqual(Brushes.Green, result);

            result = converter.Convert(SettingDialogStatus.ValidatingProxySettings, null, null, null);
            Assert.AreEqual(Brushes.Green, result);

            result = converter.Convert(SettingDialogStatus.ProxyBusy, null, null, null);
            Assert.AreEqual(Brushes.Orange, result);

            result = converter.Convert(SettingDialogStatus.ProxyCannotConnect, null, null, null);
            Assert.AreEqual(Brushes.Orange, result);

            result = converter.Convert(SettingDialogStatus.ProxyUnvalid, null, null, null);
            Assert.AreEqual(Brushes.Orange, result);
        }
    }
}
