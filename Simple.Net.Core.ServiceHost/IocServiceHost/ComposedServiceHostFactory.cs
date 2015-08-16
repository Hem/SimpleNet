using System;
using System.ServiceModel.Activation;
using Microsoft.Practices.ServiceLocation;

namespace Simple.Net.Core.ServiceHost.IocServiceHost
{
    // you can then modify the .svc file so that it uses it in the Factory attribute:
    // <% @ServiceHost Service="FooService" Factory="ComposedServiceHostFactory" %>
    public class ComposedServiceHostFactory : ServiceHostFactory
    {
        //public static IServiceLocator ServiceLocator { get; set; }

        protected override System.ServiceModel.ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {

            if(!ServiceLocator.IsLocationProviderSet)
                throw new Exception("Service locator is not set, please set it using ServiceLocator.SetLocatorProvider(() => locator); ");

            var serviceLocator = ServiceLocator.Current;

            return CreateComposedServiceHost(serviceType, baseAddresses, serviceLocator);

        }
        protected virtual ComposedServiceHost CreateComposedServiceHost(Type serviceType, Uri[] baseAddresses, IServiceLocator serviceLocator)
        {
            return new ComposedServiceHost(serviceType, serviceLocator, baseAddresses);
        }
    }

}
