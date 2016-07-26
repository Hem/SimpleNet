namespace SimpleNet.DiagnosticService.Contracts.Callback
{

    public delegate void DiagnosticMessage( LogMessageDto message );

    public class DefaultDiagnosticsServicesCallback : IDiagnosticsServiceCallback
    {
        public event DiagnosticMessage MessageReceived;

        protected virtual void OnMessageReceived( LogMessageDto message )
        {
            DiagnosticMessage handler = MessageReceived;
            if( handler != null )
            {
                handler( message );
            }
        }

        public void ReceiveLogMessage( LogMessageDto message )
        {
            OnMessageReceived( message );
        }
    }
}
