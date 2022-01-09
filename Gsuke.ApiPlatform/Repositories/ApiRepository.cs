using System.Data;
using Dapper;

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

        public int Post(Guid containerId, Dictionary<string, dynamic> item)
        {
            var sql = @$"
INSERT
    INTO
    ""{GetContainerName(containerId)}""
    (
        {{0}}
    )
VALUES (
    {{1}}
)";

            // SQL文のINSERT INTO句とVALUES句の中を作成
            // TODO: SQLインジェクション対策が必要
            var sortedItem = new SortedDictionary<string, dynamic>(item);
            sql = String.Format(sql, String.Join(",\n", sortedItem.Keys), String.Join(",\n", sortedItem.Keys.Select(x => $"@{x}")));

            // 実行
            _logger.LogInformation(sql);
            return _conn.Execute(sql, item);
        }

        // TODO: 同じ処理がContainerRepositoryにも存在するので共通化すべき
        private string GetContainerName(Guid containerId) => $"container-{containerId}";
    }
}
