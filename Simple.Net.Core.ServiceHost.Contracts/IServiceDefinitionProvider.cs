using System;
using System.Collections.Generic;

namespace Simple.Net.Core.ServiceHost.Contracts
{
    public interface IServiceDefinitionProvider
    {
        IEnumerable<ServiceDefinitionBase> ListServiceDefinitions(String hostName);
    }
}