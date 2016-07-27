using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Practices.ServiceLocation;
using SimpleNet.ServiceHost.Behaviors;

namespace SimpleNet.ServiceHost.IocServiceHost
{
    [ExcludeFromCodeCoverage]
    public class ComposedServiceHost : System.ServiceModel.ServiceHost
    {
        private readonly Type _serviceType;
        private readonly IServiceLocator _serviceLocator;


        public ComposedServiceHost(Type serviceType, IServiceLocator serviceLocator, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
            _serviceType = serviceType;
            _serviceLocator = serviceLocator;
        }

        protected override void OnOpening()
        {
            if (Description.Behaviors.Find<ComposedServiceBehavior>() == null)
            {
                Description.Behaviors.Add(new ComposedServiceBehavior(_serviceType, _serviceLocator));
            }

            if (Description.Behaviors.Find<ErrorBehavior>() == null)
            {
                Description.Behaviors.Add(new ErrorBehavior());
            }
            

            base.OnOpening();
        }

    }
}
