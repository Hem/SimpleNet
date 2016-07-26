using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace SimpleNet.DiagnosticService.Business
{
    public class DuplexServiceClientRepository<T>
    {
        protected readonly Dictionary<Guid, T> _ConnectedClients = new Dictionary<Guid, T>();


        protected Boolean IsOpen(T callback)
        {
            // ReSharper disable CompareNonConstrainedGenericWithNull
            if (callback == null)
                return false;
            // ReSharper restore CompareNonConstrainedGenericWithNull

            var comm = (ICommunicationObject)callback;
            if (comm.State != CommunicationState.Opened)
                return false;

            return true;
        }


        public void AddCallback(Guid key, T callback)
        {
            lock (_ConnectedClients)
            {
                if (_ConnectedClients.ContainsKey(key))
                    _ConnectedClients.Remove(key);
                _ConnectedClients.Add(key, callback);
            }
        }



        public Guid[] ListAllKeys()
        {
            lock (_ConnectedClients)
            {
                return _ConnectedClients.Keys.ToArray();
            }
        }

        protected T GetCallback(Guid key)
        {
            lock (_ConnectedClients)
            {
                return _ConnectedClients.ContainsKey(key) ? _ConnectedClients[key] : default(T);
            }
        }

        public virtual void DeleteKey(Guid key)
        {
            lock (_ConnectedClients)
            {
                if (_ConnectedClients.ContainsKey(key))
                    _ConnectedClients.Remove(key);
            }
        }

    }
}
