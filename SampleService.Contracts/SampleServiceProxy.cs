using System.ComponentModel.Composition;
using SimpleNet.ServiceHost.Contracts.Address;
using SimpleNet.ServiceHost.Contracts.Proxy;

namespace SimpleNet.Sample.Contracts
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
