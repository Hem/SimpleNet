using System;
using System.Diagnostics.CodeAnalysis;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using Microsoft.Practices.ServiceLocation;

namespace Simple.Net.Core.ServiceHost.IocServiceHost
{
    [ExcludeFromCodeCoverage]
    public class ComposedInstanceProvider : IInstanceProvider
    {
        private static readonly Object Lock = new object();

        private readonly Type _serviceType;
        private readonly IServiceLocator _serviceLocator;

        public ComposedInstanceProvider(Type serviceType, IServiceLocator serviceLocator)
        {
            _serviceType = serviceType;
            _serviceLocator = serviceLocator;
        }

        public object GetInstance(InstanceContext context)
        {
            lock (Lock)
            {
                var export = GetServiceExport();
                if (export == null)
                    throw new InvalidOperationException();

                return export;
            }

        }

        public object GetInstance(InstanceContext context, Message message)
        {
            return GetInstance(context);
        }

        public void ReleaseInstance(InstanceContext context, object instance)
        {
            var disposable = instance as IDisposable;

            if (disposable != null)
                disposable.Dispose();
        }

        private Object GetServiceExport()
        {

            return _serviceLocator.GetInstance(_serviceType);

        }
    }

}
