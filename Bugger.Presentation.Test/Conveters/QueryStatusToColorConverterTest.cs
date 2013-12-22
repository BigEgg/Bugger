using Bugger.Applications.Models;
using Bugger.Presentation.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Media;

namespace Bugger.Presentation.Test.Conveters
{
    [TestClass]
    public class QueryStatusToColorConverterTest
    {
        [TestMethod]
        public void ConvertTest()
        {
            var converter = QueryStatusToColorConverter.Default;

            var result = converter.Convert(QueryStatus.Failed, null, null, null);
            Assert.AreEqual(Brushes.Red, result);

            result = converter.Convert(QueryStatus.FillData, null, null, null);
            Assert.AreEqual(Brushes.Green, result);

            result = converter.Convert(QueryStatus.NotWorking, null, null, null);
            Assert.AreEqual(Brushes.Green, result);

            result = converter.Convert(QueryStatus.Qureying, null, null, null);
            Assert.AreEqual(Brushes.Green, result);

            result = converter.Convert(QueryStatus.Success, null, null, null);
            Assert.AreEqual(Brushes.Green, result);

            result = converter.Convert(QueryStatus.QureyPause, null, null, null);
            Assert.AreEqual(Brushes.Orange, result);
        }
    }
}
