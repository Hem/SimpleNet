using System;
using System.Diagnostics;
using System.Reflection;
using System.ServiceProcess;
using Microsoft.Practices.ServiceLocation;
using SimpleNet.ServiceHost.Helpers;

namespace SimpleNet.Sample.ServiceHost
{
    static class Program
    {
        
        static Program()
        {
            // Yeah! in code!!
            Trace.Listeners.Add(new ConsoleTraceListener());
            

            var batch = MefRegistration.GetCompositionBatch();

            var locator = MefRegistration.GetServiceLocator(Assembly.GetExecutingAssembly(), batch);
            ServiceLocator.SetLocatorProvider(() => locator);
            
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {

            var servicesToRun = new ServiceBase[] 
            { 
                ServiceLocator.Current.GetInstance<ServiceContainer>()
            };

            // Run the application in memory or in interactive mode..
            if (!Environment.UserInteractive)
            {
                ServiceBase.Run(servicesToRun);
            }
            else
            {
                ServiceRunner.RunInteractive(servicesToRun);
            }
        }
    }
}
