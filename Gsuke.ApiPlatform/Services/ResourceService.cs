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

    public void Delete(string url)
    {
        int count = _repository.Delete(url);
        if (count != 1)
        {
            throw new Exception();
        }
    }

    public void Create(Resource resource)
    {
        _repository.Create(resource);
    }

    private bool Exists(string url)
    {
        return _repository.Exists(url);
    }
}
