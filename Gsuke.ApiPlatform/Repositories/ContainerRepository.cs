using System.Data;
using System.Text;
using Dapper;
using Gsuke.ApiPlatform.Models;
using Newtonsoft.Json.Schema;

namespace Gsuke.ApiPlatform.Repositories
{
    public class ContainerRepository : IContainerRepository
    {
        private readonly ILogger<ContainerRepository> _logger;
        private readonly IDbConnection _conn;

        private Dictionary<int, string> _sqlColumnType = new Dictionary<int, string>{
            {(int)JSchemaType.String, "varchar(64)"},
            {(int)JSchemaType.Number, "double"},
            {(int)JSchemaType.Integer, "integer"},
            {(int)JSchemaType.Boolean, "boolean"},
        };

        public ContainerRepository(ILogger<ContainerRepository> logger, IDbConnection conn)
        {
            _logger = logger;
            _conn = conn;
        }

        public int Create(ResourceEntity resource, JSchema dataSchema)
        {
            var sql = new StringBuilder();
            sql.Append($"CREATE TABLE IF NOT EXISTS \"{GetContainerName(resource.container_id)}\" (");

            var properties = new List<string>();
            foreach (KeyValuePair<string, JSchema> property in dataSchema.Properties)
            {
                properties.Add($"{property.Key} {ConvertJSchemaTypeToSqlColumnType(property.Value.Type)}");
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

        private string ConvertJSchemaTypeToSqlColumnType(JSchemaType? type)
        {
            if (type is null)
            {
                throw new InvalidOperationException();
            }
            return _sqlColumnType[(int)type];
        }
    }
}
