using Bugger.Applications.Models;
using Bugger.Presentation.Converters;
using Bugger.Presentation.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bugger.Presentation.Test.Conveters
{
    [TestClass]
    public class SettingDialogStatusToMessageConverterTest
    {
        [TestMethod]
        public void ConvertTest()
        {
            var converter = SettingDialogStatusToMessageConverter.Default;

            var result = converter.Convert(SettingDialogStatus.InitiatingProxyFailed, null, null, null) as string;
            Assert.AreEqual(Resources.SettingStatusInitiatingProxyFailed, result);

            result = converter.Convert(SettingDialogStatus.InitiatingProxy, null, null, null) as string;
            Assert.AreEqual(Resources.SettingStatusInitiatingProxy, result);

            result = converter.Convert(SettingDialogStatus.NotWorking, null, null, null) as string;
            Assert.AreEqual(string.Empty, result);

            result = converter.Convert(SettingDialogStatus.ProxyValid, null, null, null) as string;
            Assert.AreEqual(string.Empty, result);

            result = converter.Convert(SettingDialogStatus.ValidatingProxySettings, null, null, null) as string;
            Assert.AreEqual(Resources.SettingStatusValidatingProxySettings, result);

            result = converter.Convert(SettingDialogStatus.ProxyBusy, null, null, null) as string;
            Assert.AreEqual(Resources.SettingStatusProxyBusy, result);

            result = converter.Convert(SettingDialogStatus.ProxyCannotConnect, null, null, null) as string;
            Assert.AreEqual(Resources.SettingStatusProxyCannotConnect, result);

            result = converter.Convert(SettingDialogStatus.ProxyUnvalid, null, null, null) as string;
            Assert.AreEqual(Resources.SettingStatusProxyUnvalid, result);
        }
    }
}
