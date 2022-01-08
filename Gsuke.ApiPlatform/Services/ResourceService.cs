using Gsuke.ApiPlatform.Repositories;
using Gsuke.ApiPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Newtonsoft.Json.Schema;
using Gsuke.ApiPlatform.Misc;

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
        if (resource is null)
        {
            return new NotFoundResult();
        }
        return _mapper.Map<ResourceDto>(resource);
    }

    public IActionResult Delete(string url)
    {
        var resource = _resourceRepository.Get(url);
        if (resource is null)
        {
            return new NotFoundResult();
        }

        _resourceRepository.Delete(url);
        _containerRepository.Delete(resource.container_id);
        return new NoContentResult();
    }

    public IActionResult Create(ResourceDto resourceDto)
    {
        if (String.IsNullOrEmpty(resourceDto.url) || String.IsNullOrEmpty(resourceDto.dataSchema))
        {
            throw new InvalidOperationException();
        }

        if (Exists(resourceDto.url))
        {
            return new ConflictResult();
        }

        // データスキーマの解析
        JSchema? dataSchema = ParseDataSchema(resourceDto.dataSchema);
        if (dataSchema is null)
        {
            return new BadRequestResult();
        }

        var resourceEntity = _mapper.Map<ResourceEntity>(resourceDto);
        resourceEntity.container_id = Guid.NewGuid();

        // TODO: エラーハンドリングの方式を検討する必要あり
        _containerRepository.Create(resourceEntity, dataSchema);
        _resourceRepository.Create(resourceEntity);
        return new CreatedResult(nameof(Get), resourceDto);
    }

    private bool Exists(string url)
    {
        return _resourceRepository.Exists(url);
    }

    /// <summary>
    /// データスキーマ文字列の妥当性を検証し、JSchemaに変換する
    /// </summary>
    /// <param name="dataSchema"></param>
    /// <returns>正しければJSchemaを返す、不正ならばnullを返す</returns>
    private JSchema? ParseDataSchema(string dataSchema)
    {
        JSchema jSchema;
        try
        {
            jSchema = JSchema.Parse(dataSchema);
        }
        catch
        {
            return null;
        }

        var hasIdColumn = false;

        foreach (KeyValuePair<string, JSchema> property in jSchema.Properties)
        {
            // 指定されたTypeが対応していること
            if (ColumnType.ConvertJSchemaTypeToSqlColumnType(property.Value.Type ?? JSchemaType.None) is null)
            {
                return null;
            }
            // idカラムが含まれていること
            if (property.Key == "id")
            {
                hasIdColumn = true;
            }
        }

        if (!hasIdColumn)
        {
            return null;
        }

        return jSchema;
    }
}
