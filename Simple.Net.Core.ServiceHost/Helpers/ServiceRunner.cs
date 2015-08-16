using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;

namespace Simple.Net.Core.ServiceHost.Helpers
{
    public static class ServiceRunner
    {
        [ExcludeFromCodeCoverage]
        public static void RunInteractive(ServiceBase[] servicesToRun)
        {
            Console.WriteLine(@"Services running in interactive mode.");
            Console.WriteLine();

            MethodInfo onStartMethod = typeof(ServiceBase).GetMethod("OnStart",
                BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (ServiceBase service in servicesToRun)
            {
                try
                {
                    Trace.TraceInformation(@"Starting {0}...", service.ServiceName);
                    onStartMethod.Invoke(service, new object[] { new string[] { } });
                    Trace.TraceInformation(@"Starting {0}...... Started", service.ServiceName);
                }
                catch (Exception ex)
                {
                    Trace.TraceError(@"Starting {0}...... FAILED!!{1}{1}{2}{1}", service.ServiceName, Environment.NewLine, ex);
                }
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(@"Press any key to stop the services and end the process...");
            Console.Read();
            Console.WriteLine();

            MethodInfo onStopMethod = typeof(ServiceBase).GetMethod("OnStop", BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (ServiceBase service in servicesToRun)
            {
                Trace.Write( String.Format( @"Stopping {0}...", service.ServiceName ) );
                onStopMethod.Invoke(service, null);
                Trace.WriteLine(@"Stopped");
            }

            Console.WriteLine(@"All services stopped.");
            // Keep the console alive for a second to allow the user to see the message.
            Thread.Sleep(1000);
        }
    }
}
