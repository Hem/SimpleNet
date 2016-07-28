using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleNet.Data.Repository
{
    public abstract class AbstractInMemoryRepository<T> where T : class , new()
    {

        // ReSharper disable InconsistentNaming
        protected readonly Object _Lock = new object();
        protected readonly List<T> _Records = new List<T>();
        // ReSharper restore InconsistentNaming

        public virtual T Insert(T dto)
        {
            lock (_Lock)
            {
                if (!_Records.Any(x => AreEqual(x, dto)))
                {
                    _Records.Add(dto);
                }
            }


            return dto;
        }

        public virtual T Update(T dto)
        {
            lock (_Lock)
            {
                var record = _Records.FirstOrDefault(x => AreEqual(x, dto));
                if (record != null)
                    _Records.Remove(record);
                _Records.Add(dto);
            }


            return dto;
        }


        public virtual T Delete(T dto)
        {
            lock (_Lock)
            {
                var record = _Records.FirstOrDefault(x => AreEqual(x, dto));
                if (record != null)
                    _Records.Remove(record);

                return record;
            }
        }

        public IEnumerable<T> GetAll()
        {
            lock (_Lock)
            {
                return _Records.ToArray();
            }
        }


        public int RecordCount()
        {
            lock (_Lock)
            {
                return _Records.Count;
            }
        }

        public abstract bool AreEqual(T item, T item2);
    }
}
