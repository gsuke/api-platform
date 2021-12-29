using System.Text;
using Npgsql;

namespace Gsuke.ApiPlatform.Repositories
{
    public class ResourceRepository : IResourceRepository
    {
        private readonly ILogger<ResourceRepository> _logger;

        private readonly IRdbConnection _conn;

        public ResourceRepository(ILogger<ResourceRepository> logger, IRdbConnection conn)
        {
            _logger = logger;
            _conn = conn;
        }

        public string GetResourceList()
        {
            var sql = "SELECT * FROM resources;";
            var result = new StringBuilder();

            using (var reader = _conn.Command(sql).ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Append($" {reader["id"]}, {reader["url"]} ");
                }
            }

            return result.ToString();
        }
    }
}
