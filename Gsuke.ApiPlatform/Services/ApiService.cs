using Gsuke.ApiPlatform.Repositories;
using Gsuke.ApiPlatform.Errors;
using Gsuke.ApiPlatform.Misc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Gsuke.ApiPlatform.Services;

public class ApiService : IApiService
{
    private readonly ILogger<ApiService> _logger;
    private readonly IResourceService _resourceService;
    private readonly IApiRepository _repository;

    public ApiService(ILogger<ApiService> logger, IResourceService resourceService, IApiRepository apiRepository)
    {
        _logger = logger;
        _resourceService = resourceService;
        _repository = apiRepository;
    }

    public (List<dynamic>?, Error?) GetList(string url)
    {
        var resource = _resourceService.Get(url);
        if (resource is null)
        {
            return (null, new NotFoundError(url));
        }
        return (_repository.GetList(resource.container_id ?? throw new Exception()), null);
    }

    public (dynamic?, Error?) Get(string url, string id)
    {
        var resource = _resourceService.Get(url);
        if (resource is null)
        {
            return (null, new NotFoundError(url));
        }
        var item = _repository.Get(resource.container_id ?? throw new Exception(), id);
        if (item is null)
        {
            return (null, new NotFoundError($"{url}/{id}"));
        }
        return (item, null);
    }

    public Error? Delete(string url, string id)
    {
        var resource = _resourceService.Get(url);
        if (resource is null)
        {
            return new NotFoundError(url);
        }
        if (_repository.Get(resource.container_id ?? throw new Exception(), id) is null)
        {
            return new NotFoundError($"{url}/{id}");
        }
        _repository.Delete(resource.container_id ?? throw new Exception(), id);
        return null;
    }

    public Error? Post(string url, dynamic item)
    {
        string? id = item.id;
        // TODO: ID自動割り振り機能を実装したい
        if (String.IsNullOrEmpty(id))
        {
            return new Error();
        }

        var resource = _resourceService.Get(url);
        if (resource is null)
        {
            return new NotFoundError(url);
        }

        if (_repository.Get(resource.container_id ?? throw new Exception(), id) is not null)
        {
            return new AlreadyExistsError($"{url}/{id}");
        }

        // データスキーマを取得
        var dataSchema = DataSchema.ParseDataSchema(resource.data_schema ?? throw new Exception());
        if (dataSchema is null)
        {
            throw new Exception();
        }
        dataSchema.AllowAdditionalProperties = false; // デバッグ用

        // 入力値がデータスキーマに沿っているかを確認
        var jItem = (JObject)item;
        var isValid = jItem.IsValid(dataSchema); // TODO: エラーメッセージを取得してErrorに流そう
        if (!isValid)
        {
            return new DataSchemaValidationError();
        }

        _logger.LogInformation("OK");
        return null;
    }

}
