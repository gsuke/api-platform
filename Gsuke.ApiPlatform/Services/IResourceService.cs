using Gsuke.ApiPlatform.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gsuke.ApiPlatform.Services
{
    public interface IResourceService
    {
        ActionResult<List<Resource>> GetList();
        ActionResult<Resource> Get(string url);
        IActionResult Delete(string url);
        IActionResult Create(Resource resource);
    }
}
