using System.ComponentModel.Composition;
using SimpleNet.DiagnosticService.Contracts;

namespace SimpleNet.TraceBroadcastService.Repository
{
    [Export]
    public class DiagnosticCallBackRepository : DuplexServiceClientRepository<IDiagnosticsServiceCallback>
    {
        
    }
}