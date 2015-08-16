using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using System.Reflection;
using Simple.Net.Core.Diagnostics;
using Simple.Net.Core.Diagnostics.Loggers;
using Simple.Net.DiagnosticService.Contracts.Address;

namespace SampleServiceConsoleApp
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
                batch.AddExportedValue(DiagnosticsAddressProvider.DIAGNOSTICS_SERVICE_ADDRESS, diagnosticsAddress);
            batch.AddExportedValue("DIAGNOSTICS_SHARED_SECRET", "secret");

            return batch;
        }


        internal static CompositionContainer GetCompositionContainer(Assembly assembly, CompositionBatch batch)
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
                CompositionOptions.IsThreadSafe);


            
            batch.AddExportedValue(container);
            
            container.Compose(batch);

            return container;
        }
    }
}