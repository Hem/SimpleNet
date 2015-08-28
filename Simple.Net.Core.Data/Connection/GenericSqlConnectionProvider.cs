using System;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace Simple.Net.Core.Data.Connection
{
    public class GenericSqlConnectionProvider : IDatabaseConnection
    {
        public string ConnectionString { get; }
        public string ProviderName { get; }
        
        private DbProviderFactory Factory { get; }
        


        public GenericSqlConnectionProvider(string connectionName)
        {
            // get the connection based on connection name
            var connection = ConfigurationManager.ConnectionStrings[connectionName];

            if (connection == null)
                throw new Exception(string.Format("Connection {0} not found", connectionName));


            // Connection string
            ConnectionString = connection.ConnectionString;

            // Provider Name
            ProviderName = connection.ProviderName;

            // Provider Factory
            Factory = DbProviderFactories.GetFactory(ProviderName);
        }


        public DbConnection GetNewConnection()
        {
            var conn = Factory.CreateConnection();

            if (conn != null)
            {
                conn.ConnectionString = ConnectionString;

                if (conn.State == ConnectionState.Closed) conn.Open();

                return conn;
            }


            return null;

        }


        public DbCommand GetNewCommand(DbConnection connection)
        {
            var command = Factory.CreateCommand();

            if (command != null)
            {
                command.Connection = connection;

                return command;
            }

            return null;
        }

        public DbParameter GetSqlParameter(string name, object value)
        {
            var param = Factory.CreateParameter();

            if (param != null)
            {
                param.ParameterName = name;
                param.Value = value;
            }

            return param;
        }

        public DbParameter GetSqlParameter(string name, object value, DbType dbType)
        {
            var param = GetSqlParameter(name, value);

            if (param != null)
            {
                param.DbType = dbType;
            }

            return param;
        }


        public DbParameter GetSqlParameter(string name, object value, DbType dbType, ParameterDirection direction)
        {
            var param = GetSqlParameter(name, value);

            if (param != null)
            {
                param.DbType = dbType;
                param.Direction = direction;
            }

            return param;
        }


        public DbParameter GetSqlParameter(string name, object value, ParameterDirection direction)
        {
            var param = GetSqlParameter(name, value);

            if (param != null)
            {
                param.Direction = direction;
            }

            return param;
        }

    }

}
