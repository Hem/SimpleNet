using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.ServiceModel.Configuration;

namespace Simple.Net.Core.ServiceHost.Contracts.Helpers
{
    /**
     * http://blogs.msdn.com/b/markgabarra/archive/2006/04/27/585607.aspx
     * http://blogs.msdn.com/b/markgabarra/archive/2006/05/08/592880.aspx
     */
    public static class BindingHelper
    {
        // skipping unit test as it's not used in code anywhere yet.
        [ExcludeFromCodeCoverage]
        public static void EnumerateAvailableClientEndpoints()
        {
            Configuration appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ServiceModelSectionGroup serviceModel = ServiceModelSectionGroup.GetSectionGroup(appConfig);

            if (serviceModel == null) return;

            if (100 > Console.WindowWidth)
            {
                Console.WindowWidth = 100;
            }

            Console.WriteLine("Configuration Name : Address : Binding : Contract");

            foreach (ChannelEndpointElement endpoint in serviceModel.Client.Endpoints)
            {
                Console.WriteLine("{0} : {1} : {2} : {3}",
                    endpoint.Name,
                    endpoint.Address,
                    endpoint.Binding,
                    endpoint.Contract);
            }
        }

        public static IEnumerable<CcsServiceEndpoint> EnumerateAvailableServiceEndpoints()
        {
            var endpoints = new List<CcsServiceEndpoint>();
            var appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var serviceModel = ServiceModelSectionGroup.GetSectionGroup(appConfig);

            if (serviceModel != null)
                foreach (ServiceElement service in serviceModel.Services.Services)
                {
                    endpoints.AddRange(from ServiceEndpointElement endpoint in service.Endpoints
                                       select new CcsServiceEndpoint
                                       {
                                           ServiceName = service.Name,
                                           EndpointName = endpoint.Name,
                                           Address = endpoint.Address,
                                           Binding = endpoint.Binding,
                                           Contract = endpoint.Contract
                                       });
                }

            return endpoints;
        }

        // skipping unit test as it's not used in code anywhere yet.
        [ExcludeFromCodeCoverage]
        public static void EnumerateAvailableServiceEndpoints(string serviceConfigFile)
        {
            var fileMap = new ExeConfigurationFileMap { ExeConfigFilename = serviceConfigFile };
            var appConfig = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            var serviceModel = ServiceModelSectionGroup.GetSectionGroup(appConfig);


            if (serviceModel == null) return;

            Trace.WriteLine("Service Name: Configuration Name : Address : Binding : Contract");
            foreach (ServiceElement service in serviceModel.Services.Services)
            {
                foreach (ServiceEndpointElement endpoint in service.Endpoints)
                {
                    Trace.WriteLine(String.Format("{0} : {1} : {2} : {3} : {4}",
                        service.Name,
                        endpoint.Name,
                        endpoint.Address,
                        endpoint.Binding,
                        endpoint.Contract));
                }
            }
        }

        // skipping unit test as it's not used in code anywhere yet.
        [ExcludeFromCodeCoverage]
        public static void EnumerateAllAvailableBindingElementExtensions()
        {
            var serviceModelExtensions = (ExtensionsSection)ConfigurationManager.GetSection("system.serviceModel/extensions");

            if (97 > Console.WindowWidth)
            {
                Console.WindowWidth = 97;
            }
            Console.WriteLine("{0,-22} : {1}",
                "Xml Element Name",
                "Underlying Binding Element Type");
            foreach (ExtensionElement bindingElementExtension in serviceModelExtensions.BindingElementExtensions)
            {
                var type = Type.GetType(bindingElementExtension.Type);
                
                if (type != null)
                {
                    var bindingElement = (BindingElementExtensionElement)Activator.CreateInstance(type);


                    Trace.WriteLine(String.Format("{0,-22} : {1}", bindingElementExtension.Name, bindingElement.BindingElementType.FullName));
                }

            }
        }

        // skipping unit test as it's not used in code anywhere yet.
        [ExcludeFromCodeCoverage]
        public static void EnumerateConfiguredCustomBindings()
        {
            var bindings = (BindingsSection)ConfigurationManager.GetSection("system.serviceModel/bindings");

            if (97 > Console.WindowWidth)
            {
                Console.WindowWidth = 97;
            }
            foreach (CustomBindingElement customBinding in bindings.CustomBinding.Bindings)
            {
                Console.WriteLine("Binding '{0}':", customBinding.Name);
                foreach (BindingElementExtensionElement bindingElement in customBinding)
                {
                    Console.WriteLine("{0,-22} : {1}",
                        bindingElement.ConfigurationElementName,
                        bindingElement.BindingElementType.FullName);
                }
            }
        }
    }

    public class CcsServiceEndpoint
    {
        public string ServiceName { get; set; }
        public string EndpointName { get; set; }
        public Uri Address { get; set; }
        public string Binding { get; set; }
        public string Contract { get; set; }
    }
}
