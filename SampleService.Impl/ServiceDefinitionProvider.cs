using System.Collections.Generic;
using System.ComponentModel.Composition;
using SimpleNet.Sample.Contracts;
using SimpleNet.ServiceHost.Contracts;
using SimpleNet.ServiceHost.Contracts.Address;

namespace SimpleNet.Sample.Impl
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
