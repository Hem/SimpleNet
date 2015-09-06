using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace SimpleNet.Data.Helpers
{
    public static class SqlCommandHelper
    {

        /// <summary>
        /// Will properly format the command to run in SQL Command Window.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="procedureName"></param>
        /// <returns>EXEC @procedureName @param1=@value....</returns>
        public static string FormatToExecuteProcedure(this IEnumerable<DbParameter> parameters, string procedureName)
        {
            // ReSharper disable InconsistentNaming
            const string PARAMETER_FORMAT = " {0} = {1} ";
            const string EXEC_FORMAT = "EXEC {0} {1}";
            // ReSharper restore InconsistentNaming

            var paramText = parameters.Select(parameter => String.Format(PARAMETER_FORMAT, parameter.ParameterName, GetFormattedParameterValue(parameter))).ToArray();

            return String.Format(EXEC_FORMAT, procedureName, String.Join(", \r\n", paramText));

        }



        /// <summary>
        /// returns a fully formatted sql parameter value so we can use it in logging.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>Fully formatted sql parameter value</returns>
        public static Object GetFormattedParameterValue(this DbParameter parameter)
        {
            if (parameter.Value == null || parameter.Value == DBNull.Value) return "null";


            SqlDbType sqlDbType;

            var p1 = parameter as SqlParameter;

            if (p1 != null)
            {
                sqlDbType = p1.SqlDbType;
            }
            else
            {
                sqlDbType = SqlDbType.Text;
            }



            switch (sqlDbType)
            {
                case SqlDbType.Int:
                case SqlDbType.Money:
                    return parameter.Value;
                case SqlDbType.Bit:
                    return Convert.ToBoolean(parameter.Value) ? 1 : 0;
                case SqlDbType.Date:
                    return $" '{Convert.ToDateTime(parameter.Value).ToString("d")}' ";
                case SqlDbType.DateTime:
                    return $" '{Convert.ToDateTime(parameter.Value).ToString("g")}' ";
                default:
                    return $" '{parameter.Value}' ";
            }
        }

    }

}
