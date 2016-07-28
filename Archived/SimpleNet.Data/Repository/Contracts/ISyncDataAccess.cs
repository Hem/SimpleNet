using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using SimpleNet.Data.Mapper;

namespace SimpleNet.Data.Repository.Contracts
{
    public interface ISyncDataAccess
    {
        int ExecuteNonQuery(string commandText, CommandType commandType, DbParameter[] parameters);
        object ExecuteScalar(string commandText, CommandType commandType, DbParameter[] parameters);
        DataTable Read(string commandText, CommandType commandType, DbParameter[] parameters);

        DataTable ReadSql(string commandText, DbParameter[] parameters);
        DataTable ReadProc(string commandText, DbParameter[] parameters);
    }



    public interface ISyncDataAccessor
    {
        IEnumerable<T> Read<T>(IRowMapper<T> mapper, string commandText, CommandType commandType, DbParameter[] parameters);
    }
}