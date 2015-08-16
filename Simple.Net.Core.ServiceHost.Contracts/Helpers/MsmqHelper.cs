using System;
using System.Messaging;

namespace Simple.Net.Core.ServiceHost.Contracts.Helpers
{
    public static class MsmqHelper
    {

        public static void EnsureQueueExists(string qName, bool transactional = false)
        {
            var path = qName.ToLower().Contains(@"/private/") ? qName.ToLower().Replace(@"/private/", @".\Private$\") : @".\Private$\" + qName;

            // if the private machine queue does not exist, create it
            if (MessageQueue.Exists(path)) return;

            // create the queue and set its transactional status
            var mq = MessageQueue.Create(path, transactional);
            mq.Label = qName;
            mq.SetPermissions("Everyone", MessageQueueAccessRights.FullControl, AccessControlEntryType.Allow);
        }



        public static bool DeleteQueue(string qName)
        {
            var path = qName.ToLower().Contains(@"/private/") ? qName.ToLower().Replace(@"/private/", @".\Private$\") : @".\Private$\" + qName;
            
            // if the private machine queue exists, delete it
            if (!MessageQueue.Exists(path)) return true;

            // create the queue and set its transactional status
            try
            {
                MessageQueue.Delete(path);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }


    }
}
