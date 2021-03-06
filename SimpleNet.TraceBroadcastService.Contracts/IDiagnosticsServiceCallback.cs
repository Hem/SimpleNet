﻿using System.ServiceModel;

namespace SimpleNet.TraceBroadcastService.Contracts
{
    [ServiceContract]
    public interface IDiagnosticsServiceCallback
    {
        // stupid thing do not forget this
        [OperationContract(IsOneWay = true)]
        void ReceiveLogMessage( LogMessageDto message );
    }
}