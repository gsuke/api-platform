using Microsoft.AspNetCore.Mvc;
using Gsuke.ApiPlatform.Repositories;

namespace Gsuke.ApiPlatform.Controllers;

[ApiController]
[Route("[controller]")]
public class ResourceController : ControllerBase
{
    private readonly ILogger<ResourceController> _logger;
    private readonly IConfiguration _config;
    private readonly IResourceRepository _resourceRepository;

    public ResourceController(ILogger<ResourceController> logger, IConfiguration config, IResourceRepository resourceRepository)
    {
        _logger = logger;
        _config = config;
        _resourceRepository = resourceRepository;
    }

    [HttpGet]
    public string Get()
    {
        return _resourceRepository.GetResourceList();
    }
}
