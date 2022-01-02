using Microsoft.AspNetCore.Mvc;
using Gsuke.ApiPlatform.Services;
using Gsuke.ApiPlatform.Models;

namespace Gsuke.ApiPlatform.Controllers;

[ApiController]
[Route("[controller]")]
public class ResourceController : ControllerBase
{
    private readonly ILogger<ResourceController> _logger;
    private readonly IResourceService _service;

    public ResourceController(ILogger<ResourceController> logger, IResourceService resourceService)
    {
        _logger = logger;
        _service = resourceService;
    }

    [HttpGet]
    public ActionResult<List<ResourceDto>> GetList()
    {
        return _service.GetList();
    }

    [HttpGet("{url}")]
    public ActionResult<ResourceDto> Get(string url)
    {
        return _service.Get(url);
    }

    [HttpDelete("{url}")]
    public IActionResult Delete(string url)
    {
        return _service.Delete(url);
    }

    [HttpPost]
    public IActionResult Create(ResourceDto resource)
    {
        return _service.Create(resource);
    }
}
