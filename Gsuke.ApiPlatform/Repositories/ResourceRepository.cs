using System.Text;
using System.Data;
using Dapper;
using Gsuke.ApiPlatform.Models;
using Npgsql;

namespace Gsuke.ApiPlatform.Repositories
{
    public class ResourceRepository : IResourceRepository
    {
        private readonly ILogger<ResourceRepository> _logger;
        private readonly IDbConnection _conn;

        public ResourceRepository(ILogger<ResourceRepository> logger, IDbConnection conn)
        {
            _logger = logger;
            _conn = conn;
        }

        public IEnumerable<Resource> GetList()
        {
            var sql = "SELECT * FROM resources;";
            return _conn.Query<Resource>(sql);
        }

        public Resource Get(string url)
        {
            var sql = @"
SELECT
    *
FROM
    resources
WHERE
    url = @url
; ";
            var param = new { url = url };
            return _conn.QueryFirstOrDefault<Resource>(sql, param);
        }
    }
}
