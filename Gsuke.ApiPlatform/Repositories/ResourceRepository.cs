using System.Text;
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

        public IEnumerable<Resource> GetList()
        {
            var sql = "SELECT * FROM resources;";
            return _conn.Query<Resource>(sql);
        }
    }
}
