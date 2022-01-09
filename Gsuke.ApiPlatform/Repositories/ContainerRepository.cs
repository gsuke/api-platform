using System.Data;
using System.Text;
using Dapper;
using Gsuke.ApiPlatform.Models;
using Newtonsoft.Json.Schema;
using Gsuke.ApiPlatform.Misc;

namespace Gsuke.ApiPlatform.Repositories
{
    public class ContainerRepository : IContainerRepository
    {
        private readonly ILogger<ContainerRepository> _logger;
        private readonly IDbConnection _conn;

        public ContainerRepository(ILogger<ContainerRepository> logger, IDbConnection conn)
        {
            _logger = logger;
            _conn = conn;
        }

        public int Create(ResourceEntity resource, JSchema dataSchema)
        {
            // CREATE TABLE文の土台を作成
            var sql = new StringBuilder();
            sql.Append($"CREATE TABLE IF NOT EXISTS \"{GetContainerName(resource.container_id ?? throw new Exception())}\" (");

            // カラムをすべて定義する
            var properties = new List<string>();
            foreach (KeyValuePair<string, JSchema> property in dataSchema.Properties)
            {
                var tempStr = $"{property.Key} {ColumnType.ConvertJSchemaTypeToSqlColumnType(property.Value.Type ?? JSchemaType.None)}";

                // 制約を付与
                if (property.Key == "id")
                {
                    // idカラムは主キーとなる
                    tempStr += " PRIMARY KEY";
                }
                else
                {
                    // idカラム以外: データスキーマを参照し、requiredになっていればNOT NULL制約をつける
                    if (dataSchema.Required.FirstOrDefault(name => name == property.Key) is not null)
                    {
                        tempStr += " NOT NULL";
                    }
                }
                properties.Add(tempStr);
            }
            sql.Append(String.Join(", ", properties));

            sql.Append(")");
            _logger.LogInformation(sql.ToString());
            return _conn.Execute(sql.ToString());
        }

        public int Delete(Guid containerId)
        {
            var sql = $"DROP TABLE IF EXISTS \"{GetContainerName(containerId)}\"";
            return _conn.Execute(sql);
        }

        private string GetContainerName(Guid containerId) => $"container-{containerId}";
    }
}
