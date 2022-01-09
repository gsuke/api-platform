using Microsoft.AspNetCore.Mvc;
using Gsuke.ApiPlatform.Services;
using Gsuke.ApiPlatform.Models;
using Gsuke.ApiPlatform.Errors;

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
        return _service.GetList().Select(x => _service.EntityToDto(x)).ToList();
    }

    [HttpGet("{url}")]
    public ActionResult<ResourceDto> Get(string url)
    {
        var resource = _service.Get(url);
        if (resource is null)
        {
            return NotFound(new NotFoundError(url));
        }
        return _service.EntityToDto(resource);
    }

    [HttpDelete("{url}")]
    public IActionResult Delete(string url)
    {
        var error = _service.Delete(url);
        if (error is not NoError)
        {
            if (error is NotFoundError)
            {
                return NotFound(error);
            }
        }
        return NoContent();
    }

    [HttpPost]
    public IActionResult Create(ResourceDto resource)
    {
        var error = _service.Create(resource);
        if (error is not NoError)
        {
            if (error is AlreadyExistsError)
            {
                return Conflict(error);
            }
            else
            {
                return BadRequest(error);
            }
        }
        return CreatedAtAction(nameof(Get), new { url = resource.url }, resource);
    }

    [HttpDelete]
    public IActionResult Delete()
    {
        _service.DeleteAll();
        return NoContent();
    }
}
