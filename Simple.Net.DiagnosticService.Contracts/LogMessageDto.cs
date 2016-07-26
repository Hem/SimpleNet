using System;

namespace SimpleNet.DiagnosticService.Contracts
{
    public class LogMessageDto
    {
        public string LogLevel { get; set; }
        public DateTime LogDateTime { get; set; }
        public string LogText { get; set; }
        public string ComputerName { get; set; }
        public string ApplicationName { get; set; }
    }
}