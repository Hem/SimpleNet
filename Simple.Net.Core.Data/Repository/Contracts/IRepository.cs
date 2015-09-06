using System;

namespace SimpleNet.Data.Repository.Contracts
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
