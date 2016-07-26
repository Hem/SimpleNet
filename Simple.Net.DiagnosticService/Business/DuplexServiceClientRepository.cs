using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace SimpleNet.DiagnosticService.Business
{
    public class DuplexServiceClientRepository<T>
    {


        protected readonly Dictionary<Guid, T> ConnectedClients = new Dictionary<Guid, T>();


        protected bool IsOpen(T callback)
        {
            // ReSharper disable CompareNonConstrainedGenericWithNull
            if (callback == null)
                return false;
            // ReSharper restore CompareNonConstrainedGenericWithNull

            var comm = (ICommunicationObject)callback;


            return comm.State == CommunicationState.Opened;
        }


        public void AddCallback(Guid key, T callback)
        {
            lock (ConnectedClients)
            {
                if (ConnectedClients.ContainsKey(key))
                    ConnectedClients.Remove(key);
                ConnectedClients.Add(key, callback);
            }
        }



        public Guid[] ListAllKeys()
        {
            lock (ConnectedClients)
            {
                return ConnectedClients.Keys.ToArray();
            }
        }

        protected T GetCallback(Guid key)
        {
            lock (ConnectedClients)
            {
                return ConnectedClients.ContainsKey(key) ? ConnectedClients[key] : default(T);
            }
        }

        public virtual void DeleteKey(Guid key)
        {
            lock (ConnectedClients)
            {
                if (ConnectedClients.ContainsKey(key))
                    ConnectedClients.Remove(key);
            }
        }

    }
}
