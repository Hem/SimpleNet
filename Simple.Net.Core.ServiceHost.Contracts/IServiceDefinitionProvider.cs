using System;
using System.Collections.Generic;

namespace SimpleNet.ServiceHost.Contracts
{
    public interface IServiceDefinitionProvider
    {
        IEnumerable<ServiceDefinitionBase> ListServiceDefinitions(string hostName);
    }
}