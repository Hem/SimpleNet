using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace SimpleNet.ServiceHost.Contracts.Bindings
{
    // TODO: Use security and encryption
    // http://www.felinesoft.com/blog/index.php/2014/02/securing-a-wcf-service-with-username-and-password-using-message-security-and-the-channel-factory-pattern/

    public class StandardBindings
    {

        #region Basic HTTP Binding

        public static BasicHttpBinding GetBasicHttpBinding()
        {
            var binding = new BasicHttpBinding();
            Configure(binding);
            return binding;
        }
        public static BasicHttpBinding GetBasicHttpBinding(String bindingName)
        {
            return new BasicHttpBinding(bindingName);
        }
        public static BasicHttpBinding GetBasicHttpBinding(BasicHttpSecurityMode mode)
        {
            var binding = new BasicHttpBinding(mode);
            Configure(binding);
            return binding;
        }
        public static void Configure(HttpBindingBase binding)
        {
            if (binding == null)
                throw new ArgumentNullException("binding");

            binding.BypassProxyOnLocal = true;
            binding.SendTimeout = new TimeSpan(0, 0, 30, 0); // 30 minute timeout
            binding.MaxBufferSize = Int32.MaxValue;
            binding.MaxBufferPoolSize = 2147483647;
            binding.MaxReceivedMessageSize = Int32.MaxValue;
            binding.ReaderQuotas.MaxArrayLength = Int32.MaxValue;
            binding.ReaderQuotas.MaxBytesPerRead = Int32.MaxValue;
            binding.ReaderQuotas.MaxDepth = Int32.MaxValue;
            binding.ReaderQuotas.MaxNameTableCharCount = Int32.MaxValue;
            binding.ReaderQuotas.MaxStringContentLength = Int32.MaxValue;
        }

        #endregion
        
        #region MSMQ Binding

        public static Binding GetMsmqBinding(bool transactional)
        {
            var binding = new NetMsmqBinding();
            Configure(binding, transactional);
            return binding;
        }

        public static Binding GetMsmqBinding(String bindingName)
        {
            var binding = new NetMsmqBinding(bindingName);
            return binding;
        }

        public static Binding GetMsmqBinding(bool transactional, NetMsmqSecurityMode mode)
        {
            var binding = new NetMsmqBinding(mode);
            Configure(binding, transactional);
            return binding;
        }

        private static void Configure(NetMsmqBinding binding, bool trasactional)
        {
            if (binding == null)
                throw new ArgumentNullException("binding");

            binding.SendTimeout = new TimeSpan(0, 0, 10, 0);
            binding.OpenTimeout = new TimeSpan(0, 0, 10, 0);
            binding.CloseTimeout = new TimeSpan(0, 0, 10, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 0, 10, 0);
            binding.DeadLetterQueue = DeadLetterQueue.System;
            binding.Durable = true;
            binding.ExactlyOnce = trasactional;
            binding.UseActiveDirectory = false;
            
            binding.MaxBufferPoolSize = 2147483647;
            binding.MaxReceivedMessageSize = Int32.MaxValue;
            binding.MaxRetryCycles = 2;
            binding.ReceiveErrorHandling = ReceiveErrorHandling.Fault;
            binding.ReceiveRetryCount = 5;
            binding.RetryCycleDelay = new TimeSpan(0, 0, 30, 0);
            binding.TimeToLive = new TimeSpan(1, 0, 0, 0);

            binding.UseSourceJournal = false;
            binding.UseMsmqTracing = false;
            binding.QueueTransferProtocol = QueueTransferProtocol.Native;

            binding.ReaderQuotas.MaxArrayLength = Int32.MaxValue;
            binding.ReaderQuotas.MaxBytesPerRead = Int32.MaxValue;
            binding.ReaderQuotas.MaxDepth = Int32.MaxValue;
            binding.ReaderQuotas.MaxNameTableCharCount = Int32.MaxValue;
            binding.ReaderQuotas.MaxStringContentLength = Int32.MaxValue;
        }

        #endregion
        
        #region NET TCP Binding

        public static Binding GetNetTcpBinding()
        {
            var binding = new NetTcpBinding();
            Configure(binding);
            return binding;
        }
        public static Binding GetNetTcpBinding(SecurityMode mode)
        {
            var binding = new NetTcpBinding(mode);
            Configure(binding);
            return binding;
        }
        public static void Configure(NetTcpBinding binding)
        {
            if (binding == null)
                throw new ArgumentNullException("binding");

            
            binding.SendTimeout = new TimeSpan(0, 0, 10, 0);
            binding.MaxBufferSize = Int32.MaxValue;
            binding.MaxBufferPoolSize = 2147483647;
            binding.MaxReceivedMessageSize = Int32.MaxValue;
            binding.ReaderQuotas.MaxArrayLength = Int32.MaxValue;
            binding.ReaderQuotas.MaxBytesPerRead = Int32.MaxValue;
            binding.ReaderQuotas.MaxDepth = Int32.MaxValue;
            binding.ReaderQuotas.MaxNameTableCharCount = Int32.MaxValue;
            binding.ReaderQuotas.MaxStringContentLength = Int32.MaxValue;
            binding.PortSharingEnabled = true;
            binding.TransferMode = TransferMode.Buffered;
            
        }

        #endregion
    }
}
