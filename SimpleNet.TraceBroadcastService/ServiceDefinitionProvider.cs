using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using SimpleNet.ServiceHost.Contracts;
using SimpleNet.ServiceHost.Contracts.Address;
using SimpleNet.TraceBroadcastService.Contracts;
using SimpleNet.TraceBroadcastService.Contracts.Address;
using SimpleNet.TraceBroadcastService.TraceListeners;

namespace SimpleNet.TraceBroadcastService
{
    [Export(typeof(IServiceDefinitionProvider))]
    public class ServiceDefinitionProvider : IServiceDefinitionProvider
    {
        [Import(DiagnosticsAddressProvider.DIAGNOSTICS_SERVICE_ADDRESS, AllowDefault = true)]
        public IWcfServiceAddress ServiceAddress { get; set; }


        [ImportingConstructor]
        public ServiceDefinitionProvider(BroadcastTraceListener traceListener)
        {
            Trace.Listeners.Add(traceListener);
        }




        public IEnumerable<ServiceDefinitionBase> ListServiceDefinitions(string hostName)
        {
            if (ServiceAddress == null)
                Trace.TraceWarning("IWcfServiceAddress is not provided for DIAGNOSTICS_SERVICE_ADDRESS");

            if (ServiceAddress != null)
                yield return new WcfServiceDefinition
                {
                    ServiceType = ServiceTypeDefinition.WcfTcpServiceHost,
                    EnableDebugBehavior = true,
                    EnableWsdl = false,
                    ServiceDescription = "Log broadcasting service",
                    WcfServiceInterfaceType = typeof(IDiagnosticsService),
                    WcfServiceOperationType = typeof(TraceBroadcastService),
                    ServiceAddress = ServiceAddress,

                    // TODO: Maybe it would be a good idea to enable Security! Just saying.
                    // Change the Security on the binding too!
                    // EnableSecurity = false,                  // To enable windows authentication set this to true
                    // CustomUserNamePasswordValidator = null   // To enable custom user name and password provide this value!
                };
        }
    }
}
