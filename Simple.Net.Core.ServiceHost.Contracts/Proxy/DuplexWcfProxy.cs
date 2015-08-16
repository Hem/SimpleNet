using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using Simple.Net.Core.ServiceHost.Contracts.Address;

namespace Simple.Net.Core.ServiceHost.Contracts.Proxy
{
    public abstract class DuplexWcfProxy<T, TV> : IUpdateWcfProxyCredentials, IWcfProxyChannel, IDisposable
    {
        protected DuplexChannelFactory<T> ChannelFactory { get; set; }
        protected IWcfServiceAddress Address { get; set; }
        protected TV CallbackImplementation { get; set; }


        /// <summary>
        /// Do not close this channel until the application is ready to close...
        /// </summary>
        protected IChannel Channel { get; private set; }

        protected IChannel GetChannel()
        {
            lock (this)
            {
                if (ChannelFactory.State == CommunicationState.Faulted)
                    EstablishChannel();

                if (ChannelFactory.State == CommunicationState.Closed)
                    ChannelFactory.Open();

                if (Channel == null || Channel.State == CommunicationState.Closed)
                {
                    Channel = (IChannel)ChannelFactory.CreateChannel();
                }

                return Channel;
            }
        }


        protected DuplexWcfProxy(IWcfServiceAddress address, TV callback)
        {
            Address = address;
            CallbackImplementation = callback;

            EstablishChannel();
        }


        protected void EstablishChannel()
        {
            if (ChannelFactory != null)
            {
                ChannelFactory.Abort();
            }

            ChannelFactory = new DuplexChannelFactory<T>(
                                                           new InstanceContext(CallbackImplementation),
                                                           Address.Binding,
                                                           new EndpointAddress(Address.WcfEndpointUrl));

            ChannelFactory.Faulted += ChannelFactoryOnFaulted;
        }
        protected void ChannelFactoryOnFaulted(object sender, EventArgs eventArgs)
        {
            while (true)
            {
                try
                {
                    EstablishChannel();
                    break;
                }
                catch
                {
                    Task.Delay(1000).Wait();
                }
            }
        }




        /// <summary>
        /// Invoke to close the channel!
        /// Needed so we can dispose correctly
        /// </summary>
        public void CloseChannel()
        {
            if (ChannelFactory != null)
                ChannelFactory.Close();
        }

        public void OpenChannel()
        {
            if( ChannelFactory != null )
                ChannelFactory.Open();
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


        /// <summary>
        /// Implementation to make service calls
        /// </summary>
        /// <param name="action"></param>
        public void Process(Action<T> action)
        {
            try
            {
                var client = GetChannel();
                action((T)client);
            }
            catch (CommunicationObjectFaultedException)
            {
                EstablishChannel();
            }

        }


        public void Dispose()
        {
            var channel = GetChannel();

            if (channel != null && channel.State != CommunicationState.Closed)
            {
                channel.Close();
                channel.Abort();
            }

            CloseChannel();
        }


    }
}