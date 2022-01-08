using Gsuke.ApiPlatform.Repositories;
using Gsuke.ApiPlatform.Errors;

namespace Gsuke.ApiPlatform.Services;

public class ApiService : IApiService
{
    private readonly ILogger<ResourceService> _logger;
    private readonly IResourceService _resourceService;
    private readonly IContainerRepository _containerRepository;

    public ApiService(ILogger<ResourceService> logger, IResourceService resourceService, IContainerRepository containerRepository)
    {
        _logger = logger;
        _resourceService = resourceService;
        _containerRepository = containerRepository;
    }

    public (List<dynamic>?, Error?) GetList(string url)
    {
        var resource = _resourceService.Get(url);
        if (resource is null)
        {
            return (null, new NotFoundError(url));
        }
        return (_containerRepository.GetList(resource.container_id), null);
    }

    public (dynamic?, Error?) Get(string url, string id)
    {
        var resource = _resourceService.Get(url);
        if (resource is null)
        {
            return (null, new NotFoundError(url));
        }
        var item = _containerRepository.Get(resource.container_id, id);
        if (item is null)
        {
            return (null, new NotFoundError($"{url}/{id}"));
        }
        return (item, null);
    }

}
