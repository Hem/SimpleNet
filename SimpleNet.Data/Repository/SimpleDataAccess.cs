using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using SimpleNet.Data.Connection;
using SimpleNet.Data.Helpers;
using SimpleNet.Data.Mapper;
using SimpleNet.Data.Repository.Contracts;

namespace SimpleNet.Data.Repository
{
    public class SimpleDataAccess : ISimpleDataAccess
    {
        /// <summary>
        /// override this in your Sql Repository Implementation.
        /// </summary>
        protected ISimpleSqlConnectionProvider SqlConnectionInfo { get;  }


        #region constructor
        public SimpleDataAccess(string connectionName) : this(new SimpleSqlConnectionProvider(connectionName))
        {
        }
        public SimpleDataAccess(ISimpleSqlConnectionProvider connection)
        {
            SqlConnectionInfo = connection;
        }
        #endregion


        #region ISyncDataAccess Layer

        public DataTable ReadSql(string commandText, DbParameter[] parameters)
        {
            return Read(commandText, CommandType.Text, parameters);
        }

        public DataTable ReadProc(string commandText, DbParameter[] parameters)
        {
            return Read(commandText, CommandType.StoredProcedure, parameters);
        }


        /// <summary>
        /// Execute a read against the database and returns the dataset.
        /// </summary>
        /// <param name="commandText">The sql or procedure name to execute</param>
        /// <param name="commandType">The command type</param>
        /// <param name="parameters">An array || a list of parameters to pass to the command</param>
        /// <returns>The records populated into a dataset/data table.</returns>
        public DataTable Read(string commandText, CommandType commandType, DbParameter[] parameters)
        {
            var dataTable = new DataTable();

            try
            {
                using (var connection = SqlConnectionInfo.GetConnection())
                {

                    using (var command = SqlConnectionInfo.CreateDbCommand(connection))
                    {
                        command.CommandText = commandText;
                        command.CommandType = commandType;

                        // Add parameters
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters.ToArray());
                        }

                        var dr = command.ExecuteReader();

                        dataTable.Load(dr);

                        command.Parameters.Clear();
                    }

                }

            }
            catch (Exception ex)
            {
                ex.Data.Add("CommandText", commandText);
                ex.Data.Add("CommandType", commandType.ToString());

                if (parameters != null)
                {
                    if (commandType == CommandType.StoredProcedure)
                        ex.Data.Add("SqlCommand", parameters.FormatToExecuteProcedure(commandText));

                    foreach (var parameter in parameters)
                    {
                        ex.Data.Add(parameter.ParameterName, parameter.Value);
                    }
                }

                ex.TraceException();

                throw;
            }

