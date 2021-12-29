using System.Text;
using Npgsql;

namespace Gsuke.ApiPlatform.Repositories
{
    public class ResourceRepository : IResourceRepository
    {
        private readonly ILogger<ResourceRepository> _logger;
        private readonly IConfiguration _config;

        public ResourceRepository(ILogger<ResourceRepository> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public string GetResourceList()
        {
            var sql = "SELECT * FROM resources;";
            var result = new StringBuilder();

            using (var connection = new NpgsqlConnection(_config["RdbConnectionString"]))
            using (var command = new NpgsqlCommand(sql, connection))
            {
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Append($" {reader["id"]}, {reader["url"]} ");
                    }
                }
            }

            return result.ToString();
        }
    }
}
