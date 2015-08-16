﻿using System;
using Simple.Net.Core.Diagnostics.Contracts;

namespace Simple.Net.Core.Diagnostics
{

    public delegate void LogMessage(LoggerLogLevel level, String logText);
    
    
    public class Logger
    {

        public event LogMessage MessageBroadcast;
        protected virtual void BroadcastMessage(LoggerLogLevel level, string logtext)
        {
            var handler = MessageBroadcast;
            if (handler != null) handler(level, logtext);
        }

        public void Debug(string logText)
        {
            BroadcastMessage(LoggerLogLevel.Debug, logText);
        }

        public void Critical(string logText)
        {
            BroadcastMessage(LoggerLogLevel.Critical, logText);
        }

        public void Error(string logText, Exception ex)
        {
            BroadcastMessage(LoggerLogLevel.Error, logText);
        }

        public void Error(Exception ex)
        {
            if (ex != null)
            {
                BroadcastMessage(LoggerLogLevel.Error, ex.Message);

                // TODO: Send detailed error!

                if (ex.InnerException != null)
                    Error(ex.InnerException);
            }
        }

        public void Warning(string logText)
        {
            BroadcastMessage(LoggerLogLevel.Warning, logText);
        }

        public void Info(string logText)
        {
            BroadcastMessage(LoggerLogLevel.Info, logText);
        }


    }
}
