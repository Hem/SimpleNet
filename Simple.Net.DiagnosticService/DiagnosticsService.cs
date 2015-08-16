using System;
using System.ComponentModel.Composition;
using System.ServiceModel;
using Simple.Net.Core.Diagnostics.Contracts;
using Simple.Net.DiagnosticService.Business;
using Simple.Net.DiagnosticService.Contracts;

namespace Simple.Net.DiagnosticService
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple,
                          InstanceContextMode = InstanceContextMode.PerCall,
                                    AddressFilterMode = AddressFilterMode.Any)]
    public class DiagnosticsService : IDiagnosticsService
    {
        [Import]
        public ConnectedDiagnosticClientCallbacks CallbackManager { get; set; }

        [Import("DIAGNOSTICS_SHARED_SECRET", AllowDefault = true)]
        public String SharedSecret { get; set; }

        public void Connect( Guid appInstanceId, string minLogLevel, string sharedSecret )
        {
            // TODO: Move to MEF + Application Settings
            if (sharedSecret.Equals(SharedSecret))
            {
                LoggerLogLevel level;

                Enum.TryParse( minLogLevel, out level );

                CallbackManager.AddCallback( appInstanceId, level , Callback );
                
            }

        }

        public void Disconnect( Guid appInstanceId )
        {
            CallbackManager.DeleteKey( appInstanceId );
        }


        IDiagnosticsServiceCallback Callback
        {
            get
            {
                return OperationContext.Current.GetCallbackChannel<IDiagnosticsServiceCallback>();
            }
        }
    }
}
