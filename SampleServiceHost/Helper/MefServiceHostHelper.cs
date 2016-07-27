using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using Microsoft.Practices.ServiceLocation;

using SimpleNet.ServiceHost.Helpers;

namespace SimpleNet.Sample.ServiceHost.Helper
{
    [Export]
    public class MefServiceHostHelper : AbstractServiceHostHelper
    {
        private const string NAME = "SAMPLE SERVICE HOST";


        [ImportingConstructor]
        public MefServiceHostHelper([Import] IServiceLocator serviceLocator) : base( NAME, serviceLocator)
        {
        }


        public override void LogInformation(string message)
        {
            Trace.TraceInformation(message);
        }

        public override void LogError(string message, Exception ex)
        {
            // Logger.Error(message, ex);
            Trace.TraceError(message);
            Trace.TraceError(ex.Message);
            Trace.TraceError(ex.Data.ToString());
        }
    }

}
