using BigEgg.Framework;
using BigEgg.Framework.Applications;
using BigEgg.Framework.Applications.ViewModels;
using Bugger.Applications.Controllers;
using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

namespace Bugger.Presentation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private AggregateCatalog catalog;
        private CompositionContainer container;
        private IApplicationController controller;


        public App()
        {
#if (DEBUG)
            BEConfiguration.Debug = true;
#endif
        }


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

#if (DEBUG != true)
            // Don't handle the exceptions in Debug mode because otherwise the Debugger wouldn't
            // jump into the code when an exception occurs.
            DispatcherUnhandledException += AppDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += AppDomainUnhandledException;
#endif

            catalog = new AggregateCatalog();
            // Add the Framework assembly to the catalog
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(ViewModel).Assembly));
            // Add the Bugger.Presentation assembly to the catalog
            catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
            // Add the Bugger.Applications assembly to the catalog
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(IApplicationController).Assembly));

            // Add the Bugger.Proxy assemblies to the catalog
            string proxyAsseblyPath = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "Proxies");
            foreach (var file in new System.IO.DirectoryInfo(proxyAsseblyPath).GetFiles())
            {
                if (file.Extension.ToLower() == ".dll" && file.Name.StartsWith("Bugger.Proxy."))
                {
                    catalog.Catalogs.Add(new AssemblyCatalog(file.FullName));
                }
            }

            container = new CompositionContainer(catalog);
            CompositionBatch batch = new CompositionBatch();
            batch.AddExportedValue(container);
            container.Compose(batch);

            controller = container.GetExportedValue<IApplicationController>();
            controller.Initialize();
            controller.Run();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            controller.ShutDown();
            container.Dispose();
            catalog.Dispose();

            base.OnExit(e);
        }

        private void AppDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            HandleException(e.Exception, false);
            e.Handled = true;
        }

        private static void AppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleException(e.ExceptionObject as Exception, e.IsTerminating);
        }

        private static void HandleException(Exception e, bool isTerminating)
        {
            if (e == null) { return; }

            Trace.TraceError(e.ToString());

            if (!isTerminating)
            {
                MessageBox.Show(
                    string.Format(
                        CultureInfo.CurrentCulture, 
                        Bugger.Presentation.Properties.Resources.UnknownError, 
                        e.ToString()),
                    ApplicationInfo.ProductName, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
