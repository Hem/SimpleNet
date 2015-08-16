using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using System.Reflection;
using Microsoft.Mef.CommonServiceLocator;
using Microsoft.Practices.ServiceLocation;
using Simple.Net.Core.Diagnostics;
using Simple.Net.Core.Diagnostics.Loggers;
using Simple.Net.DiagnosticService.Contracts.Address;


namespace SampleServiceHost
{
    internal static class MefRegistration
    {
        internal static CompositionBatch GetCompositionBatch()
        {
            const string diagnosticsAddress = @"net.tcp://localhost:6000/DiagnosticService.svc";

            var batch = new CompositionBatch();
            var logger = new Logger();

            
            batch.AddExportedValue(logger);
            batch.AddExportedValue(new ConsoleLogWritter(logger));

            if (!String.IsNullOrEmpty(diagnosticsAddress))
            {
                batch.AddExportedValue(DiagnosticsAddressProvider.DIAGNOSTICS_SERVICE_ADDRESS, diagnosticsAddress);
                batch.AddExportedValue("DIAGNOSTICS_SHARED_SECRET", "secret");

            }
                

            return batch;
        }


        internal static IServiceLocator GetServiceLocator(Assembly assembly, CompositionBatch batch)
        {
            
            var assemblyLocation = assembly.Location;
            var file = new FileInfo(assemblyLocation);

            var catalogs = new List<ComposablePartCatalog>
            {
                new AssemblyCatalog(assembly),
                new DirectoryCatalog(file.DirectoryName ?? ".")
            };


            var catalog = new AggregateCatalog(catalogs);

            
            var container = new CompositionContainer(catalog,
                                                        CompositionOptions.DisableSilentRejection |
                                                        CompositionOptions.IsThreadSafe );


            var serviceLocator = new MefServiceLocator(container);

            batch.AddExportedValue(container);
            batch.AddExportedValue<IServiceLocator>(serviceLocator);

            container.Compose(batch);

            return serviceLocator;
        }
    }
}
