using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace SimpleNet.ServiceHost.Contracts.Bindings
{
    public class StandardPclBindings
    {

        #region Basic HTTP Binding

        public static Binding GetBasicHttpBinding()
        {
            var binding = new BasicHttpBinding();
            Configure(binding);
            return binding;
        }
        
        public static Binding GetBasicHttpBinding(BasicHttpSecurityMode mode)
        {
            var binding = new BasicHttpBinding(mode);
            Configure(binding);
            return binding;
        }
        public static void Configure(HttpBindingBase binding)
        {
            if (binding == null)
                throw new ArgumentNullException("binding");

            //binding.BypassProxyOnLocal = true;
            binding.OpenTimeout = new TimeSpan(0, 0, 10, 0); // 30 minute timeout
            binding.SendTimeout = new TimeSpan(0, 0, 10, 0); // 30 minute timeout
            binding.ReceiveTimeout = new TimeSpan(0, 0, 10, 0); // 30 minute timeout
            binding.CloseTimeout = new TimeSpan(0, 0, 10, 0); // 30 minute timeout

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


        #region NET HTTP Binding

        public static Binding GetNetHttpBinding()
        {
            var binding = new NetHttpBinding();
            Configure(binding);
            return binding;
        }


        public static Binding GetNetHttpBinding(BasicHttpSecurityMode mode)
        {
            var binding = new NetHttpBinding(mode);
            Configure(binding);
            return binding;
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
            binding.OpenTimeout = new TimeSpan(0, 0, 10, 0);
            binding.CloseTimeout = new TimeSpan(0, 0, 10, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 0, 10, 0);

            binding.MaxBufferSize = Int32.MaxValue;
            binding.MaxBufferPoolSize = 2147483647;
            binding.MaxReceivedMessageSize = Int32.MaxValue;
            binding.ReaderQuotas.MaxArrayLength = Int32.MaxValue;
            binding.ReaderQuotas.MaxBytesPerRead = Int32.MaxValue;
            binding.ReaderQuotas.MaxDepth = Int32.MaxValue;
            binding.ReaderQuotas.MaxNameTableCharCount = Int32.MaxValue;
            binding.ReaderQuotas.MaxStringContentLength = Int32.MaxValue;
            // binding.PortSharingEnabled = true;
            binding.TransferMode = TransferMode.Buffered;
            
        }

        #endregion
    }
}
