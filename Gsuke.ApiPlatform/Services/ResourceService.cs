using Gsuke.ApiPlatform.Repositories;
using Gsuke.ApiPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace Gsuke.ApiPlatform.Services;

public class ResourceService : IResourceService
{
    private readonly ILogger<ResourceService> _logger;
    private readonly IResourceRepository _resourceRepository;
    private readonly IContainerRepository _containerRepository;
    private readonly IMapper _mapper;

    public ResourceService(ILogger<ResourceService> logger, IResourceRepository resourceRepository, IContainerRepository containerRepository)
    {
        _logger = logger;
        _resourceRepository = resourceRepository;
        _containerRepository = containerRepository;

        _mapper = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<ResourceEntity, ResourceDto>().ReverseMap();
            cfg.SourceMemberNamingConvention = new LowerUnderscoreNamingConvention();
        }).CreateMapper();
    }

    public ActionResult<List<ResourceDto>> GetList()
    {
        return _mapper.Map<List<ResourceDto>>(_resourceRepository.GetList());
    }

    public ActionResult<ResourceDto> Get(string url)
    {
        var resource = _resourceRepository.Get(url);
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
        _resourceRepository.Delete(url);
        return new NoContentResult();
    }

    public IActionResult Create(ResourceDto resourceDto)
    {
        if (String.IsNullOrEmpty(resourceDto.url))
        {
            return new BadRequestResult();
        }
        if (Exists(resourceDto.url))
        {
            return new ConflictResult();
        }

        var resourceEntity = _mapper.Map<ResourceEntity>(resourceDto);
        resourceEntity.container_id = Guid.NewGuid();

        _resourceRepository.Create(resourceEntity);
        return new CreatedResult(nameof(Get), resourceDto);
    }

    private bool Exists(string url)
    {
        return _resourceRepository.Exists(url);
    }
}
