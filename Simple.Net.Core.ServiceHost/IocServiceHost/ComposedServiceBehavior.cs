using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Microsoft.Practices.ServiceLocation;

namespace Simple.Net.Core.ServiceHost.IocServiceHost
{
    [ExcludeFromCodeCoverage]
    public class ComposedServiceBehavior : IServiceBehavior
    {
        private readonly IInstanceProvider _instanceProvider;

        public ComposedServiceBehavior(Type serviceType, IServiceLocator serviceLocator)
        {
            _instanceProvider = new ComposedInstanceProvider(serviceType, serviceLocator);
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints,
            BindingParameterCollection bindingParameters)
        {

        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcherBase dispatcher in serviceHostBase.ChannelDispatchers)
            {
                var channelDispatcher = dispatcher as ChannelDispatcher;
                if (channelDispatcher == null)
                    continue;

                foreach (EndpointDispatcher endpointDispatcher in channelDispatcher.Endpoints)
                {
                    endpointDispatcher.DispatchRuntime.InstanceProvider = _instanceProvider;
                }
            }

        }
    }
}
