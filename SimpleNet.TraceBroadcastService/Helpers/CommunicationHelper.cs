using System.ServiceModel;

namespace SimpleNet.TraceBroadcastService.Helpers
{
    public static class CommunicationHelper
    {
        public static bool IsOpen(this ICommunicationObject callback)
        {
            return callback?.State == CommunicationState.Opened;
        }
    }
}