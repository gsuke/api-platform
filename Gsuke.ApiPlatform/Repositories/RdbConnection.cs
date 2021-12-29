using Npgsql;

namespace Gsuke.ApiPlatform.Repositories
{
    public class RdbConnection : IRdbConnection
    {
        private readonly ILogger<RdbConnection> _logger;
        private readonly IConfiguration _config;

        private NpgsqlConnection _conn;

        public RdbConnection(ILogger<RdbConnection> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;

            _conn = new NpgsqlConnection(_config["RdbConnectionString"]);
            _conn.Open();
        }

        ~RdbConnection()
        {
            _conn.Close();
        }

        public NpgsqlCommand Command(string sql)
        {
            return new NpgsqlCommand(sql, _conn);
        }

    }
}
