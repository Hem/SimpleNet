using System.Collections.Generic;
using System.ComponentModel.Composition;
using SampleService.Contracts;
using Simple.Net.Core.ServiceHost.Contracts;
using Simple.Net.Core.ServiceHost.Contracts.Address;

namespace SampleService.Impl
{
    [Export(typeof(IServiceDefinitionProvider))]
    public class ServiceDefinitionProvider : IServiceDefinitionProvider
    {
        [Import("SAMPLE_SERVICE")]
        public IWcfServiceAddress ServiceAddress { get; set; }



        public IEnumerable<ServiceDefinitionBase> ListServiceDefinitions(string hostName)
        {
            yield return new WcfServiceDefinition
            {
                ServiceType = ServiceTypeDefinition.WcfHttpServiceHost,
                WcfServiceInterfaceType = typeof(ISampleService),
                WcfServiceOperationType = typeof(SampleServiceOperation),
                ServiceDescription = "Demo HTTP Service",
                EnableDebugBehavior = true,
                EnableWsdl = true,
                EnableSecurity = false,
                ServiceAddress = ServiceAddress
            };
        }
    }
}
