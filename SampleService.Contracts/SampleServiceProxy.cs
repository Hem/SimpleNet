using System.ComponentModel.Composition;
using Simple.Net.Core.ServiceHost.Contracts.Address;
using Simple.Net.Core.ServiceHost.Contracts.Proxy;

namespace SampleService.Contracts
{
    [Export]
    public class SampleServiceProxy : WcfProxy<ISampleService>, ISampleService
    {
        
        
        [ImportingConstructor]
        public SampleServiceProxy([Import("SAMPLE_SERVICE")] IWcfServiceAddress address)
                :base(address.Binding, address.WcfEndpointUrl)
        {
            
        }


        public string GetServiceName()
        {
            return Process(x => x.GetServiceName());
        }
    }
}
