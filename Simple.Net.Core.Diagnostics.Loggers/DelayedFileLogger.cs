using System;
using System.IO;
using Simple.Net.Core.Diagnostics.Contracts;

namespace Simple.Net.Core.Diagnostics.Loggers
{
    public class DelayedFileLogger : AbstractDelayedLogWritter
    {
        protected readonly Int64 ArchiveSizeBytes;
        protected readonly String LogFileName;
        protected readonly Object _lock = new object();

        private const long OneKb = 1024;
        private const long OneMb = OneKb * 1024;


        public DelayedFileLogger(
                    String filename,
                    int archiveSizeMb,
                    Logger logger, 
                    int seconds = 2)
            : base(logger, seconds)
        {
            ArchiveSizeBytes = archiveSizeMb*OneMb;
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
                var contents = String.Format("{0}: {1:d} {1:T}: {2} \n", level, logDateTime, logText);
                File.AppendAllText(LogFileName, contents);
            }
        }

        public override void PostWriteLog()
        {
        }



    }
}
