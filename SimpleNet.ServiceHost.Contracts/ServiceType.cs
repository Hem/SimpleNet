namespace SimpleNet.ServiceHost.Contracts
{
    public enum ServiceTypeDefinition
    {
        SelfContainedHost,
        WcfHttpServiceHost,
        WcfMsmqServiceHost,
        WcfTcpServiceHost
    }
}