using System.ComponentModel.Composition;
using System.Diagnostics;

namespace SimpleNet.TraceBroadcastService.TraceListeners
{
    [Export]
    public class BroadcastTraceListener : TraceListener
    {
        const string CRLF = "\r\n";

        [Import]
        public TraceBroadcaster TraceBroadcaster { get; set; }

        public override void Write(string message)
        {
            TraceBroadcaster.BroadcastMessage(message);
        }

        public override void WriteLine(string message)
        {
            TraceBroadcaster.BroadcastMessage(message + CRLF);
        }
        
    }
}
