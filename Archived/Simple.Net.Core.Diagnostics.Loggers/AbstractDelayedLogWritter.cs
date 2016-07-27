using System;
using System.Collections.Concurrent;
using System.Timers;
using SimpleNet.Diagnostics.Contracts;

namespace SimpleNet.Diagnostics.Loggers
{
    public abstract class AbstractDelayedLogWritter : AbstractLogWritter
    {

        protected readonly ConcurrentQueue<Tuple<LoggerLogLevel, DateTime, string>> Queue;
        protected readonly Timer Timer;
        
        protected readonly object Lock = new object();
        protected bool WriteInProgress;


        // flush contects on close
        ~AbstractDelayedLogWritter()
        {
            Flush();
        }

        protected AbstractDelayedLogWritter(Logger logger, int seconds = 2) : base(logger)
        {
            Queue = new ConcurrentQueue<Tuple<LoggerLogLevel, DateTime, string>>();


            Timer = new Timer(1000 * seconds);
            Timer.Elapsed += TimerOnElapsed;
            Timer.Enabled = true;
        }



        public sealed override void WriteLog(LoggerLogLevel level, string logText)
        {
            Queue.Enqueue(new Tuple<LoggerLogLevel, DateTime, string>(level, DateTime.Now, logText));
        }




        public abstract void PreWriteLog();
        public abstract void WriteLog(LoggerLogLevel level, DateTime logDateTime, string logText);
        public abstract void PostWriteLog();

        protected void Flush()
        {
            if (WriteInProgress) return;

            if (Queue.Count > 0)
            {
                WriteInProgress = true;

                lock (Lock)
                {
                    PreWriteLog();

                    Tuple<LoggerLogLevel, DateTime, String> log;

                    while (Queue.TryDequeue(out log))
                    {
                        WriteLog( log.Item1, log.Item2, log.Item3 );
                    }

                    PostWriteLog();
                }
                WriteInProgress = false;
            }
        }


        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            Flush();
        }

        public void Dispose()
        {
            Timer.Stop();
            Timer.Dispose();
            Flush();
        }
    }
}