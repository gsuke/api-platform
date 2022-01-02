using Gsuke.ApiPlatform.Repositories;
using Gsuke.ApiPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace Gsuke.ApiPlatform.Services;

public class ResourceService : IResourceService
{
    private readonly ILogger<ResourceService> _logger;
    private readonly IResourceRepository _repository;
    private readonly IMapper _mapper;

    public ResourceService(ILogger<ResourceService> logger, IResourceRepository resourceRepository)
    {
        _logger = logger;
        _repository = resourceRepository;

        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<ResourceEntity, ResourceDto>()));
    }

    public ActionResult<List<ResourceEntity>> GetList()
    {
        return _repository.GetList().ToList();
    }

    public ActionResult<ResourceDto> Get(string url)
    {
        var resource = _repository.Get(url);
        if (resource == null)
        {
            return new NotFoundResult();
        }
        return _mapper.Map<ResourceDto>(resource);
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
