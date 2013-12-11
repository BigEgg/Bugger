using Bugger.Proxy.TFS.Models;
using Bugger.Proxy.TFS.Presentation.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bugger.Proxy.TFS.Presentation.Test.Converters
{
    [TestClass]
    public class ProgressTypesToEnableConverterTest
    {
        [TestMethod]
        public void ConvertTest()
        {
            var converter = ProgressTypesToEnableConverter.Default;

            var result = (bool)converter.Convert(ProgressTypes.FailedOnConnect, null, null, null);
            Assert.IsFalse(result);

            result = (bool)converter.Convert(ProgressTypes.FailedOnGetFileds, null, null, null);
            Assert.IsFalse(result);

            result = (bool)converter.Convert(ProgressTypes.NotWorking, null, null, null);
            Assert.IsFalse(result);

            result = (bool)converter.Convert(ProgressTypes.OnAutoFillMapSettings, null, null, null);
            Assert.IsFalse(result);

            result = (bool)converter.Convert(ProgressTypes.OnConnectProgress, null, null, null);
            Assert.IsFalse(result);

            result = (bool)converter.Convert(ProgressTypes.OnGetFiledsProgress, null, null, null);
            Assert.IsFalse(result);

            result = (bool)converter.Convert(ProgressTypes.Success, null, null, null);
            Assert.IsTrue(result);

            result = (bool)converter.Convert(ProgressTypes.SuccessWithError, null, null, null);
            Assert.IsTrue(result);
        }
    }
}
