using Gsuke.ApiPlatform.Repositories;
using Gsuke.ApiPlatform.Models;
using AutoMapper;
using Newtonsoft.Json.Schema;
using Gsuke.ApiPlatform.Misc;
using Gsuke.ApiPlatform.Errors;

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

    public List<ResourceEntity> GetList()
    {
        return _resourceRepository.GetList();
    }

    public ResourceEntity? Get(string url)
    {
        var resource = _resourceRepository.Get(url);
        if (resource is null)
        {
            return null;
        }
        return resource;
    }

    public Error Delete(string url)
    {
        var resource = _resourceRepository.Get(url);
        if (resource is null)
        {
            return new NotFoundError(url);
        }

        _resourceRepository.Delete(url);
        _containerRepository.Delete(resource.container_id ?? throw new Exception());
        return new NoError();
    }

    public Error Create(ResourceDto resourceDto)
    {
        if (String.IsNullOrEmpty(resourceDto.url) || String.IsNullOrEmpty(resourceDto.dataSchema))
        {
            throw new InvalidOperationException();
        }

        if (Get(resourceDto.url) is not null)
        {
            return new AlreadyExistsError(resourceDto.url);
        }

        // ??????????????????????????????
        var (dataSchema, error) = DataSchema.ParseDataSchema(resourceDto.dataSchema);
        if (dataSchema is null)
        {
            return error;
        }

        var resourceEntity = _mapper.Map<ResourceEntity>(resourceDto);
        resourceEntity.container_id = Guid.NewGuid();

        _containerRepository.Create(resourceEntity, dataSchema);
        _resourceRepository.Create(resourceEntity);
        return new NoError();
    }

    public ResourceDto EntityToDto(ResourceEntity resourceEntity)
    {
        return _mapper.Map<ResourceDto>(resourceEntity);
    }

    public void DeleteAll()
    {
        var resources = GetList();
        foreach (var resource in resources)
        {
            if (String.IsNullOrEmpty(resource.url))
            {
                continue;
            }
            Delete(resource.url);
        }
    }


}
