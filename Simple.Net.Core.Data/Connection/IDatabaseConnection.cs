using System.Data;
using System.Data.Common;

namespace Simple.Net.Core.Data.Connection
{
    public interface IDatabaseConnection : IDatabaseParameter
    {
        string ConnectionString { get; }
        string ProviderName { get; }

        DbConnection GetNewConnection();
        DbCommand GetNewCommand(DbConnection connection);

    }


    public interface IDatabaseParameter
    {
        DbParameter GetSqlParameter(string name, object value);
        DbParameter GetSqlParameter(string name, object value, DbType dbType);
        DbParameter GetSqlParameter(string name, object value, DbType dbType, ParameterDirection direction);
        DbParameter GetSqlParameter(string name, object value, ParameterDirection direction);

    }



}
