namespace Simple.Net.Core.ServiceHost.Contracts
{
    public class ServiceDefinitionHost
    {
        public ServiceDefinitionBase ServiceDefinition { get; set; }
        public System.ServiceModel.ServiceHost Host { get; set; }
        public ISelfContainedHost SelfContainedHost { get; set; }
    }
}