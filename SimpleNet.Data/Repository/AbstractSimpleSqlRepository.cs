using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using SimpleNet.Data.Mapper;
using SimpleNet.Data.Repository.Contracts;

namespace SimpleNet.Data.Repository
{
    public abstract class AbstractSimpleSqlRepository
    {
        public  abstract ISimpleDataAccess Database { get; set; }


        protected IEnumerable<T> Read<T>(IRowMapper<T> mapper, string commandText, CommandType commandType, DbParameter[] parameters)
        {
            return Database.Read(mapper, commandText, commandType, parameters);
        }

        protected Task<IEnumerable<T>> ReadAsync<T>(IRowMapper<T> mapper, string commandText, CommandType commandType,
            DbParameter[] parameters)
        {
            return Database.ReadAsync(mapper, commandText, commandType, parameters);
        }

        protected int ExecuteNonQuery(string commandText, CommandType commandType, DbParameter[] parameters)
        {
            return Database.ExecuteNonQuery(commandText, commandType, parameters);
        }

        protected object ExecuteScalar(string commandText, CommandType commandType, DbParameter[] parameters)
        {
            return Database.ExecuteScalar(commandText, commandType, parameters);
        }

        protected DataTable Read(string commandText, CommandType commandType, DbParameter[] parameters)
        {
            return Database.Read(commandText, commandType, parameters);
        }

        protected DataTable ReadSql(string commandText, DbParameter[] parameters)
        {
            return Read(commandText, CommandType.Text, parameters);
        }

        protected DataTable ReadProc(string commandText, DbParameter[] parameters)
        {
            return Read(commandText, CommandType.StoredProcedure, parameters);
        }

        protected Task<DataTable> ReadAsync(string commandText, CommandType commandType, DbParameter[] parameters)
        {
            return Database.ReadAsync(commandText, commandType, parameters);
        }

        protected Task<DataTable> ReadProcAsync(string commandText, DbParameter[] parameters)
        {
            return ReadAsync(commandText, CommandType.StoredProcedure, parameters);
        }

        protected Task<DataTable> ReadSqlAsync(string commandText, DbParameter[] parameters)
        {
            return ReadAsync(commandText, CommandType.Text, parameters);
        }

        protected Task<int> ExecuteNonQueryAsync(string commandText, CommandType commandType, DbParameter[] parameters)
        {
            return Database.ExecuteNonQueryAsync(commandText, commandType, parameters);
        }

        protected Task<object> ExecuteScalarAsync(string commandText, CommandType commandType, DbParameter[] parameters)
        {
            return Database.ExecuteScalarAsync(commandText, commandType, parameters);
        }

        protected DbParameter GetDbParameter(string name, object value)
        {
            return Database.GetDbParameter(name, value);
        }

        protected DbParameter GetDbParameter(string name, object value, DbType dbType)
        {
            return Database.GetDbParameter(name, value, dbType);
        }

        protected DbParameter GetDbParameter(string name, object value, DbType dbType, ParameterDirection direction)
        {
            return Database.GetDbParameter(name, value, dbType, direction);
        }

        protected DbParameter GetDbParameter(string name, object value, ParameterDirection direction)
        {
            return Database.GetDbParameter(name, value, direction);
        }
    }
}