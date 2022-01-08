using Gsuke.ApiPlatform.Models;
using Gsuke.ApiPlatform.Errors;

namespace Gsuke.ApiPlatform.Services
{
    public interface IResourceService
    {
        List<ResourceDto> GetList();
        ResourceDto? Get(string url);
        Error? Delete(string url);
        Error? Create(ResourceDto resourceDto);
        bool Exists(string url);
    }
}
