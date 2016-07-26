using System;
using System.ComponentModel.Composition;
using Microsoft.Practices.ServiceLocation;
using SimpleNet.Diagnostics;
using SimpleNet.ServiceHost.Helpers;

namespace SimpleNet.Sample.ServiceHost.Helper
{
    [Export]
    public class MefServiceHostHelper : AbstractServiceHostHelper
    {
        private const string NAME = "SAMPLE SERVICE HOST";

        [Import]
        public Logger Logger { get; set; }


        [ImportingConstructor]
        public MefServiceHostHelper([Import] IServiceLocator serviceLocator) : base( NAME, serviceLocator)
        {
        }


        public override void LogInformation(string message)
        {
            Logger.Info(message);
        }

        public override void LogError(string message, Exception ex)
        {
            Logger.Error(message, ex);
        }
    }

}
