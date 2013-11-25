using Bugger.Proxy.TFS.Models;
using Bugger.Proxy.TFS.Presentation.Converters;
using Bugger.Proxy.TFS.Presentation.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bugger.Proxy.TFS.Presentation.Test.Converters
{
    [TestClass]
    public class ProgressTypesToMessageConverterTest
    {
        [TestMethod]
        public void ConvertTest()
        {
            var converter = ProgressTypesToMessageConverter.Default;

            var result = converter.Convert(ProgressTypes.FailedOnConnect, null, null, null) as string;
            Assert.AreEqual(Resources.FailedOnConnect, result);

            result = converter.Convert(ProgressTypes.FailedOnGetFileds, null, null, null) as string;
            Assert.AreEqual(Resources.FailedOnGetFileds, result);

            result = converter.Convert(ProgressTypes.NotWorking, null, null, null) as string;
            Assert.AreEqual(string.Empty, result);

            result = converter.Convert(ProgressTypes.OnAutoFillMapSettings, null, null, null) as string;
            Assert.AreEqual(Resources.OnAutoFillMapSettings, result);

            result = converter.Convert(ProgressTypes.OnConnectProgress, null, null, null) as string;
            Assert.AreEqual(Resources.OnConnectProgress, result);

            result = converter.Convert(ProgressTypes.OnGetFiledsProgress, null, null, null) as string;
            Assert.AreEqual(Resources.OnGetFiledsProgress, result);

            result = converter.Convert(ProgressTypes.Success, null, null, null) as string;
            Assert.AreEqual(Resources.Success, result);

            result = converter.Convert(ProgressTypes.SuccessWithError, null, null, null) as string;
            Assert.AreEqual(Resources.SuccessWithError, result);
        }
    }
}
