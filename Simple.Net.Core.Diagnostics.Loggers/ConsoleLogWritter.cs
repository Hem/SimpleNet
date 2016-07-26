using System;
using SimpleNet.Diagnostics.Contracts;

namespace SimpleNet.Diagnostics.Loggers
{
    public class ConsoleLogWritter : AbstractLogWritter
    {
        public ConsoleLogWritter(Logger logger) : base(logger)
        {
        }

        public override void WriteLog(LoggerLogLevel level, string logText)
        {
            Console.WriteLine($"{level}: {logText}");
        }
    }
}
