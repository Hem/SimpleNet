using System;
using System.ServiceModel.Channels;

namespace Simple.Net.Core.ServiceHost.Contracts.Address
{
    public interface IWcfServiceAddress
    {
        /// <summary>
        /// Binding Information
        /// </summary>
        Binding Binding { get; }

        /// <summary>
        /// The WCF Endpoint URL
        /// </summary>
        String WcfEndpointUrl { get; }

        /// <summary>
        /// The url used for purposes of binding to MSMQ (mostly)
        /// </summary>
        String MsmqBindingUrl { get; }

        /// <summary>
        /// The URL for WSDL information if mex is available
        /// </summary>
        String WsdlUrl { get; }
    }
}