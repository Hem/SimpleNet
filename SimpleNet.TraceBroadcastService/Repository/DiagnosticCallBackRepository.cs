using System.ComponentModel.Composition;
using SimpleNet.TraceBroadcastService.Contracts;

namespace SimpleNet.TraceBroadcastService.Repository
{
    [Export]
    public class DiagnosticCallBackRepository : DuplexServiceClientRepository<IDiagnosticsServiceCallback>
    {
        
    }
}