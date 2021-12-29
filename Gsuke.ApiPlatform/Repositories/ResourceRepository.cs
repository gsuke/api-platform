namespace Gsuke.ApiPlatform.Repositories
{
    public class ResourceRepository : IResourceRepository
    {
        private readonly ILogger<IResourceRepository> _logger;
        private readonly IConfiguration _config;

        public ResourceRepository(ILogger<IResourceRepository> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public string GetResourceList()
        {
            return _config["RdbConnectionStrings"];
        }
    }
}
