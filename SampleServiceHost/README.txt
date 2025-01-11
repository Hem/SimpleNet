HTTP could not register URL http://+:80/SampleService.svc/. 
Your process does not have access rights to this namespace 
(see http://go.microsoft.com/fwlink/?LinkId=70353 for details).

netsh http add urlacl url=http://+:80/ user=EVERYONE



The service endpoint failed to listen on the URI 'net.tcp://localhost:6000/DiagnosticService.svc' 
because access was denied.  Verify that the current user is granted access in the appropriate allowAccounts 
section of SMSvcHost.exe.config.


The TransportManager failed to listen on the supplied URI using the NetTcpPortSharing service: 
failed to start the service because it is disabled. 
An administrator can enable it by running 'sc.exe config NetTcpPortSharing start= demand'..




Windows 11.. Start Net TCP Port Sharing

https://learn.microsoft.com/en-us/dotnet/framework/wcf/feature-details/how-to-enable-the-net-tcp-port-sharing-service