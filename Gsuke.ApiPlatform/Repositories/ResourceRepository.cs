using System.Data;
using Dapper;
using Gsuke.ApiPlatform.Models;

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

        public IEnumerable<ResourceEntity> GetList()
        {
            var sql = "SELECT * FROM resources;";
            return _conn.Query<ResourceEntity>(sql);
        }

        public ResourceEntity? Get(string url)
        {
            var sql = @"
SELECT
    *
FROM
    resources
WHERE
    url = @url
";
            var param = new { url = url };
            return _conn.QueryFirstOrDefault<ResourceEntity>(sql, param);
        }

        public bool Exists(string url)
        {
            var sql = @"
SELECT
    EXISTS (
        SELECT
            *
        FROM
            resources
        WHERE
            url = @url
    )
";
            var param = new { url = url };
            return _conn.QueryFirstOrDefault<bool>(sql, param);
        }

        public int Delete(string url)
        {
            var sql = @"
DELETE
FROM
    resources
WHERE
    url = @url
";
            var param = new { url = url };
            return _conn.Execute(sql, param);
        }

        public int Create(ResourceEntity resource)
        {
            var sql = @"
INSERT
    INTO
    resources
    (
        url
        , data_schema
        , container_id
    )
VALUES
(
    @url
    , @data_schema
    , @container_id
)
";
            return _conn.Execute(sql, resource);
        }
    }
}
