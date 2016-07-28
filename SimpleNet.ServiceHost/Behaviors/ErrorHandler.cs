using System;
using System.Diagnostics;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace SimpleNet.ServiceHost.Behaviors
{
    public class ErrorHandler : IErrorHandler
    { 
        
        // Provide a fault. The Message fault parameter can be replaced, or set to
        // null to suppress reporting a fault.
        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            Trace.TraceError("Exception: {0} - {1}", error.GetType().Name, error.Message);
        }

        // HandleError. Log an error, then allow the error to be handled as usual.
        // Return true if the error is considered as already handled
        public bool HandleError(Exception error)
        {
            
            Trace.TraceError("Exception: {0} - {1}", error.GetType().Name, error.Message);
            foreach (var key in error.Data.Keys)
            {
                Trace.TraceError("Exception: {0} - {1}", key, error.Data[key]);
            }

            if (error.InnerException != null)
                HandleError(error.InnerException);

            return false;
        }
    }
}
