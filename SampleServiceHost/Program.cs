using System;
using System.Reflection;
using System.ServiceProcess;
using Microsoft.Practices.ServiceLocation;
using Simple.Net.Core.ServiceHost.Helpers;

namespace SampleServiceHost
{
    static class Program
    {
        
        static Program()
        {
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
