using System;

namespace Simple.Net.Core.ServiceHost.Contracts.Address
{
    public class AppHostPortProvider
    {
        public String HostName { get; set; }
        public String HttpPort { get; set; }
        public String HttpsPort { get; set; }
        public String TcpPort { get; set; }
        public Boolean UseSsl { get; set; }

    }
}
