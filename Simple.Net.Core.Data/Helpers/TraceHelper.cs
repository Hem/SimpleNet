using System;
using System.Diagnostics;

namespace SimpleNet.Data.Helpers
{
    public static class TraceHelper
    {
        public static void TraceException(this Exception ex)
        {
            Trace.TraceError("------------------------------------------------------");

            WriteErrorTrace(ex, 1);

            Trace.TraceError("------------------------------------------------------");
        }

        static void WriteErrorTrace(Exception ex, int level)
        {
            if (level == 5) return;

            Trace.Indent();

            Trace.TraceError(ex.Message);
            Trace.TraceError(ex.Source);

            foreach (var key in ex.Data.Keys)
            {
                Trace.TraceError(string.Format("{0} = {1}", key, ex.Data[key]));
            }

            if (ex.InnerException != null)
            {
                Trace.TraceError("-INNER EXCEPTION-");

                WriteErrorTrace(ex.InnerException, ++level);

                Trace.TraceError("-END INNER EXCEPTION-");
            }

            Trace.Unindent();
        }

    }

}
