using Gsuke.ApiPlatform.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gsuke.ApiPlatform.Services
{
    public interface IResourceService
    {
        List<ResourceDto> GetList();
        ActionResult<ResourceDto> Get(string url);
        IActionResult Delete(string url);
        IActionResult Create(ResourceDto resourceDto);
    }
}
