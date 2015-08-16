using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Reflection;
using Simple.Net.Core.Diagnostics;
using Simple.Net.Core.Diagnostics.Contracts;
using Simple.Net.DiagnosticService.Contracts;

namespace Simple.Net.DiagnosticService.Business
{
    [Export]
    public class ConnectedDiagnosticClientCallbacks : DuplexServiceClientRepository<IDiagnosticsServiceCallback>, IPartImportsSatisfiedNotification , IDisposable
    {

        [Import]
        public Logger Broadcaster { get; set; }
        public LoggerLogLevel MinLogLevel { get; set; }


        protected readonly Dictionary<Guid, LoggerLogLevel> _ConnectedClientLogLevels = new Dictionary<Guid, LoggerLogLevel>();


        #region Callback Framework
        public void AddCallback(Guid key, LoggerLogLevel level, IDiagnosticsServiceCallback callback)
        {
            
            lock (_ConnectedClientLogLevels)
            {
                if( _ConnectedClientLogLevels.ContainsKey( key ) )
                    _ConnectedClientLogLevels.Remove( key );

                _ConnectedClientLogLevels.Add( key, level );


                AddCallback(key, callback);
            }
        }
        public override void DeleteKey( Guid key )
        {
            lock (_ConnectedClientLogLevels)
            {
                if (_ConnectedClientLogLevels.ContainsKey(key))
                    _ConnectedClientLogLevels.Remove(key);

                base.DeleteKey(key);
            }
        }
        #endregion


        #region Framework Wiring and unwiring
        public void OnImportsSatisfied()
        {
            Broadcaster.MessageBroadcast += BroadcasterOnLogBroadcast;
        }

        public void Dispose()
        {
            Broadcaster.MessageBroadcast -= BroadcasterOnLogBroadcast;
        }
        #endregion

        // Broadcast Log Messages
        private void BroadcasterOnLogBroadcast(LoggerLogLevel level, string logText)
        {
            var keys = ListAllKeys();

            foreach( var key in keys )
            {
                if(!_ConnectedClientLogLevels.ContainsKey( key )) 
                    continue;

                var minLevel = _ConnectedClientLogLevels[ key ];

                // incoming log is the level was want
                if( (int)minLevel <= (int)level)
                {
                    var callback = GetCallback(key);

                    if (!IsOpen(callback))
                    {
                        DeleteKey(key);
                        continue;
                    }


                    try
                    {
                        // callback...
                        callback.ReceiveLogMessage( new LogMessageDto
                        {
                            ComputerName = Environment.MachineName,
                            LogDateTime = DateTime.Now,
                            LogLevel = level.ToString(),
                            LogText = logText,
                            ApplicationName = Assembly.GetEntryAssembly().FullName
                        } );
                    }
                    catch (Exception)
                    {
                        DeleteKey(key);
                    }

                }

            }
        }

    }
}
