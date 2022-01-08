using System.Data;
using System.Text;
using Dapper;
using Gsuke.ApiPlatform.Models;
using Newtonsoft.Json.Schema;
using Gsuke.ApiPlatform.Misc;

namespace Gsuke.ApiPlatform.Repositories
{
    public class ApiRepository : IApiRepository
    {
        private readonly ILogger<ApiRepository> _logger;
        private readonly IDbConnection _conn;

        public ApiRepository(ILogger<ApiRepository> logger, IDbConnection conn)
        {
            _logger = logger;
            _conn = conn;
        }

        public List<dynamic> GetList(Guid containerId)
        {
            var sql = $"SELECT * FROM \"{GetContainerName(containerId)}\"";
            return _conn.Query(sql).ToList();
        }

        public dynamic? Get(Guid containerId, string id)
        {
            var sql = @$"
SELECT
    *
FROM
    ""{GetContainerName(containerId)}""
WHERE
    id = @id
";
            var param = new { id = id };
            return _conn.QueryFirstOrDefault(sql, param);
        }

        public int Delete(Guid containerId, string id)
        {
            var sql = @$"
DELETE
FROM
    ""{GetContainerName(containerId)}""
WHERE
    id = @id
";
            var param = new { id = id };
            return _conn.Execute(sql, param);
        }

        // TODO: 同じ処理がContainerRepositoryにも存在するので共通化すべき
        private string GetContainerName(Guid containerId) => $"container-{containerId}";
    }
}
