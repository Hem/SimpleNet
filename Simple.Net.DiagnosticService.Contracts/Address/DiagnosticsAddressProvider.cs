﻿using System;
using System.ComponentModel.Composition;
using System.ServiceModel;
using Simple.Net.Core.ServiceHost.Contracts.Address;
using Simple.Net.Core.ServiceHost.Contracts.Bindings;

namespace Simple.Net.DiagnosticService.Contracts.Address
{
    public class DiagnosticsAddressProvider
    {
        /// <summary>
        /// Export IWcfServiceAddress with this key please...
        /// </summary>
        public const string DIAGNOSTICS_SERVICE_ADDRESS = "DIAGNOSTICS_SERVICE_ADDRESS";


        
        [Import(DIAGNOSTICS_SERVICE_ADDRESS, AllowDefault = true)]
        public string ServiceAddress { get; set; }
        



        [Export(DIAGNOSTICS_SERVICE_ADDRESS, typeof(IWcfServiceAddress))]
        public IWcfServiceAddress GetDiagnosticServiceAddress
        {
            get
            {
                if( String.IsNullOrEmpty( ServiceAddress ) )
                    return null;


                return new WcfServiceAddress
                {
                    Binding = StandardBindings.GetNetTcpBinding( SecurityMode.None ),
                    WcfEndpointUrl = ServiceAddress
                };
            }

        }
    }
}
