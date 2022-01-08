using Gsuke.ApiPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Gsuke.ApiPlatform.Errors;

namespace Gsuke.ApiPlatform.Services
{
    public interface IResourceService
    {
        List<ResourceDto> GetList();
        ResourceDto? Get(string url);
        Error? Delete(string url);
        IActionResult Create(ResourceDto resourceDto);
    }
}
