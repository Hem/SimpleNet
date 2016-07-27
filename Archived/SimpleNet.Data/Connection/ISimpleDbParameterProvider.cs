using System.Data;
using System.Data.Common;

namespace SimpleNet.Data.Connection
{
    public interface ISimpleDbParameterProvider
    {
        DbParameter GetDbParameter(string name, object value);
        DbParameter GetDbParameter(string name, object value, DbType dbType);
        DbParameter GetDbParameter(string name, object value, DbType dbType, ParameterDirection direction);
        DbParameter GetDbParameter(string name, object value, ParameterDirection direction);

    }
}