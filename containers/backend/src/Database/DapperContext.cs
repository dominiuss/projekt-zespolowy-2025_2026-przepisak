/* using System.Data;
using Microsoft.Data.Sqlite;

namespace PrzepisakApi.src.Database
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public IDbConnection CreateConnection()
            => new SqliteConnection(_connectionString);
    }
}
*/
using Npgsql;
using System.Data;
using System.Diagnostics.CodeAnalysis;
[ExcludeFromCodeCoverage]
public class DapperContext
{
    private readonly string _connectionString;

    public DapperContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public IDbConnection CreateConnection()
        => new NpgsqlConnection(_connectionString);
}

