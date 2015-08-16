using System;
using System.ServiceModel;

namespace SampleService.Contracts
{
    [ServiceContract]
    public interface ISampleQueueService
    {
        [OperationContract(IsOneWay = true)]
        void Ping(String message);
    }
}