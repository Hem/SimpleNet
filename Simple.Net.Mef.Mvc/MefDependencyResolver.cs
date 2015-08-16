using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Web.Http.Dependencies;

namespace Simple.Net.Mef.Mvc
{



    /// <summary>
    /// Resolve dependencies for MVC / Web API using MEF.
    ///
    ///  var appPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");
    ///  var container = MefRegistrationForMvcWebApi.GetMefContainer(batch, appPath);
    /// 
    ///  var resolver = new MefDependencyResolver(container);
    ///  
    ///  DependencyResolver.SetResolver(resolver); // Install MEF dependency resolver for MVC
    ///  
    ///  System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = resolver; // Install MEF dependency resolver for Web API
    /// </summary>
    /// 
       
    public class MefDependencyResolver : IDependencyResolver, System.Web.Mvc.IDependencyResolver
    {
        private readonly CompositionContainer _container;

        public MefDependencyResolver(CompositionContainer container)
        {
            _container = container;
        }

        public IDependencyScope BeginScope()
        {
            return this;
        }

        /// <summary>
        /// Called to request a service implementation.
        /// 
        /// Here we call upon MEF to instantiate implementations of dependencies.
        /// </summary>
        /// <param name="serviceType">Type of service requested.</param>
        /// <returns>Service implementation or null.</returns>
        public object GetService(Type serviceType)
        {
            if (serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));

            var name = AttributedModelServices.GetContractName(serviceType);
            var export = _container.GetExportedValueOrDefault<object>(name);
            return export;
        }

        /// <summary>
        /// Called to request service implementations.
        /// 
        /// Here we call upon MEF to instantiate implementations of dependencies.
        /// </summary>
        /// <param name="serviceType">Type of service requested.</param>
        /// <returns>Service implementations.</returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));

            var exports = _container.GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));
            return exports;
        }

        public void Dispose()
        {
        }
    }
}
