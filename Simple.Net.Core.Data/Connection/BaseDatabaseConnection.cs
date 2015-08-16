using System;

namespace Simple.Net.Core.Data.Connection
{
    public class BaseDatabaseConnection
    {
        public String ConnectionString { get; set; }

        public BaseDatabaseConnection(string connectionString)
        {
            // TODO: Add decrption capabilities
            ConnectionString = connectionString;
        }

    }
}
