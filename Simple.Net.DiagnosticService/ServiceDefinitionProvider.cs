using System.Collections.Generic;
using System.ComponentModel.Composition;
using SimpleNet.Diagnostics;
using SimpleNet.DiagnosticService.Contracts;
using SimpleNet.DiagnosticService.Contracts.Address;
using SimpleNet.ServiceHost.Contracts;
using SimpleNet.ServiceHost.Contracts.Address;

namespace SimpleNet.DiagnosticService
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
