using System;

namespace SimpleNet.DiagnosticService.Contracts
{
    public class LogMessageDto
    {
        public String LogLevel { get; set; }
        public DateTime LogDateTime { get; set; }
        public String LogText { get; set; }
        public String ComputerName { get; set; }
        public String ApplicationName { get; set; }
    }
}