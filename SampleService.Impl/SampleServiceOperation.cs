using System.ComponentModel.Composition;
using System.ServiceModel;
using SimpleNet.Diagnostics;
using SimpleNet.Sample.Contracts;

namespace SimpleNet.Sample.Impl
{


    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, 
                    InstanceContextMode = InstanceContextMode.PerCall)]
    
    public class SampleServiceOperation : ISampleService
    {
        [Import]
        public Logger Logger { get; set; }

        public string GetServiceName()
        {
            Logger.Info("Loggin info from Sample Service => GetServiceName");

            return "Hello World - Response from GetServiceName";
        }
    }
}
