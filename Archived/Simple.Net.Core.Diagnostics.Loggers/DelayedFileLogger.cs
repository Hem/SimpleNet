using System;
using System.IO;
using SimpleNet.Diagnostics.Contracts;

namespace SimpleNet.Diagnostics.Loggers
{
    public class DelayedFileLogger : AbstractDelayedLogWritter
    {
        protected readonly Int64 ArchiveSizeBytes;
        protected readonly String LogFileName;
        protected readonly Object _lock = new object();

        private const long ONE_KB = 1024;
        private const long ONE_MB = ONE_KB * 1024;


        public DelayedFileLogger(
                    String filename,
                    int archiveSizeMb,
                    Logger logger, 
                    int seconds = 2)
            : base(logger, seconds)
        {
            ArchiveSizeBytes = archiveSizeMb*ONE_MB;
            LogFileName = filename;
        }

        public override void PreWriteLog()
        {
            lock (_lock)
            {
                var info = new FileInfo(LogFileName);

                if (info.Exists && info.Length >= ArchiveSizeBytes)
                {
                    info.MoveTo(LogFileName + "." + DateTime.Now.ToString("yyMMdd_HHmmsstt"));
                }
            }
        }

        public override void WriteLog(LoggerLogLevel level, DateTime logDateTime, string logText)
        {
            lock (_lock)
            {
                var contents = $"{level}: {logDateTime:d} {logDateTime:T}: {logText} \n";
                File.AppendAllText(LogFileName, contents);
            }
        }

        public override void PostWriteLog()
        {
        }



    }
}
