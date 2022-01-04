using System.Data;
using Dapper;
using Gsuke.ApiPlatform.Models;
using Newtonsoft.Json.Schema;

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
            var sql = $"CREATE TABLE IF NOT EXISTS \"{GetContainerName(resource.container_id)}\" ()";
            return _conn.Execute(sql);
        }
        public int Delete(Guid containerId)
        {
            var sql = $"DROP TABLE IF EXISTS \"{GetContainerName(containerId)}\"";
            return _conn.Execute(sql);
        }

        private string GetContainerName(Guid containerId) => $"container-{containerId}";

    }
}
