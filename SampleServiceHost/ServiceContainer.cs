using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.ServiceProcess;
using Microsoft.Practices.ServiceLocation;
using SimpleNet.Sample.ServiceHostApp.Helper;

namespace SimpleNet.Sample.ServiceHostApp
{
    [Export]
    public partial class ServiceContainer : ServiceBase
    {
        [Import]
        public MefServiceHostHelper HostHelper { get; set; }
        

        [Import]
        public IServiceLocator ServiceLocator { get; set; }


        public ServiceContainer()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                var startedList = HostHelper.OnStart(args);

                if (startedList.Count == 0) throw new Exception("No services were started");

                // TODO: Send email on start up of application
                Trace.TraceInformation(
                    $"Service Host Started:{HostHelper.HostName} at [{DateTime.Now:MM-dd-yyy hh:mm.ss}] \r\n{string.Join("\r\n", startedList)}");

            }
            catch (Exception ex)
            {
                PrintError(ex);
                throw;
            }

        }

        protected override void OnStop()
        {
            var stoppedList = HostHelper.OnStop();

            var message =
                $"Service Host Stopped:{HostHelper.HostName} at [{DateTime.Now:MM-dd-yyy hh:mm.ss}] \r\n {string.Join("\r\n", stoppedList)}";

            Trace.TraceInformation(message);
        }



        protected void PrintError(Exception ex)
        {
            Trace.TraceError($"Service Host Failed to Started:{HostHelper.HostName}");
            Trace.TraceError(ex.Message);

            if(ex.InnerException != null)
                PrintError(ex.InnerException);
        }
    }
}
