using Gsuke.ApiPlatform.Repositories;
using Gsuke.ApiPlatform.Models;
using Microsoft.AspNetCore.Mvc;

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

    public ActionResult<List<ResourceEntity>> GetList()
    {
        return _repository.GetList().ToList();
    }

    public ActionResult<ResourceEntity> Get(string url)
    {
        var resource = _repository.Get(url);
        if (resource == null)
        {
            return new NotFoundResult();
        }
        return resource;
    }

    public IActionResult Delete(string url)
    {
        if (!Exists(url))
        {
            return new NotFoundResult();
        }
        _repository.Delete(url);
        return new NoContentResult();
    }

    public IActionResult Create(ResourceEntity resource)
    {
        if (String.IsNullOrEmpty(resource.url))
        {
            return new BadRequestResult();
        }
        if (Exists(resource.url))
        {
            return new ConflictResult();
        }
        _repository.Create(resource);
        return new CreatedResult(nameof(Get), resource);
    }

    private bool Exists(string url)
    {
        return _repository.Exists(url);
    }
}
