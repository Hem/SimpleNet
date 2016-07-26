using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Reflection;
using SimpleNet.Diagnostics;
using SimpleNet.Diagnostics.Contracts;
using SimpleNet.DiagnosticService.Contracts;

namespace SimpleNet.DiagnosticService.Business
{
    [Export]
    public class ConnectedDiagnosticClientCallbacks : DuplexServiceClientRepository<IDiagnosticsServiceCallback>, IPartImportsSatisfiedNotification, IDisposable
    {

        [Import]
        public Logger Logger { get; set; }


        public LoggerLogLevel MinLogLevel { get; set; }


        protected readonly Dictionary<Guid, LoggerLogLevel> ConnectedClientLogLevels = new Dictionary<Guid, LoggerLogLevel>();


        #region Callback Framework
        public void AddCallback(Guid key, LoggerLogLevel level, IDiagnosticsServiceCallback callback)
        {

            lock (ConnectedClientLogLevels)
            {
                if (ConnectedClientLogLevels.ContainsKey(key))
                    ConnectedClientLogLevels.Remove(key);

                ConnectedClientLogLevels.Add(key, level);


                AddCallback(key, callback);
            }
        }
        public override void DeleteKey(Guid key)
        {
            lock (ConnectedClientLogLevels)
            {
                if (ConnectedClientLogLevels.ContainsKey(key))
                    ConnectedClientLogLevels.Remove(key);

                base.DeleteKey(key);
            }
        }
        #endregion


        #region Framework Wiring and unwiring
        public void OnImportsSatisfied()
        {
            Logger.MessageBroadcast += BroadcasterOnLogBroadcast;
        }

        public void Dispose()
        {
            Logger.MessageBroadcast -= BroadcasterOnLogBroadcast;
        }
        #endregion

        // Broadcast Log Messages
        private void BroadcasterOnLogBroadcast(LoggerLogLevel level, string logText)
        {
            var keys = ListAllKeys();


            foreach (var key in keys)
            {
                LoggerLogLevel minLevel;

                lock (ConnectedClientLogLevels)
                {
                    if (!ConnectedClientLogLevels.ContainsKey(key))
                        continue;
                    minLevel = ConnectedClientLogLevels[key];
                }



                // incoming log is the level was want
                if ((int) minLevel > (int) level) continue;


                // ok send message through channel!
                var callback = GetCallback(key);

                if (!IsOpen(callback))
                {
                    DeleteKey(key);
                    continue;
                }


                try
                {
                    // callback...
                    callback.ReceiveLogMessage(new LogMessageDto
                    {
                        ComputerName = Environment.MachineName,
                        LogDateTime = DateTime.Now,
                        LogLevel = level.ToString(),
                        LogText = logText,
                        ApplicationName = Assembly.GetEntryAssembly().FullName
                    });
                }
                catch (Exception)
                {
                    DeleteKey(key);
                }
            }
        }

    }
}
