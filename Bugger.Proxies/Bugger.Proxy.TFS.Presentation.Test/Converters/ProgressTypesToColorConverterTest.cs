using System.Windows.Media;
using Bugger.Proxy.TFS.Models;
using Bugger.Proxy.TFS.Presentation.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bugger.Proxy.TFS.Presentation.Test.Converters
{
    [TestClass]
    public class ProgressTypesToColorConverterTest
    {
        [TestMethod]
        public void ConvertTest()
        {
            var converter = ProgressTypesToColorConverter.Default;

            var result = converter.Convert(ProgressTypes.FailedOnConnect, null, null, null);
            Assert.AreEqual(Brushes.Red, result);

            result = converter.Convert(ProgressTypes.FailedOnGetFileds, null, null, null);
            Assert.AreEqual(Brushes.Red, result);

            result = converter.Convert(ProgressTypes.NotWorking, null, null, null);
            Assert.AreEqual(Brushes.Green, result);

            result = converter.Convert(ProgressTypes.OnAutoFillMapSettings, null, null, null);
            Assert.AreEqual(Brushes.Green, result);

            result = converter.Convert(ProgressTypes.OnConnectProgress, null, null, null);
            Assert.AreEqual(Brushes.Green, result);

            result = converter.Convert(ProgressTypes.OnGetFiledsProgress, null, null, null);
            Assert.AreEqual(Brushes.Green, result);

            result = converter.Convert(ProgressTypes.Success, null, null, null);
            Assert.AreEqual(Brushes.Green, result);

            result = converter.Convert(ProgressTypes.SuccessWithError, null, null, null);
            Assert.AreEqual(Brushes.Orange, result);
        }
    }
}
