using Microsoft.AspNetCore.Mvc;
using Gsuke.ApiPlatform.Repositories;

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

    [HttpGet]
    public string GetList()
    {
        return _repository.GetList();
    }
}
