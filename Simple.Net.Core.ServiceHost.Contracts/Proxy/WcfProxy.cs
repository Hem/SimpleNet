using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace Simple.Net.Core.ServiceHost.Contracts.Proxy
{
    
    public class WcfProxy<T> : IUpdateWcfProxyCredentials
    {
        /// <summary>
        /// Please create an instace of the channel factory...
        /// </summary>
        protected ChannelFactory<T> ChannelFactory { get; set; }


        public WcfProxy(Binding binding, String wcfEndpointUrl)
        {

            ChannelFactory = new ChannelFactory<T>(binding, new EndpointAddress(wcfEndpointUrl));
        }




        /// <summary>
        /// Authentication user name and password for WCF endpoint!
        /// </summary>
        public void UpdateWcfEndpointCredentials(String username, String password)
        {
            if (ChannelFactory.Credentials != null)
            {
                ChannelFactory.Credentials.UserName.UserName = username;
                ChannelFactory.Credentials.UserName.Password = password;
            }
        }


        /// <summary>
        /// Authentication user name and password for network proxy
        /// </summary>
        public void UpdateNetworkProxyCredentials(string username, string password, string domaninName)
        {
            if (ChannelFactory.Credentials != null)
            {
                ChannelFactory.Credentials.Windows.ClientCredential
                    = new NetworkCredential(username, password, domaninName);
            }
        }


        public void Process(Action<T> action)
        {
            using (var client = (IClientChannel)ChannelFactory.CreateChannel())
            {
                try
                {
                    action((T)client);
                }
                catch
                {
                    client.Abort();
                    throw;
                }
            }
        }


        public TResult Process<TResult>(Func<T, TResult> action)
        {
            using (var client = (IClientChannel)ChannelFactory.CreateChannel())
            {
                try
                {
                    return action((T)client);
                }
                catch
                {
                    client.Abort();
                    throw;
                }
            }
        }

        public async Task ProcessAsync(Func<T, Task> action)
        {
            using (var client = (IClientChannel)ChannelFactory.CreateChannel())
            {
                try
                {
                    await action((T)client);
                }
                catch
                {
                    client.Abort();
                    throw;
                }
            }
        }

        public async Task<TResult> ProcessAsync<TResult>(Func<T, Task<TResult>> action)
        {
            using (var client = (IClientChannel)ChannelFactory.CreateChannel())
            {
                try
                {
                    return await action((T)client);
                }
                catch
                {
                    client.Abort();
                    throw;
                }
            }
        }

    }
}

