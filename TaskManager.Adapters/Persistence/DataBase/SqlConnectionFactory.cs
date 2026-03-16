using TaskManager.Adapters.Persistence.DataBase.Interface;

namespace TaskManager.Adapters.Persistence.DataBase
{
    public class SqlConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;
        public SqlConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
        }

        public SqlConnection Create()
            => new SqlConnection(_connectionString);
    }
}
