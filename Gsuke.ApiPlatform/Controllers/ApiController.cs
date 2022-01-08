using Microsoft.AspNetCore.Mvc;
using Gsuke.ApiPlatform.Services;
using Gsuke.ApiPlatform.Models;

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
    public ActionResult<string> GetList(string url)
    {
        return url;
    }

    [HttpGet("{url}/{id}")]
    public ActionResult<string> Get(string url, string id)
    {
        return url + ", " + id;
    }
}
