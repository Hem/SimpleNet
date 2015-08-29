using Simple.Net.Core.Data.Connection;

namespace Simple.Net.Core.Data.Repository.Contracts
{
    public interface ISimpleDataAccess : ISyncDataAccess, IAsyncDataAccess, ISimpleDbParameterProvider
    {
        
    }
    
}