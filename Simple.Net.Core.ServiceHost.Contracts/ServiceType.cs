namespace Simple.Net.Core.ServiceHost.Contracts
{
    public enum ServiceTypeDefinition
    {
        SelfContainedHost,
        WcfHttpServiceHost,
        WcfMsmqServiceHost,
        WcfTcpServiceHost
    }
}