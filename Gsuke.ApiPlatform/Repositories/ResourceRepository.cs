namespace Gsuke.ApiPlatform.Repositories
{
    public class ResourceRepository : IResourceRepository
    {
        private readonly ILogger<ResourceRepository> _logger;
        private readonly IConfiguration _config;

        public ResourceRepository(ILogger<ResourceRepository> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public string GetResourceList()
        {
            return _config["RdbConnectionString"];
        }
    }
}
