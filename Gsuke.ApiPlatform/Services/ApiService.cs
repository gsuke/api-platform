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

    public (List<dynamic>?, Error) GetList(string url)
    {
        var resource = _resourceService.Get(url);
        if (resource is null)
        {
            return (null, new NotFoundError(url));
        }
        return (_repository.GetList(resource.container_id ?? throw new Exception()), new NoError());
    }

    public (dynamic?, Error) Get(string url, string id)
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
        return (item, new NoError());
    }

    public Error Delete(string url, string id)
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
        return new NoError();
    }

    public Error Post(string url, Dictionary<string, dynamic> item)
    {
        // TODO: ID??????????????????????????????????????????

        // ?????????????????????
        var resource = _resourceService.Get(url);
        if (resource is null)
        {
            return new NotFoundError(url);
        }

        // ??????????????????????????????
        var (dataSchema, error) = DataSchema.ParseDataSchema(resource.data_schema ?? throw new Exception());
        if (dataSchema is null)
        {
            return error;
        }

        // ???????????????????????????????????????????????????????????????
        var jItem = JObject.FromObject(item);
        if (!jItem.IsValid(dataSchema, out IList<string> errorMessages))
        {
            return new DataSchemaValidationError(String.Join(" ", errorMessages));
        }

        // id????????????
        dynamic? id;
        if (!item.TryGetValue("id", out id))
        {
            // TODO: ????????????????????????????????????????????????????????????????????????????????????????????????Exception????????????????????????
            return new DataSchemaValidationError();
        }

        // ???????????????????????????
        if (_repository.Get(resource.container_id ?? throw new Exception(), id) is not null)
        {
            return new AlreadyExistsError($"{url}/{id}");
        }

        _repository.Post(resource.container_id ?? throw new Exception(), item);
        return new NoError();
    }

    public Error DeleteAll(string url)
    {
        var (items, error) = GetList(url);
        if (items is null)
        {
            return error;
        }

        foreach (IDictionary<string, object> item in items)
        {
            Delete(url, (string)item["id"]);
        }

        return new NoError();
    }

    public (Dictionary<string, dynamic>?, Error) JsonToDictionary(string json)
    {
        // ??????????????????
        Dictionary<string, dynamic> result;
        try
        {
            result = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(json) ?? throw new JsonException();
        }
        catch (JsonException)
        {
            return (null, new JsonError());
        }

        // id????????????????????????????????????
        if (!result.ContainsKey("id"))
        {
            // id?????????????????????????????????id????????????????????????
            result["id"] = Guid.NewGuid().ToString();
        }

        return (result, new NoError());
    }

}
