using Bugger.Proxy.TFS.Presentation.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;

namespace Bugger.Proxy.TFS.Presentation.Test
{
    [TestClass]
    public class ItemsToStringConverterTest
    {
        [TestMethod]
        public void ConvertTest()
        {
            ItemsToStringConverter converter = ItemsToStringConverter.Default;

            ObservableCollection<string> list = new ObservableCollection<string>()
            {
                "Work Item",
                "Bug"
            };
            string result = converter.Convert(list, null, null, null) as string;
            Assert.AreEqual("Work Item; Bug", result);

            list.Add("Test Case");
            result = converter.Convert(list, null, null, null) as string;
            Assert.AreEqual("Work Item; Bug; T...", result);
        }
    }
}
