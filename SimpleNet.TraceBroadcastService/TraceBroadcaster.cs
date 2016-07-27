using System;
using System.ComponentModel.Composition;
using System.Reflection;
using System.ServiceModel;
using SimpleNet.DiagnosticService.Contracts;
using SimpleNet.TraceBroadcastService.Repository;

namespace SimpleNet.TraceBroadcastService
{
    [Export]
    public class TraceBroadcaster
    {
        [Import]
        public DiagnosticCallBackRepository CallBackRepository { get; set; }

        public void BroadcastMessage(string message)
        {
            Broadcast("Info", message);
        }

        public void BroadcastMessage(string type, string message)
        {
            Broadcast(type, message);
        }



        protected void Broadcast(string type, string logText)
        {
            var keys = CallBackRepository.ListKeys();


            foreach (var key in keys)
            {   
                // ok send message through channel!
                var callback = CallBackRepository.Get(key);

                if (!(callback as ICommunicationObject).IsOpen())
                {
                    CallBackRepository.Delete(key);
                    continue;
                }


                try
                {
                    // callback...
                    callback.ReceiveLogMessage(new LogMessageDto
                    {
                        ComputerName = Environment.MachineName,
                        LogDateTime = DateTime.Now,
                        LogLevel = type,
                        LogText = logText,
                        ApplicationName = Assembly.GetEntryAssembly().FullName
                    });
                }
                catch (Exception)
                {
                    CallBackRepository.Delete(key);
                }
            }
        }

    }
}
