using Bugger.Applications.Models;
using Bugger.Presentation.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bugger.Presentation.Test.Conveters
{
    [TestClass]
    public class QueryStatusToIndeterminateConverterTest
    {
        [TestMethod]
        public void ConvertTest()
        {
            var converter = QueryStatusToIndeterminateConverter.Default;

            var result = (bool)converter.Convert(QueryStatus.Failed, null, null, null);
            Assert.IsFalse(result);

            result = (bool)converter.Convert(QueryStatus.FillingData, null, null, null);
            Assert.IsFalse(result);

            result = (bool)converter.Convert(QueryStatus.NotWorking, null, null, null);
            Assert.IsFalse(result);

            result = (bool)converter.Convert(QueryStatus.Qureying, null, null, null);
            Assert.IsTrue(result);

            result = (bool)converter.Convert(QueryStatus.QureyPause, null, null, null);
            Assert.IsFalse(result);

            result = (bool)converter.Convert(QueryStatus.Success, null, null, null);
            Assert.IsFalse(result);
        }
    }
}
