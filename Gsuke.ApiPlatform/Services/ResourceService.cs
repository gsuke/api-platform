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

    public Error? Delete(string url)
    {
        var resource = _resourceRepository.Get(url);
        if (resource is null)
        {
            return new NotFoundError(url);
        }

        _resourceRepository.Delete(url);
        _containerRepository.Delete(resource.container_id);
        return null;
    }

    public Error? Create(ResourceDto resourceDto)
    {
        if (String.IsNullOrEmpty(resourceDto.url) || String.IsNullOrEmpty(resourceDto.dataSchema))
        {
            throw new InvalidOperationException();
        }

        if (Get(resourceDto.url) is not null)
        {
            return new AlreadyExistsError(resourceDto.url);
        }

        // データスキーマの解析
        JSchema? dataSchema = ParseDataSchema(resourceDto.dataSchema);
        if (dataSchema is null)
        {
            return new DataSchemaError();
        }

        var resourceEntity = _mapper.Map<ResourceEntity>(resourceDto);
        resourceEntity.container_id = Guid.NewGuid();

        _containerRepository.Create(resourceEntity, dataSchema);
        _resourceRepository.Create(resourceEntity);
        return null;
    }

    public ResourceDto EntityToDto(ResourceEntity resourceEntity)
    {
        return _mapper.Map<ResourceDto>(resourceEntity);
    }

    /// <summary>
    /// データスキーマ文字列の妥当性を検証し、JSchemaに変換する
    /// </summary>
    /// <param name="dataSchema"></param>
    /// <returns>正しければJSchemaを返す、不正ならばnullを返す</returns>
    private JSchema? ParseDataSchema(string dataSchema)
    {
        // TODO: ここでエラー詳細を返すべき
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
