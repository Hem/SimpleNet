using System.Data.Common;

namespace SimpleNet.Data.Connection
{
    public interface ISimpleSqlConnectionProvider : ISimpleDbParameterProvider
    {
        string ConnectionString { get; }
        string ProviderName { get; }

        DbConnection GetConnection();

        DbCommand CreateDbCommand(DbConnection connection);
    }
}
