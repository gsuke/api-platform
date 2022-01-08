using Gsuke.ApiPlatform.Repositories;
using Gsuke.ApiPlatform.Errors;

namespace Gsuke.ApiPlatform.Services;

public class ApiService : IApiService
{
    private readonly ILogger<ResourceService> _logger;
    private readonly IResourceService _resourceService;
    private readonly IApiRepository _repository;

    public ApiService(ILogger<ResourceService> logger, IResourceService resourceService, IApiRepository apiRepository)
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
        return (_repository.GetList(resource.container_id), null);
    }

    public (dynamic?, Error?) Get(string url, string id)
    {
        var resource = _resourceService.Get(url);
        if (resource is null)
        {
            return (null, new NotFoundError(url));
        }
        var item = _repository.Get(resource.container_id, id);
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
        if (_repository.Get(resource.container_id, id) is null)
        {
            return new NotFoundError($"{url}/{id}");
        }
        _repository.Delete(resource.container_id, id);
        return null;
    }

}
