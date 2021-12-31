using Microsoft.AspNetCore.Mvc;
using Gsuke.ApiPlatform.Services;

namespace Gsuke.ApiPlatform.Controllers;

[ApiController]
[Route("[controller]")]
public class ResourceController : ControllerBase
{
    private readonly ILogger<ResourceController> _logger;
    private readonly IConfiguration _config;
    private readonly IResourceService _service;

    public ResourceController(ILogger<ResourceController> logger, IConfiguration config, IResourceService resourceService)
    {
        _logger = logger;
        _config = config;
        _service = resourceService;
    }

    [HttpGet]
    public string GetList()
    {
        return _service.GetList();
    }
}
