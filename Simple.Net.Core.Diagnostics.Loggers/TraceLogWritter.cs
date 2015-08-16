using System;
using System.Diagnostics;
using Simple.Net.Core.Diagnostics.Contracts;

namespace Simple.Net.Core.Diagnostics.Loggers
{
    public class TraceLogWritter : AbstractLogWritter
    {
        public TraceLogWritter(Logger logger) : base(logger)
        {
        }

        public override void WriteLog(LoggerLogLevel level, string logText)
        {
            Trace.WriteLine(String.Format("{0}: {1}", level, logText));
        }
    }
}