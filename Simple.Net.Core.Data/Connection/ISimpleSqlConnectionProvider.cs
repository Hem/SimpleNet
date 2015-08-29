using System.Data.Common;

namespace Simple.Net.Core.Data.Connection
{
    public interface ISimpleSqlConnectionProvider : ISimpleDbParameterProvider
    {
        string ConnectionString { get; }
        string ProviderName { get; }

        DbConnection GetNewConnection();
        DbCommand GetNewCommand(DbConnection connection);

    }
}
