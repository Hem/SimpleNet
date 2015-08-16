using System;
using System.Data;
using System.Data.Common;

namespace Simple.Net.Core.Data.Connection
{
    public interface IDatabaseConnection : IDatabaseParameter
    {
        String ConnectionString { get; }
        
        DbConnection GetNewConnection { get; }

    }


    public interface IDatabaseParameter
    {

        DbParameter GetSqlParameter(String name, Object value);

        DbParameter GetSqlParameter(String name, Object value, SqlDbType sqlDbType);

    }



}
