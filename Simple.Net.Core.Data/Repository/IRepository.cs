using System;

namespace Simple.Net.Core.Data.Repository
{
    public interface IRepository : IDisposable
    {

    }

    public interface IRepository<T, in TV> : IRepository
    {
        T GetById(TV id);

        T Create(T dto);

        T Update(T dto);

        T Delete(T dto);
    }

  

}
