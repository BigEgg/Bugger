﻿using BigEgg.Framework.Applications.Extensions.Applications.Services.Messages;
using BigEgg.Framework.Applications.Extensions.Presentation.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Windows;

namespace BigEgg.Framework.Applications.Extensions.Test.Presentation.Services
{
    [TestClass]
    public class MessageServiceTest
    {
        [TestMethod]
        public void InjectionTest()
        {
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new TypeCatalog(
                typeof(MessageService)
            ));

            var container = new CompositionContainer(catalog);
            CompositionBatch batch = new CompositionBatch();
            batch.AddExportedValue(container);
            container.Compose(batch);

            var service = container.GetExportedValue<IMessageService>();
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void MessageBoxResultTest()
        {
            MessageService messageService = new MessageService();
            PropertyInfo messageBoxResultInfo = typeof(MessageService).GetProperty("MessageBoxResult",
                BindingFlags.Static | BindingFlags.NonPublic);

            Assert.AreEqual(MessageBoxResult.None, (MessageBoxResult)messageBoxResultInfo.GetValue(null, null));
        }

        [TestMethod]
        public void MessageBoxOptionsTest()
        {
            MessageService messageService = new MessageService();
            PropertyInfo messageBoxOptionsInfo = typeof(MessageService).GetProperty("MessageBoxOptions",
                BindingFlags.Static | BindingFlags.NonPublic);

            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
            Assert.AreEqual(MessageBoxOptions.None, (MessageBoxOptions)messageBoxOptionsInfo.GetValue(null, null));

            Thread.CurrentThread.CurrentUICulture = new CultureInfo("ar-SA");
            Assert.AreEqual(MessageBoxOptions.RtlReading, (MessageBoxOptions)messageBoxOptionsInfo.GetValue(null, null));
        }

        [TestMethod]
        public void MessageBoxOptionsTest_RightToLeft()
        {
            MessageService messageService = new MessageService();
            PropertyInfo messageBoxOptionsInfo = typeof(MessageService).GetProperty("MessageBoxOptions",
                BindingFlags.Static | BindingFlags.NonPublic);

            Thread.CurrentThread.CurrentUICulture = new CultureInfo("ar-SA");
            Assert.AreEqual(MessageBoxOptions.RtlReading, (MessageBoxOptions)messageBoxOptionsInfo.GetValue(null, null));
        }
    }
}
