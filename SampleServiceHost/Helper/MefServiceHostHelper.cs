using System.ComponentModel.Composition;
using Microsoft.Practices.ServiceLocation;
using SimpleNet.Diagnostics;
using SimpleNet.ServiceHost.Helpers;

namespace SimpleNet.Sample.ServiceHost.Helper
{
    [Export]
    public class MefServiceHostHelper : AbstractServiceHostHelper
    {
        public sealed override string HostName { get; set; }

        [Import]
        public sealed override IServiceLocator ServiceLocator { get; set; }

        [Import]
        public sealed override Logger Logger { get; set; }


        public MefServiceHostHelper()
        {
            HostName = "SAMPLE SERVICE HOST";
        }
    }

}
