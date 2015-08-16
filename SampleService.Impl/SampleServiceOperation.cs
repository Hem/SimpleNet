using System.ComponentModel.Composition;
using System.ServiceModel;
using SampleService.Contracts;
using Simple.Net.Core.Diagnostics;

namespace SampleService.Impl
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
