using System;
using System.Diagnostics;
using SimpleNet.Diagnostics.Contracts;

namespace SimpleNet.Diagnostics.Loggers
{
    public class TraceLogWritter : AbstractLogWritter
    {
        public TraceLogWritter(Logger logger) : base(logger)
        {
        }

        public override void WriteLog(LoggerLogLevel level, string logText)
        {
            Trace.WriteLine($"{level}: {logText}");
        }
    }
}