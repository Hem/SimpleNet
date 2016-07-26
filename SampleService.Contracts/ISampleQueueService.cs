using System;
using System.ServiceModel;

namespace SimpleNet.Sample.Contracts
{
    [ServiceContract]
    public interface ISampleQueueService
    {
        [OperationContract(IsOneWay = true)]
        void Ping(String message);
    }
}