using System;
using System.Collections.Generic;
using System.IdentityModel.Configuration;
using System.ServiceModel;
using System.ServiceModel.Description;
using Microsoft.Practices.ServiceLocation;
using Simple.Net.Core.Diagnostics;
using Simple.Net.Core.ServiceHost.Contracts;
using Simple.Net.Core.ServiceHost.Contracts.Bindings;
using Simple.Net.Core.ServiceHost.IocServiceHost;
using Simple.Net.Core.ServiceHost.Validators;

namespace Simple.Net.Core.ServiceHost.Helpers
{

    // Read: http://leastprivilege.com/2012/07/16/wcf-and-identity-in-net-4-5-usernamepassword-authentication/

    public abstract class AbstractServiceHostHelper
    {
        
        public abstract String HostName { get; set; }

        public abstract IServiceLocator ServiceLocator { get; set; }

        public abstract Logger Logger { get; set; }


        protected readonly List<ServiceDefinitionHost> ServiceDefinitionHosts = new List<ServiceDefinitionHost>();



        public List<string> OnStart(string[] args)
        {
            var startedList = new List<string>();

            try
            {
                // close the service definition hosts if necessary.
                foreach (var servicehost in ServiceDefinitionHosts)
                {
                    servicehost.Host.Close();
                }
                ServiceDefinitionHosts.Clear();

                
                var serviceDefs = ServiceLocator.GetAllInstances<IServiceDefinitionProvider>();


                foreach (var defProvider in serviceDefs)
                {
                    var defs = defProvider.ListServiceDefinitions(HostName);


                    if (defs == null) continue;


                    foreach (var sd in defs)
                    {
                        Logger.Info(String.Format("Starting:{0} of type:{1}", sd.ServiceDescription, sd.ServiceType));


                        ServiceDefinitionHost sdh = null;

                        switch (sd.ServiceType)
                        {

                            case ServiceTypeDefinition.SelfContainedHost:

                                var selfService = sd as SelfContainerServiceDefinition;

                                if (selfService != null)
                                {
                                    sdh = new ServiceDefinitionHost
                                    {
                                        ServiceDefinition = sd,
                                        SelfContainedHost = ServiceLocator.GetInstance<ISelfContainedHost>(selfService.ServiceContractName)
                                    };
                                    sdh.SelfContainedHost.Start();
                                }
                                break;

                            case ServiceTypeDefinition.WcfMsmqServiceHost:

                                var wcfServiceMsmq = sd as WcfServiceDefinition;

                                if (wcfServiceMsmq != null)
                                {
                                    var baseAddressUri = new Uri(wcfServiceMsmq.ServiceAddress.WcfEndpointUrl, UriKind.Absolute);
                                    var msmqBindingUri = new Uri(wcfServiceMsmq.ServiceAddress.MsmqBindingUrl, UriKind.Absolute);

                                    Logger.Info(String.Format("... URL:{0}", baseAddressUri));
                                    Logger.Info(String.Format("... Binding URL: {0}", msmqBindingUri));

                                    sdh = new ServiceDefinitionHost
                                    {
                                        ServiceDefinition = sd,
                                        Host = new ComposedServiceHost(wcfServiceMsmq.WcfServiceOperationType, ServiceLocator, baseAddressUri)
                                    };

                                    sdh.Host.AddServiceEndpoint(wcfServiceMsmq.WcfServiceInterfaceType, wcfServiceMsmq.ServiceAddress.Binding, baseAddressUri, msmqBindingUri);


                                    if (wcfServiceMsmq.EnableWsdl)
                                    {
                                        var metaDataBehavior = StandardServiceBehaviors.TcpServiceMetadataBehavior;

                                        if (!sdh.Host.Description.Behaviors.Contains(metaDataBehavior))
                                        {
                                            sdh.Host.Description.Behaviors.Add(metaDataBehavior);
                                        }
                                    }


                                    //Configure metadata behavior
                                    if (wcfServiceMsmq.ServiceBehaviors != null)
                                        foreach (var behavior in wcfServiceMsmq.ServiceBehaviors)
                                        {
                                            if (!sdh.Host.Description.Behaviors.Contains(behavior))
                                                sdh.Host.Description.Behaviors.Add(behavior);
                                        }


                                    if (wcfServiceMsmq.EnableDebugBehavior)
                                    {
                                        if (sdh.Host.Description.Behaviors.Find<ServiceDebugBehavior>() != null)
                                        {
                                            sdh.Host.Description.Behaviors.Find<ServiceDebugBehavior>()
                                                .IncludeExceptionDetailInFaults = true;
                                        }
                                        else
                                        {
                                            sdh.Host.Description.Behaviors.Add(new ServiceDebugBehavior
                                            {
                                                IncludeExceptionDetailInFaults = true
                                            });
                                        }
                                    }

                                    sdh.Host.Open();


                                    foreach (var uri in sdh.Host.BaseAddresses)
                                    {
                                        Logger.Info(String.Format("Listening at:{0}", uri));
                                    }


                                }

                                break;

                            case ServiceTypeDefinition.WcfHttpServiceHost:

                                var wcfServiceHttp = sd as WcfServiceDefinition;

                                if (wcfServiceHttp != null)
                                {

                                    var url = new Uri(wcfServiceHttp.ServiceAddress.WcfEndpointUrl, UriKind.Absolute);
                                    var isHttps = wcfServiceHttp.ServiceAddress.WcfEndpointUrl.StartsWith(@"https://", StringComparison.CurrentCultureIgnoreCase);

                                    sdh = new ServiceDefinitionHost
                                    {
                                        ServiceDefinition = sd,
                                        Host = new ComposedServiceHost(wcfServiceHttp.WcfServiceOperationType, ServiceLocator, url)
                                    };

                                    sdh.Host.AddServiceEndpoint(wcfServiceHttp.WcfServiceInterfaceType, wcfServiceHttp.ServiceAddress.Binding, url);


                                    if (wcfServiceHttp.EnableWsdl)
                                    {
                                        var metaDataBehavior = isHttps ? StandardServiceBehaviors.HttpsServiceMetadataBehavior :
                                                                         StandardServiceBehaviors.HttpServiceMetadataBehavior;

                                        if (!sdh.Host.Description.Behaviors.Contains(metaDataBehavior))
                                        {
                                            sdh.Host.Description.Behaviors.Add(metaDataBehavior);
                                        }
                                    }

                                    //Configure metadata behavior
                                    if (wcfServiceHttp.ServiceBehaviors != null)
                                    {
                                        foreach (var b in wcfServiceHttp.ServiceBehaviors)
                                        {   
                                            if (!sdh.Host.Description.Behaviors.Contains(b))
                                                sdh.Host.Description.Behaviors.Add(b);
                                        }
                                    }


                                    if (wcfServiceHttp.EnableDebugBehavior)
                                    {
                                        if (sdh.Host.Description.Behaviors.Find<ServiceDebugBehavior>() != null)
                                        {
                                            sdh.Host.Description.Behaviors.Find<ServiceDebugBehavior>()
                                                .IncludeExceptionDetailInFaults = true;
                                        }
                                        else
                                        {
                                            sdh.Host.Description.Behaviors.Add(new ServiceDebugBehavior
                                            {
                                                IncludeExceptionDetailInFaults = true
                                            });
                                        }

                                    }

                                    // this enables default windows authentication
                                    if( wcfServiceHttp.EnableSecurity )
                                    {
                                        // Maybe we need to move this to another setting
                                        sdh.Host.Description.Behaviors.Find<ServiceAuthorizationBehavior>().PrincipalPermissionMode = PrincipalPermissionMode.Always;
                                        sdh.Host.Credentials.UseIdentityConfiguration = true;
                                        
                                    }

                                    if (wcfServiceHttp.CustomUserNamePasswordValidator != null)
                                    {
                                        // Maybe we need to move this to another setting
                                        sdh.Host.Description.Behaviors.Find<ServiceAuthorizationBehavior>().PrincipalPermissionMode = PrincipalPermissionMode.Always;
                                        sdh.Host.Credentials.UseIdentityConfiguration = true;

                                        sdh.Host.Credentials.IdentityConfiguration = new IdentityConfiguration();
                                        sdh.Host.Credentials.IdentityConfiguration.SecurityTokenHandlers.AddOrReplace(
                                            new CustomUserNamePasswordValidatorSecurityTokenHandler
                                            {
                                                Validator = wcfServiceHttp.CustomUserNamePasswordValidator
                                            });

                                        //sdh.Host.Credentials.UserNameAuthentication.UserNamePasswordValidationMode = UserNamePasswordValidationMode.Custom;
                                        //sdh.Host.Credentials.UserNameAuthentication.CustomUserNamePasswordValidator = wcfServiceHttp.CustomUserNamePasswordValidator;
                                    }

                                    //sdh.Host.Credentials.ServiceCertificate.SetCertificate(StoreLocation.CurrentUser, StoreName.Root, X509FindType.FindByIssuerName, "Correct Care Solutions");

                                    sdh.Host.Open();

                                    foreach (var uri in sdh.Host.BaseAddresses)
                                    {
                                        Logger.Info(String.Format("Listening at:{0}", uri));
                                    }

                                }
                                break;






                            case ServiceTypeDefinition.WcfTcpServiceHost:

                                var wcfServiceTcp = sd as WcfServiceDefinition;

                                if (wcfServiceTcp != null)
                                {

                                    var urlList = new List<Uri>();

                                    var url = new Uri(wcfServiceTcp.ServiceAddress.WcfEndpointUrl, UriKind.Absolute);
                                    var wsdlUrl = wcfServiceTcp.ServiceAddress.WsdlUrl == null ? null : new Uri(wcfServiceTcp.ServiceAddress.WsdlUrl, UriKind.Absolute);

                                    urlList.Add(url);
                                    if (wsdlUrl != null) urlList.Add(wsdlUrl);

                                    sdh = new ServiceDefinitionHost
                                    {
                                        ServiceDefinition = sd,
                                        Host = new ComposedServiceHost(wcfServiceTcp.WcfServiceOperationType, ServiceLocator, urlList.ToArray())
                                    };

                                    sdh.Host.AddServiceEndpoint(wcfServiceTcp.WcfServiceInterfaceType, wcfServiceTcp.ServiceAddress.Binding, url);

                                    // no idea if this works... grr.
                                    if (wcfServiceTcp.EnableWsdl && wsdlUrl != null)
                                    {
                                        var metaDataBehavior = StandardServiceBehaviors.HttpServiceMetadataBehavior;

                                        if (!sdh.Host.Description.Behaviors.Contains(metaDataBehavior))
                                        {
                                            sdh.Host.Description.Behaviors.Add(metaDataBehavior);
                                        }
                                        // NOTE: This requires a NET.TCP binding and NOT a HTTP Binding.
                                        var mexBinding = MetadataExchangeBindings.CreateMexHttpBinding();
                                        sdh.Host.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName, mexBinding, "mex", wsdlUrl);
                                        
                                        Logger.Info( "WSDL URL: " + wsdlUrl );
                                    }

                                    //Configure metadata behavior
                                    if (wcfServiceTcp.ServiceBehaviors != null)
                                    {
                                        foreach (var behavior in wcfServiceTcp.ServiceBehaviors)
                                        {
                                            if (!sdh.Host.Description.Behaviors.Contains(behavior))
                                                sdh.Host.Description.Behaviors.Add(behavior);
                                        }
                                    }

                                    if (wcfServiceTcp.EnableDebugBehavior)
                                    {
                                        if (sdh.Host.Description.Behaviors.Find<ServiceDebugBehavior>() != null)
                                        {
                                            sdh.Host.Description.Behaviors.Find<ServiceDebugBehavior>()
                                                .IncludeExceptionDetailInFaults = true;
                                        }
                                        else
                                        {
                                            sdh.Host.Description.Behaviors.Add(new ServiceDebugBehavior
                                            {
                                                IncludeExceptionDetailInFaults = true
                                            });
                                        }

                                    }


                                    sdh.Host.Open();

                                    foreach (var uri in sdh.Host.BaseAddresses)
                                    {
                                        Logger.Info(String.Format("Listening at:{0}", uri));
                                    }

                                }


                                break;
                        }


                        if (sdh == null) continue;
                        ServiceDefinitionHosts.Add(sdh);
                        startedList.Add(sd.ServiceDescription);
                    }


                } // foreach (var defProviderLazy



            }
            catch (Exception ex)
            {
                Logger.Error( "Service Failed To Start", ex );

                throw new ServiceActivationException(String.Format("Service Host:{0} failed to start", HostName), ex);
            }

            return startedList;
        }


        public List<String> OnStop()
        {
            var stoppedList = new List<string>();

            // close the service definition hosts if necessary.
            foreach (var servicehost in ServiceDefinitionHosts)
            {
                switch (servicehost.ServiceDefinition.ServiceType)
                {
                    case ServiceTypeDefinition.SelfContainedHost:
                        if (servicehost.SelfContainedHost == null) continue;
                        Logger.Info("...Stopping:" + servicehost.ServiceDefinition.ServiceDescription);
                        servicehost.SelfContainedHost.Stop();

                        break;

                    case ServiceTypeDefinition.WcfHttpServiceHost:

                        if (servicehost.Host == null) continue;
                        Logger.Info("...Stopping:" + servicehost.ServiceDefinition.ServiceDescription);
                        servicehost.Host.Close();

                        break;
                }

                stoppedList.Add(servicehost.ServiceDefinition.ServiceDescription);
            }
            ServiceDefinitionHosts.Clear();

            return stoppedList;
        }


    }
}
