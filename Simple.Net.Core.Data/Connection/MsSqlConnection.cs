using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Simple.Net.Core.Data.Connection
{
    public class MsSqlConnection : BaseDatabaseConnection, IDatabaseConnection
    {
        public MsSqlConnection(string connectionString) : base(connectionString)
        {
        }

        public DbConnection GetNewConnection
        {
            get
            {
                 var conn = new SqlConnection(ConnectionString);

                if(conn.State == ConnectionState.Closed )
                    conn.Open();

                return conn;
            }
        }
        
        /// <summary>
        /// Helper method to retreive the SqlParameter
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DbParameter GetSqlParameter(String name, Object value)
        {
            return new SqlParameter(name, value ?? DBNull.Value);
        }

        /// <summary>
        /// Helper method to retreive the SqlParameter
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="sqlDbType"></param>
        /// <returns></returns>
        public DbParameter GetSqlParameter(String name, Object value, SqlDbType sqlDbType)
        {
            return new SqlParameter(name, value ?? DBNull.Value) { SqlDbType = sqlDbType };
        }
        
    }
}
