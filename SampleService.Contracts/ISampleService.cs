using System;
using System.ServiceModel;

namespace SimpleNet.Sample.Contracts
{

    [ServiceContract]
    public interface ISampleService
    {
        [OperationContract]
        String GetServiceName();
    }
}
