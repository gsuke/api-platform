using Microsoft.AspNetCore.Mvc;
using Gsuke.ApiPlatform.Services;
using Gsuke.ApiPlatform.Errors;

namespace Gsuke.ApiPlatform.Controllers;

[ApiController]
[Route("[controller]")]
public class ApiController : ControllerBase
{
    private readonly ILogger<ApiController> _logger;
    private readonly IApiService _service;

    public ApiController(ILogger<ApiController> logger, IApiService apiService)
    {
        _logger = logger;
        _service = apiService;
    }

    [HttpGet("{url}")]
    public ActionResult<List<dynamic>> GetList(string url)
    {
        var (result, error) = _service.GetList(url);
        if (result is null || error is NotFoundError)
        {
            return NotFound(error);
        }
        return result;
    }

    [HttpGet("{url}/{id}")]
    public ActionResult<dynamic> Get(string url, string id)
    {
        var (result, error) = _service.Get(url, id);
        if (result is null || error is NotFoundError)
        {
            return NotFound(error);
        }
        return result;
    }
}
