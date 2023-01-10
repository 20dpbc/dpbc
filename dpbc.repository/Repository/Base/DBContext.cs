using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System.Data;

namespace dpbc.repository.Repository
{
    public class DBContext : IDBContext
    {
        public IDbConnection connection { get; set; }

        public DBContext()
        {
            connection = new SqliteConnection("DataSource=file::memory:?cache=shared");
            connection.Open();
        }

        public DBContext(string conn)
        {
            connection = new SqlConnection(conn);
            connection.Open();
        }

        public void Dispose()
        {
            connection?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
