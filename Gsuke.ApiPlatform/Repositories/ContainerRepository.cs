using System.Data;
using Dapper;
using Gsuke.ApiPlatform.Models;

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

        public int Create(ResourceEntity resource)
        {
            return 1;
        }
        public int Delete(string containerId)
        {
            return 1;
        }

    }
}
