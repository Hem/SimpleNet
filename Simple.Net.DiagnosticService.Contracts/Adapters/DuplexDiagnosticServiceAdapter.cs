using System;
using SimpleNet.ServiceHost.Contracts.Address;
using SimpleNet.ServiceHost.Contracts.Proxy;

namespace SimpleNet.TraceBroadcastService.Contracts.Adapters
{
    public interface IDuplexDiagnosticServiceAdapter : IDiagnosticsService, IWcfProxyChannel
    {
        
    }


    public class DuplexDiagnosticServiceAdapter : 
            DuplexWcfProxy<IDiagnosticsService, IDiagnosticsServiceCallback>, 
            IDuplexDiagnosticServiceAdapter
    {

        public DuplexDiagnosticServiceAdapter( IWcfServiceAddress address,
                                               IDiagnosticsServiceCallback callback ) : base( address, callback )
        {
            
        }

        public void Connect( Guid appInstanceId, string minLogLevel, string sharedSecret )
        {
            Process( x => x.Connect( appInstanceId, minLogLevel, sharedSecret ) );
        }
        public void Disconnect( Guid appInstanceId )
        {
            Process( x => x.Disconnect( appInstanceId ) );
        }

    }

    
}
