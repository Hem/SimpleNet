using System;
using System.ServiceModel;

namespace SampleService.Contracts
{

    [ServiceContract]
    public interface ISampleService
    {
        [OperationContract]
        String GetServiceName();
    }
}
