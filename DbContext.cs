using System.Data.Common;
using System.Data.SqlClient;

namespace RepositoryBase
{
    public abstract class DbContext : DbConnection
    {

        private static SqlConnection? _connection;

        protected static SqlConnection Connection { get => _connection; }


        public DbContext(IConfiguration configuration)
        {
          _connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