            return dataTable;
        }



        /// <summary>
        /// Executes a command against the dataset and returns the first value received.
        /// </summary>
        /// <param name="commandText">The sql or procedure name to execute</param>
        /// <param name="commandType">The command type</param>
        /// <param name="parameters">An array || a list of parameters to pass to the command</param>
        /// <returns>The first value returned</returns>
        public object ExecuteScalar(string commandText, CommandType commandType, DbParameter[] parameters)
        {
            object value;

            try
            {

                using (var connection = SqlConnectionInfo.GetConnection())
                {

                    using (var command = SqlConnectionInfo.CreateDbCommand(connection))
                    {
                        command.CommandText = commandText;
                        command.CommandType = commandType;

                        // Add parameters
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters.ToArray());
                        }

                        value = command.ExecuteScalar();

                        command.Parameters.Clear();
                    }

                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("CommandText", commandText);
                ex.Data.Add("CommandType", commandType.ToString());

                if (parameters != null)
                {
                    if (commandType == CommandType.StoredProcedure)
                        ex.Data.Add("SqlCommand", parameters.FormatToExecuteProcedure(commandText));

                    foreach (var parameter in parameters)
                    {
                        ex.Data.Add(parameter.ParameterName, parameter.Value);
                    }
                }

                ex.TraceException();
                throw;
            }

            return value;
        }


        /// <summary>
        /// Use this to execute a command against the database when a response is NOT needed 
        /// or we only need the count of number of records affected
        /// </summary>
        /// <param name="commandText">The sql or procedure name to execute</param>
        /// <param name="commandType">The command type</param>
        /// <param name="parameters">An array || a list of parameters to pass to the command</param>
        /// <returns>The count of number of records affected.</returns>
        public int ExecuteNonQuery(string commandText, CommandType commandType, DbParameter[] parameters)
        {
            int value;

            try
            {

                using (var connection = SqlConnectionInfo.GetConnection())
                {

                    using (var command = SqlConnectionInfo.CreateDbCommand(connection))
                    {
                        command.CommandText = commandText;
                        command.CommandType = commandType;

                        // Add parameters
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters.ToArray());
                        }

                        value = command.ExecuteNonQuery();

                        command.Parameters.Clear();
                    }

                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("CommandText", commandText);
                ex.Data.Add("CommandType", commandType.ToString());

                if (parameters != null)
                {
                    if (commandType == CommandType.StoredProcedure)
                        ex.Data.Add("SqlCommand", parameters.FormatToExecuteProcedure(commandText));

                    foreach (var parameter in parameters)
                    {
                        ex.Data.Add(parameter.ParameterName, parameter.Value);
                    }
                }


                ex.TraceException();
                throw;
            }

            return value;
        }


        #endregion


        #region Async Data Access

        public async Task<DataTable> ReadSqlAsync(string commandText, DbParameter[] parameters)
        {
            return await ReadAsync(commandText, CommandType.Text, parameters);
        }

        public async Task<DataTable> ReadProcAsync(string commandText, DbParameter[] parameters)
        {
            return await ReadAsync(commandText, CommandType.StoredProcedure, parameters);
        }

        /// <summary>
        /// Execute a read against the database and returns the dataset.
        /// </summary>
        /// <param name="commandText">The sql or procedure name to execute</param>
        /// <param name="commandType">The command type</param>
        /// <param name="parameters">An array || a list of parameters to pass to the command</param>
        /// <returns>The records populated into a dataset/data table.</returns>
        public async Task<DataTable> ReadAsync(string commandText, CommandType commandType, DbParameter[] parameters)
        {
            var dataTable = new DataTable();

            try
            {
                using (var connection = SqlConnectionInfo.GetConnection())
                {

                    using (var command = SqlConnectionInfo.CreateDbCommand(connection))
                    {
                        command.CommandText = commandText;
                        command.CommandType = commandType;

                        // Add parameters
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters.ToArray());
                        }

                        var dr = await command.ExecuteReaderAsync();

                        dataTable.Load(dr);

                        command.Parameters.Clear();
                    }

                }

            }
            catch (Exception ex)
            {
                ex.Data.Add("CommandText", commandText);
                ex.Data.Add("CommandType", commandType.ToString());

                if (parameters != null)
                {
                    if (commandType == CommandType.StoredProcedure)
                        ex.Data.Add("SqlCommand", parameters.FormatToExecuteProcedure(commandText));

                    foreach (var parameter in parameters)
                    {
                        ex.Data.Add(parameter.ParameterName, parameter.Value);
                    }
                }

                ex.TraceException();
                throw;
            }

            return dataTable;
        }


        /// <summary>
        /// Executes a command against the dataset and returns the first value received.
        /// </summary>
        /// <param name="commandText">The sql or procedure name to execute</param>
        /// <param name="commandType">The command type</param>
        /// <param name="parameters">An array || a list of parameters to pass to the command</param>
        /// <returns>The first value returned</returns>
        public async Task<object> ExecuteScalarAsync(string commandText, CommandType commandType, DbParameter[] parameters)
        {
            object value;

            try
            {
                using (var connection = SqlConnectionInfo.GetConnection())
                {


                    using (var command = SqlConnectionInfo.CreateDbCommand(connection))
                    {
                        command.CommandText = commandText;
                        command.CommandType = commandType;

                        // Add parameters
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters.ToArray());
                        }

                        value = await command.ExecuteScalarAsync();

                        command.Parameters.Clear();
                    }

                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("CommandText", commandText);
                ex.Data.Add("CommandType", commandType.ToString());

                if (parameters != null)
                {
                    if (commandType == CommandType.StoredProcedure)
                        ex.Data.Add("SqlCommand", parameters.FormatToExecuteProcedure(commandText));

                    foreach (var parameter in parameters)
                    {
                        ex.Data.Add(parameter.ParameterName, parameter.Value);
                    }
                }

                ex.TraceException();
                throw;
            }

            return value;
        }


        /// <summary>
        /// Use this to execute a command against the database when a response is NOT needed 
        /// or we only need the count of number of records affected
        /// </summary>
        /// <param name="commandText">The sql or procedure name to execute</param>
        /// <param name="commandType">The command type</param>
        /// <param name="parameters">An array || a list of parameters to pass to the command</param>
        /// <returns>The count of number of records affected.</returns>
        public async Task<int> ExecuteNonQueryAsync(string commandText, CommandType commandType, DbParameter[] parameters)
        {
            int value;

            try
            {

                using (var connection = SqlConnectionInfo.GetConnection())
                {

                    using (var command = SqlConnectionInfo.CreateDbCommand(connection))
                    {
                        command.CommandText = commandText;
                        command.CommandType = commandType;

                        // Add parameters
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters.ToArray());
                        }

                        value = await command.ExecuteNonQueryAsync();

                        command.Parameters.Clear();
                    }

                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("CommandText", commandText);
                ex.Data.Add("CommandType", commandType.ToString());

                if (parameters != null)
                {
                    if (commandType == CommandType.StoredProcedure)
                        ex.Data.Add("SqlCommand", parameters.FormatToExecuteProcedure(commandText));

                    foreach (var parameter in parameters)
                    {
                        ex.Data.Add(parameter.ParameterName, parameter.Value);
                    }
                }

                ex.TraceException();
                throw;
            }

            return value;
        }


        #endregion

        public IEnumerable<T> Read<T>(IRowMapper<T> mapper, string commandText, CommandType commandType, DbParameter[] parameters)
        {
            using (var connection = SqlConnectionInfo.GetConnection())
            {
                using (var command = SqlConnectionInfo.CreateDbCommand(connection))
                {
                    command.CommandText = commandText;
                    command.CommandType = commandType;
                    // Add parameters
                    if (parameters != null) command.Parameters.AddRange(parameters.ToArray());

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return mapper.MapRow(reader);
                        }
                    }

                    command.Parameters.Clear();
                }
            }
        }

        public async Task< IEnumerable<T> > ReadAsync<T>(IRowMapper<T> mapper, string commandText, CommandType commandType, DbParameter[] parameters)
        {
            var results = new List<T>();

            using (var connection = SqlConnectionInfo.GetConnection())
            {
                using (var command = SqlConnectionInfo.CreateDbCommand(connection))
                {
                    command.CommandText = commandText;
                    command.CommandType = commandType;
                    // Add parameters
                    if (parameters != null) command.Parameters.AddRange(parameters.ToArray());

                    using (IDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            results.Add(mapper.MapRow(reader));
                        }
                    }

                    command.Parameters.Clear();
                }
            }

            return results;
        }


        #region ISimple Db Parameter Provider
        public DbParameter GetDbParameter(string name, object value)
        {
            return SqlConnectionInfo.GetDbParameter(name, value);
        }

        public DbParameter GetDbParameter(string name, object value, DbType dbType)
        {
            return SqlConnectionInfo.GetDbParameter(name, value, dbType);
        }

        public DbParameter GetDbParameter(string name, object value, DbType dbType, ParameterDirection direction)
        {
            return SqlConnectionInfo.GetDbParameter(name, value, dbType, direction);
        }

        public DbParameter GetDbParameter(string name, object value, ParameterDirection direction)
        {
            return SqlConnectionInfo.GetDbParameter(name, value, direction);
        }

        #endregion

        public void Dispose()
        {
        }
    }

}
