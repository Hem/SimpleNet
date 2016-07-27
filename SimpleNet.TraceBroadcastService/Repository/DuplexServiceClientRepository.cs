using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleNet.TraceBroadcastService.Repository
{
    public class DuplexServiceClientRepository<T>
    {

        protected readonly Dictionary<Guid, T> Records = new Dictionary<Guid, T>();
        

        public void Add(Guid key, T callback)
        {
            lock (Records)
            {
                if (Records.ContainsKey(key))
                    Records.Remove(key);
                Records.Add(key, callback);
            }
        }



        public Guid[] ListKeys()
        {
            lock (Records)
            {
                return Records.Keys.ToArray();
            }
        }

        public T Get(Guid key)
        {
            lock (Records)
            {
                return Records.ContainsKey(key) ? Records[key] : default(T);
            }
        }

        public void Delete(Guid key)
        {
            lock (Records)
            {
                if (Records.ContainsKey(key))
                    Records.Remove(key);
            }
        }

    }
}
