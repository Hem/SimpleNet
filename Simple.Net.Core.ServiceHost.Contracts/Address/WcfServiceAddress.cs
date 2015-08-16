using System;
using System.ServiceModel.Channels;

namespace Simple.Net.Core.ServiceHost.Contracts.Address
{
    public class WcfServiceAddress : IWcfServiceAddress
    {
        /// <summary>
        /// Binding Information
        /// </summary>
        public Binding Binding { get; set; }

        /// <summary>
        /// The WCF Endpoint URL
        /// </summary>
        public String WcfEndpointUrl { get; set; }
        
        /// <summary>
        /// The url used for purposes of binding to MSMQ (mostly)
        /// </summary>
        public String MsmqBindingUrl { get; set; }

        /// <summary>
        /// The URL for WSDL information if mex is available
        /// </summary>
        public String WsdlUrl { get; set; }

        /// <summary>
        /// Tells the framework if we have to use an SSL
        /// </summary>
        public Boolean UseSsl { get; set; }
    }
}
