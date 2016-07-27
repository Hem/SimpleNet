using System;
using System.ComponentModel.Composition;
using System.ServiceModel;
using SimpleNet.DiagnosticService.Contracts;
using SimpleNet.TraceBroadcastService.Repository;

namespace SimpleNet.TraceBroadcastService
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple,
                          InstanceContextMode = InstanceContextMode.PerCall,
                                    AddressFilterMode = AddressFilterMode.Any)]
    public class TraceBroadcastService : IDiagnosticsService
    {

        [Import]
        public DiagnosticCallBackRepository DiagnosticCallBackRepository { get; set; }


        [Import("TRACE_BROADCAST_SHARED_SECRET", AllowDefault = true)]
        public string SharedSecret { get; set; }



        public void Connect(Guid appInstanceId, string minLogLevel, string sharedSecret)
        {
            if (sharedSecret.Equals(SharedSecret))
            {   
                DiagnosticCallBackRepository.Add(appInstanceId, Callback);
            }
        }

        public void Disconnect(Guid appInstanceId)
        {
            DiagnosticCallBackRepository.Delete(appInstanceId);
        }


        IDiagnosticsServiceCallback Callback => OperationContext.Current.GetCallbackChannel<IDiagnosticsServiceCallback>();
    }
}
