using Gsuke.ApiPlatform.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gsuke.ApiPlatform.Services
{
    public interface IResourceService
    {
        ActionResult<List<ResourceEntity>> GetList();
        ActionResult<ResourceEntity> Get(string url);
        IActionResult Delete(string url);
        IActionResult Create(ResourceEntity resource);
    }
}
