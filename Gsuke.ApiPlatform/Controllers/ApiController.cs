using Microsoft.AspNetCore.Mvc;
using Gsuke.ApiPlatform.Services;
using Gsuke.ApiPlatform.Errors;
using Newtonsoft.Json;

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

    [HttpDelete("{url}/{id}")]
    public IActionResult Delete(string url, string id)
    {
        var error = _service.Delete(url, id);
        if (error is NotFoundError)
        {
            return NotFound(error);
        }
        return NoContent();
    }

    // TODO: 全体的に非同期化するべきだと思う
    [HttpPost("{url}")]
    public async Task<IActionResult> Post(string url)
    {
        using (var reader = new StreamReader(Request.Body))
        {
            var body = await reader.ReadToEndAsync();
            dynamic? item = JsonConvert.DeserializeObject<dynamic>(body);

            var error = _service.Post(url, item);
            if (error is NotFoundError)
            {
                return NotFound(error);
            }
            else if (error is AlreadyExistsError)
            {
                return Conflict(error);
            }
            else if (error is not null)
            {
                return BadRequest(error);
            }
            return NoContent();
        }
    }
}
