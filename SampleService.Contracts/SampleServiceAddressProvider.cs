using System.ComponentModel.Composition;
using SimpleNet.ServiceHost.Contracts.Address;
using SimpleNet.ServiceHost.Contracts.Bindings;

namespace SimpleNet.Sample.Contracts
{
    public class SampleServiceAddressProvider
    {
        [Export("SAMPLE_SERVICE")]
        public IWcfServiceAddress Address {
            get
            {
                return new WcfServiceAddress
                {
                    Binding = StandardBindings.GetBasicHttpBinding(),
                    UseSsl = false,
                    WcfEndpointUrl = @"http://localhost/SampleService.svc",
                    WsdlUrl = @"http://localhost/SampleService.svc"
                };
            } 
        }
    }
}
