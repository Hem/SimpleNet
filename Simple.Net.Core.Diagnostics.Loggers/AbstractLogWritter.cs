using Simple.Net.Core.Diagnostics.Contracts;

namespace Simple.Net.Core.Diagnostics.Loggers
{
    public abstract class AbstractLogWritter
    {
        protected Logger Logger { get; set; }

        protected LoggerLogLevel MinLogLevel { get; set; }


        protected AbstractLogWritter(Logger logger)
        {
            Logger = logger;

            Logger.MessageBroadcast -= OnLogBroadcast;
            Logger.MessageBroadcast += OnLogBroadcast;
        }


        private void OnLogBroadcast(LoggerLogLevel level, string logtext)
        {
            if ((int)MinLogLevel <= (int)level)
            {
                WriteLog(level, logtext);
            }
        }

        public abstract void WriteLog(LoggerLogLevel level, string logText);
    }
}
