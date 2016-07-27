namespace SimpleNet.DiagnosticService.Contracts.Callback
{

    public delegate void DiagnosticMessage( LogMessageDto message );

    public class DefaultDiagnosticsServicesCallback : IDiagnosticsServiceCallback
    {
        public event DiagnosticMessage MessageReceived;

        protected virtual void OnMessageReceived( LogMessageDto message )
        {
            var handler = MessageReceived;
            handler?.Invoke( message );
        }

        public void ReceiveLogMessage( LogMessageDto message )
        {
            OnMessageReceived( message );
        }
    }
}
