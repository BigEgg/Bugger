using System.Windows;
using Bugger.Proxy.TFS.Models;
using Bugger.Proxy.TFS.Presentation.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bugger.Proxy.TFS.Presentation.Test.Converters
{
    [TestClass]
    public class ProgressTypesToVisibilityConverterTest
    {
        [TestMethod]
        public void ConvertTest()
        {
            var converter = ProgressTypesToVisibilityConverter.Default;

            var result = (Visibility)converter.Convert(ProgressTypes.FailedOnConnect, null, null, null);
            Assert.AreEqual(Visibility.Visible, result);

            result = (Visibility)converter.Convert(ProgressTypes.FailedOnGetFileds, null, null, null);
            Assert.AreEqual(Visibility.Visible, result);

            result = (Visibility)converter.Convert(ProgressTypes.NotWorking, null, null, null);
            Assert.AreEqual(Visibility.Collapsed, result);

            result = (Visibility)converter.Convert(ProgressTypes.OnAutoFillMapSettings, null, null, null);
            Assert.AreEqual(Visibility.Visible, result);

            result = (Visibility)converter.Convert(ProgressTypes.OnConnectProgress, null, null, null);
            Assert.AreEqual(Visibility.Visible, result);

            result = (Visibility)converter.Convert(ProgressTypes.OnGetFiledsProgress, null, null, null);
            Assert.AreEqual(Visibility.Visible, result);

            result = (Visibility)converter.Convert(ProgressTypes.Success, null, null, null);
            Assert.AreEqual(Visibility.Visible, result);

            result = (Visibility)converter.Convert(ProgressTypes.SuccessWithError, null, null, null);
            Assert.AreEqual(Visibility.Visible, result);
        }
    }
}
