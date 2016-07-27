using System;
using System.ServiceModel;

namespace SimpleNet.TraceBroadcastService.Contracts
{
    [ServiceContract(CallbackContract = typeof(IDiagnosticsServiceCallback))]
    public interface IDiagnosticsService
    {
        
        [OperationContract( IsOneWay = true )]
        void Connect( Guid appInstanceId, String minLogLevel, String sharedSecret );


        [OperationContract(IsOneWay = true)]
        void Disconnect(Guid appInstanceId);

    }
}
