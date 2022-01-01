using Gsuke.ApiPlatform.Repositories;
using Gsuke.ApiPlatform.Models;

namespace Gsuke.ApiPlatform.Services;

public class ResourceService : IResourceService
{
    private readonly ILogger<ResourceService> _logger;
    private readonly IResourceRepository _repository;

    public ResourceService(ILogger<ResourceService> logger, IResourceRepository resourceRepository)
    {
        _logger = logger;
        _repository = resourceRepository;
    }

    public IEnumerable<Resource> GetList()
    {
        return _repository.GetList();
    }

    public Resource Get(string url)
    {
        return _repository.Get(url);
    }

    public bool Exists(string url)
    {
        return _repository.Exists(url);
    }
}
