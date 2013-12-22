using Bugger.Applications.Models;
using Bugger.Presentation.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;

namespace Bugger.Presentation.Test.Conveters
{
    [TestClass]
    public class QueryStatusToVisibilityConverterTest
    {
        [TestMethod]
        public void ConvertTest()
        {
            var converter = QueryStatusToVisibilityConverter.Default;

            var result = converter.Convert(QueryStatus.Failed, null, null, null);
            Assert.AreEqual(Visibility.Visible, result);

            result = converter.Convert(QueryStatus.FillData, null, null, null);
            Assert.AreEqual(Visibility.Visible, result);

            result = converter.Convert(QueryStatus.NotWorking, null, null, null);
            Assert.AreEqual(Visibility.Collapsed, result);

            result = converter.Convert(QueryStatus.Qureying, null, null, null);
            Assert.AreEqual(Visibility.Visible, result);

            result = converter.Convert(QueryStatus.Success, null, null, null);
            Assert.AreEqual(Visibility.Visible, result);

            result = converter.Convert(QueryStatus.QureyPause, null, null, null);
            Assert.AreEqual(Visibility.Visible, result);
        }
    }
}
