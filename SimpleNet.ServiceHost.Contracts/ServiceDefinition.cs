using System;
using System.IdentityModel.Selectors;
using System.ServiceModel.Description;
using SimpleNet.ServiceHost.Contracts.Address;

namespace SimpleNet.ServiceHost.Contracts
{
    public class ServiceDefinitionBase
    {
        // This is the service description
        public String ServiceDescription { get; set; }

        // This is the type of service
        public ServiceTypeDefinition ServiceType { get; set; }
    }


    public class SelfContainerServiceDefinition : ServiceDefinitionBase
    {
        /// <summary>
        /// Required when ServiceType = SelfContainedHost 
        /// MEF Enabled: [Export("CONTRACT_NAME", typeof(ISelfContainedHost))]
        /// </summary>
        public string ServiceContractName { get; set; }
    }


    public class WcfServiceDefinition : ServiceDefinitionBase
    {
        
        /// <summary>
        /// Required when ServiceType = ServiceHost
        /// This is the Interface contract for the service
        /// NOTE: Service can be MEF Enabled.
        /// </summary>
        public Type WcfServiceInterfaceType { get; set; }

        /// <summary>
        /// Required when ServiceType = ServiceHost
        /// NOTE: Service can be MEF Enabled.
        /// This is the Service Implementation
        /// </summary>
        public Type WcfServiceOperationType { get; set; }



        // Add additional service behaviors such as logging, error handling etc..
        public IServiceBehavior[] ServiceBehaviors { get; set; }


        /// <summary>
        /// Service Addresses such as EndPointAddress, WSDL Address, etc...
        /// </summary>
        public IWcfServiceAddress ServiceAddress { get; set; }

        /// <summary>
        /// Flag to enable WSDL
        /// </summary>
        public bool EnableWsdl { get; set; }

        /// <summary>
        /// Flag to enable debug behavior
        /// </summary>
        public bool EnableDebugBehavior { get; set; }


        /// <summary>
        /// Enable security, defaults to windows active directory security
        /// </summary>
        public bool EnableSecurity { get; set; }

        /// <summary>
        /// Provide the user name password validator service allowing for custom user name password validation
        /// </summary>
        public UserNamePasswordValidator CustomUserNamePasswordValidator { get; set; }
    }


}
