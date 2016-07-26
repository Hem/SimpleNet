using System;
using System.ComponentModel.Composition;
using System.ServiceProcess;
using Microsoft.Practices.ServiceLocation;
using SimpleNet.Diagnostics;
using SimpleNet.Sample.ServiceHost.Helper;

namespace SimpleNet.Sample.ServiceHost
{
    [Export]
    public partial class ServiceContainer : ServiceBase
    {
        [Import]
        public MefServiceHostHelper HostHelper { get; set; }

        [Import]
        public Logger Logger { get; set; }

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
                Logger.Info(
                    String.Format("Service Host Started:{0} at [{1:MM-dd-yyy hh:mm.ss}] \r\n{2}",
                                HostHelper.HostName,
                                DateTime.Now,
                                string.Join("\r\n", startedList)
                                ));

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

            var message = String.Format("Service Host Stopped:{0} at [{1:MM-dd-yyy hh:mm.ss}] \r\n {2}",
                HostHelper.HostName,
                DateTime.Now,
                string.Join("\r\n", stoppedList)
                );

            Logger.Info(message);
        }



        protected void PrintError(Exception ex)
        {
            var subject = String.Format("Service Host Failed to Started:{0}", HostHelper.HostName);
            Logger.Error(subject, ex);
        }
    }
}
