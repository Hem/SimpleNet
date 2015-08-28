using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Simple.Net.Core.Data.Connection;
using Simple.Net.Core.Data.Helpers;

namespace Simple.Net.Core.Data.Repository
{
    public abstract class AbstractSqlRepository : IRepository
    {
        /// <summary>
        /// override this in your Sql Repository Implementation.
        /// </summary>
        public abstract IDatabaseConnection SqlConnectionInfo { get; set; }

        
        protected async Task<DataTable> ReadSqlAsync(string commandText, DbParameter[] parameters)
        {
            return await ReadAsync(commandText, CommandType.Text, parameters);
        }

        protected async Task<DataTable> ReadProcAsync(string commandText, DbParameter[] parameters)
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
        protected async Task<DataTable> ReadAsync(string commandText, CommandType commandType, DbParameter[] parameters)
        {
            var dataTable = new DataTable();

            try
            {
                using (var connection = SqlConnectionInfo.GetNewConnection())
                {

                    using (var command = SqlConnectionInfo.GetNewCommand(connection))
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
        /// Execute a read against the database and returns the dataset.
        /// </summary>
        /// <param name="commandText">The sql or procedure name to execute</param>
        /// <param name="commandType">The command type</param>
        /// <param name="parameters">An array || a list of parameters to pass to the command</param>
        /// <returns>The records populated into a dataset/data table.</returns>
        protected DataTable Read(string commandText, CommandType commandType, DbParameter[] parameters)
        {
            var dataTable = new DataTable();

            try
            {
                using (var connection = SqlConnectionInfo.GetNewConnection())
                {

                    using (var command = SqlConnectionInfo.GetNewCommand(connection))
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
        protected async Task<object> ExecuteScalarAsync(string commandText, CommandType commandType, DbParameter[] parameters)
        {
            object value;

            try
            {
                using (var connection = SqlConnectionInfo.GetNewConnection())
                {


                    using (var command = SqlConnectionInfo.GetNewCommand(connection))
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
        /// Executes a command against the dataset and returns the first value received.
        /// </summary>
        /// <param name="commandText">The sql or procedure name to execute</param>
        /// <param name="commandType">The command type</param>
        /// <param name="parameters">An array || a list of parameters to pass to the command</param>
        /// <returns>The first value returned</returns>
        protected object ExecuteScalar(string commandText, CommandType commandType, DbParameter[] parameters)
        {
            object value;

            try
            {

                using (var connection = SqlConnectionInfo.GetNewConnection())
                {

                    using (var command = SqlConnectionInfo.GetNewCommand(connection))
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
        protected int ExecuteNonQuery(string commandText, CommandType commandType, DbParameter[] parameters)
        {
            int value;

            try
            {

                using (var connection = SqlConnectionInfo.GetNewConnection())
                {

                    using (var command = SqlConnectionInfo.GetNewCommand(connection))
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



        /// <summary>
        /// Use this to execute a command against the database when a response is NOT needed 
        /// or we only need the count of number of records affected
        /// </summary>
        /// <param name="commandText">The sql or procedure name to execute</param>
        /// <param name="commandType">The command type</param>
        /// <param name="parameters">An array || a list of parameters to pass to the command</param>
        /// <returns>The count of number of records affected.</returns>
        protected async Task<int> ExecuteNonQueryAsync(string commandText, CommandType commandType, DbParameter[] parameters)
        {
            int value;

            try
            {

                using (var connection = SqlConnectionInfo.GetNewConnection())
                {

                    using (var command = SqlConnectionInfo.GetNewCommand(connection))
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




        /// <summary>
        /// Helper method to retreive the SqlParameter
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        protected DbParameter GetSqlParameter(string name, object value)
        {
            return SqlConnectionInfo.GetSqlParameter(name, value);
        }

        protected DbParameter GetSqlParameter(string name, object value, DbType dbType)
        {
            return SqlConnectionInfo.GetSqlParameter(name, value, dbType);
        }

        protected DbParameter GetSqlParameter(string name, object value, DbType dbType, ParameterDirection direction)
        {
            return SqlConnectionInfo.GetSqlParameter(name, value, dbType, direction);
        }

        protected DbParameter GetSqlParameter(string name, object value, ParameterDirection direction)
        {
            return SqlConnectionInfo.GetSqlParameter(name, value, direction);
        }

        public void Dispose()
        {
        }
    }

}
