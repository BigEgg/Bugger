using BigEgg.Framework.Applications.Applications;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BigEgg.Framework.Applications.Test.Applications
{
    [TestClass]
    public class ApplicationInfoTest
    {
        [TestMethod]
        public void GeneralTest()
        {
            //  Visual Studio Unit Testing Framework doesn't have an entry assembly

            var productName = ApplicationInfo.ProductName;
            var version = ApplicationInfo.Version;
            var company = ApplicationInfo.Company;
            var copyright = ApplicationInfo.Copyright;
            var applicationInfo = ApplicationInfo.ApplicationPath;

            //  Cached values
            productName = ApplicationInfo.ProductName;
            version = ApplicationInfo.Version;
            company = ApplicationInfo.Company;
            copyright = ApplicationInfo.Copyright;
            applicationInfo = ApplicationInfo.ApplicationPath;
        }
    }
}
