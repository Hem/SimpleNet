using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using SimpleNet.Data.Mapper;

namespace SimpleNet.Data.Repository.Contracts
{
    public interface IAsyncDataAccessor
    {
        Task<IEnumerable<T>> ReadAsync<T>(IRowMapper<T> mapper, string commandText, CommandType commandType, DbParameter[] parameters);
    }
}