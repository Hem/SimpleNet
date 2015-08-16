using System.ComponentModel.Composition;
using Microsoft.Practices.ServiceLocation;
using Simple.Net.Core.Diagnostics;
using Simple.Net.Core.ServiceHost.Helpers;

namespace SampleServiceHost.Helper
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
