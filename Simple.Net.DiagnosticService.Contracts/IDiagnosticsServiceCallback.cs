using System.ServiceModel;

namespace Simple.Net.DiagnosticService.Contracts
{
    public interface IDiagnosticsServiceCallback
    {
        // stupid thing do not forget this
        [OperationContract(IsOneWay = true)]
        void ReceiveLogMessage( LogMessageDto message );
    }
}