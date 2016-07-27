sTo enable the diagnostics service please do the following.

1. Add reference to Project
	- CCS.Common.Diagnostics
	- CCS.Common.Diagnostics.Service
	- CCS.Common.Diagnostics.Service.Contracts

2. Add to your config file (please update the tcp port and service url)
	<add key="DIAGNOSTICS_SERVICE_ADDRESS" value="net.tcp://localhost:6000/DiagnosticService.svc" />

	
	// C:\>netsh http add urlacl url=http://+:6000/ user=Everyone
	// C:\>sc.exe config NetTcpPortSharing start= demand


3. Following code is needed to be added to your CompositionContainer
	
	var batch = new CompositionBatch();
		var logger = new CcsLogger();
		var diagnosticsAddress = ConfigurationManager.AppSettings[ "DIAGNOSTICS_SERVICE_ADDRESS" ];

	batch.AddExportedValue(logger);
	if( !String.IsNullOrEmpty( diagnosticsAddress ) )
		batch.AddExportedValue( DiagnosticsAddressProvider.DIAGNOSTICS_SERVICE_ADDRESS, diagnosticsAddress );



*************** CODE TO CONNECT TO THE DIAGNOSTIC SERVICE	*************** 

		var diagAddressProvider = new DiagnosticsAddressProvider{
									ServiceAddress = ConfigurationManager.AppSettings.Get("DIAGNOSTICS_SERVICE_ADDRESS")                                        
								};

		var diagServiceAddress = diagAddressProvider.GetDiagnosticServiceAddress;

		// You can add this to MEF
		var callback = new DefaultDiagnosticsServicesCallback();
		
		// Using event to rebroadcast this to any connected listeners
		callback.MessageReceived += message =>
			Console.WriteLine("{0}:{1}:{2:d} {2:T}:{3} \n", message.ComputerName, message.LogLevel, message.LogDateTime, message.LogText);


		// You can add this to MEF
		var diagnositicAdapter = new DuplexDiagnosticServiceAdapter(diagServiceAddress, callback);



************ Listening **********
	
	// diagnositicAdapter.OpenChannel();
	diagnositicAdapter.Connect(appId, LogLevel.Debug.ToString(), "correctcaresolutions");


********** STOP Listening *************
	
	diagnositicAdapter.Disconnect(appId);
	diagnositicAdapter.CloseChannel();
