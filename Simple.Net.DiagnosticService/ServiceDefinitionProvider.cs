using System.Collections.Generic;
using System.ComponentModel.Composition;
using Simple.Net.Core.Diagnostics;
using Simple.Net.Core.ServiceHost.Contracts;
using Simple.Net.Core.ServiceHost.Contracts.Address;
using Simple.Net.DiagnosticService.Contracts;
using Simple.Net.DiagnosticService.Contracts.Address;

namespace Simple.Net.DiagnosticService
{

    [Export(typeof(IServiceDefinitionProvider))]
    public class ServiceDefinitionProvider : IServiceDefinitionProvider
    {
        [Import(DiagnosticsAddressProvider.DIAGNOSTICS_SERVICE_ADDRESS, AllowDefault = true)]
        public IWcfServiceAddress ServiceAddress { get; set; }

        
        [Import]
        public Logger Logger { get; set; }


        public IEnumerable<ServiceDefinitionBase> ListServiceDefinitions(string hostName)
        {
            if( ServiceAddress == null )
                Logger.Warning( "IWcfServiceAddress is not provided for DIAGNOSTICS_SERVICE_ADDRESS" );

            if (ServiceAddress != null)
                yield return new WcfServiceDefinition
                {
                    ServiceType = ServiceTypeDefinition.WcfTcpServiceHost,
                    EnableDebugBehavior = true,
                    EnableWsdl = false,
                    ServiceDescription = "Log broadcasting service",
                    WcfServiceInterfaceType = typeof(IDiagnosticsService),
                    WcfServiceOperationType = typeof(DiagnosticsService),
                    ServiceAddress = ServiceAddress,
                    
                    // TODO: Maybe it would be a good idea to enable Security! Just saying.
                    // Change the Security on the binding too!
                    // EnableSecurity = false,                  // To enable windows authentication set this to true
                    // CustomUserNamePasswordValidator = null   // To enable custom user name and password provide this value!
                };
        }
    }
}
