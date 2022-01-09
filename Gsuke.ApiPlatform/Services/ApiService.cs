using System.Threading.Tasks.Dataflow;
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

    public Error? Post(string url, Dictionary<string, dynamic> item)
    {
        // TODO: ID自動割り振り機能を実装したい

        // リソースを取得
        var resource = _resourceService.Get(url);
        if (resource is null)
        {
            return new NotFoundError(url);
        }

        // id値を取得
        dynamic? id;
        if (!item.TryGetValue("id", out id))
        {
            return new JsonIdIsNotIncludedError();
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

        // 入力値がデータスキーマに沿っているかを確認
        var jItem = JObject.FromObject(item);
        if (!jItem.IsValid(dataSchema)) // TODO: エラーメッセージを取得してErrorに流そう
        {
            return new DataSchemaValidationError();
        }

        _repository.Post(resource.container_id ?? throw new Exception(), item);
        return null;
    }

}
