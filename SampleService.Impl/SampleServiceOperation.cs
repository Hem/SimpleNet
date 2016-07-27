using System.ComponentModel.Composition;
using System.Diagnostics;
using System.ServiceModel;
using SimpleNet.Sample.Contracts;

namespace SimpleNet.Sample.Impl
{


    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, 
                    InstanceContextMode = InstanceContextMode.PerCall)]
    
    public class SampleServiceOperation : ISampleService
    {
        
        public string GetServiceName()
        {
            Trace.TraceInformation("Loggin info from Sample Service => GetServiceName");

            return "Hello World - Response from GetServiceName";
        }
    }
}
