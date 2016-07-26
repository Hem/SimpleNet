using System;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using SimpleNet.Diagnostics;
using SimpleNet.DiagnosticService.Contracts;
using SimpleNet.DiagnosticService.Contracts.Adapters;
using SimpleNet.DiagnosticService.Contracts.Address;
using SimpleNet.DiagnosticService.Contracts.Callback;
using SimpleNet.Sample.Contracts;
using SimpleNet.ServiceHost.Contracts.Address;

namespace SimpleNet.Client.ConsoleApp
{
    class Program
    {
        private static readonly CompositionContainer Container;

        static Program()
        {
            var batch = MefRegistration.GetCompositionBatch();

            Container = MefRegistration.GetCompositionContainer(Assembly.GetExecutingAssembly(), batch);
        }

        static void Main(string[] args)
        {
            var appId = Guid.NewGuid();

            var callback = new DefaultDiagnosticsServicesCallback();
            callback.MessageReceived += delegate(LogMessageDto message)
            {
                Console.WriteLine("Received {0}: {2:d}: {1}", message.LogLevel, message.LogText, message.LogDateTime);
            };

            var serviceAddress =
                Container.GetExportedValue<IWcfServiceAddress>(DiagnosticsAddressProvider.DIAGNOSTICS_SERVICE_ADDRESS);

            var logger = Container.GetExportedValue<Logger>();
            var sampleService = Container.GetExportedValue<SampleServiceProxy>();

            var duplexCcasAdapter = new DuplexDiagnosticServiceAdapter(serviceAddress, callback);

            var inLoop = true;


            Console.Clear();

            Console.WriteLine(
                @"Please press
                    C: Connect to Duplex Messaging Service
                    R: Read data from Service
                    Q: Quit
                    ");


            while (inLoop)
            {
                Console.WriteLine("-------------------------------------------");

                var key = Console.ReadKey();
                
                switch (key.KeyChar.ToString().ToUpperInvariant())
                {
                    case "C":
                        try
                        {
                            Console.WriteLine("Establishing Duplex Connection");

                            duplexCcasAdapter.Connect(appId, "Debug", "secret");
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Failed! Please try again (takes 2 tries at times) ");
                        }
                        
                        break;
                    case "R":
                        Console.WriteLine(sampleService.GetServiceName());
                        break;
                    case "Q":
                        inLoop = false;
                        break;
                        
                }

                
            }

            
            Console.ReadLine();

        }

    }
}
