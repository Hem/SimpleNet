using System;

namespace Simple.Net.Core.ServiceHost.Contracts.Proxy
{
    public interface IUpdateWcfProxyCredentials
    {
        /// <summary>
        /// Updates the WCF Endpoint credentials.
        /// This is used to authenticate the user with the service endpoint (CCAS Enterprise Services)
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        void UpdateWcfEndpointCredentials( String username, String password );



        /// <summary>
        /// Updates the network proxy credentials, 
        /// This can be used to provide a user name and password to access the outside world.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="domaninName"></param>
        void UpdateNetworkProxyCredentials( String username, String password, String domaninName );
    }


    public interface IWcfProxyChannel
    {
        void CloseChannel();

        void OpenChannel();
    }

}