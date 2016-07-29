using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using SimpleNet.ServiceHost.Contracts;

namespace SimpleNet.Sample.Impl
{

    [Export(NAME, typeof(ISelfContainedHost))]

    public class BackgroundTaskService : ISelfContainedHost
    {

        internal const string NAME = "Sample.BackgroundTaskService"; // Unique key please!!!

        public ServiceStatus ServiceStatus { get; private set; }

        Timer _timer;


        public void Start()
        {
            Stop();

            _timer = new Timer(5000);
            _timer.Elapsed += TimerOnElapsed;
            _timer.Start();
            ServiceStatus = ServiceStatus.Started;

        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            Trace.WriteLine($"Background Timer called {DateTime.Now:G}");
        }

        public void Stop()
        {

            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
                _timer = null;
            }

            ServiceStatus = ServiceStatus.Stopped;

        }

        public void Pause()
        {

            Stop();

            ServiceStatus = ServiceStatus.Paused;
        }
    }
}
