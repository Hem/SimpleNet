using System.ComponentModel.Composition;
using Simple.Net.Core.ServiceHost.Contracts.Address;
using Simple.Net.Core.ServiceHost.Contracts.Bindings;

namespace SampleService.Contracts
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
