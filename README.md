SimpleNet
============================


A config free pluggable implementation of Windows services/WCF/Background processes.

> We support running the service in "debug mode" 
> * You will need to run Visual Studio as "Administrator" 
> * Alternatively you can open ports using netsh


### Run as Administrator in a command prompt
```
	sc.exe config NetTcpPortSharing start= demand
    netsh http add urlacl url=http://+:80/ user=EVERYONE
```

Feature List 
---------------------------

* Config free WCF
  * Programatically define endpoints
  * Progamatically define what services need to be started
  
* Support for various WCF Bindings
  * HTTP
  * NET.TCP
  * MSMQ 
  * etc..
  
* Simplified hosting of backgroud processes 
  * Start
  * Stop
  * Continue
  *
* Build with Dependency Injection in mind
  * Use adapter for Microsoft Service Provider


Library Projects
-----------------

You need to be aware of two projects
* SimpleNet.ServiceHost.Contracts :- 
  * Used in your Services Implementation 
  * Used in your WCF Client Proxy to invoke the WCF Service

* Used in your ServicesHost Container
    * This provides the snippets to start and stop the services
    * We use Microsoft Service Locator to help resolve dependencies 



Recommendation for Windows Services Project
-------------------------------


1. Your Application Service Host (Project/dll)
>> Example: SimpleNet.Sample.ServiceHostApp 
 
  This is the entry point into your application and contains the Program.cs and Main Method

* Include "SimpleNet.ServiceHost" from Nuget, this will include the following packages
  * SimpleNet.ServiceHost
  * SimpleNet.ServiceHost.Contracts
  * Microsoft.Practices.ServiceLocation 
  
* Chose your Dependency Injection Framework (Unity, Mef, etc...) and 
* Register the adapter for [Microsoft.Practices.ServiceLocation.ServiceLocatior]
```
    var locator = new ...
    ServiceLocator.SetLocatorProvider(() => locator);
```

* Add a Helper Class that extends "SimpleNet.ServiceHost.Helpers.AbstractServiceHostHelper" 
    * Specify the HostName and inject the ServiceLocator
    
* Add a Windows Service and "Copy" the code from "ServicesContainer.cs" in the sample project
   



2. Your Services Contract Project (dll) 
>> Example: SimpleNet.TraceBroadcastService.Contracts
>> Example: SimpleNet.Sample.Contracts


Keep your services contract outside of your "services implementation"
This is important as you will be re-using your contracts project/dll
To invoke your services

* Include the "SimpleNet.ServiceHost.Contracts" using Nuget
* Create your ServiceContract and Operation Contracts in this project
* Create your ServiceAddress definition
  * This is used by the Service Proxy and the Service Definition Provider
* Provide a "ServiceProxy" extending "WcfProxy<T>" for each of your ServiceContracts
* 


3. Your Services Implementation Project (dll)

>> Example: SimpleNet.TraceBroadcastService
>> Example: SimpleNet.Sample.Impl 

* This project implement your services, you will write it exactly the way you would write your services normally
* Please implement the IServiceDefinitionProvider (example: ServiceDefinitionProvider)
  * The service host will request for all classes that implement "IServiceDefinitionProvider" 
    * The system will wireup and start services based on the implementation in "IServiceDefinitionProvider"




Enjoy
- Hem

