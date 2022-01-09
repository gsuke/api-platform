using Gsuke.ApiPlatform.Models;
using Gsuke.ApiPlatform.Errors;

namespace Gsuke.ApiPlatform.Services
{
    public interface IResourceService
    {
        List<ResourceEntity> GetList();
        ResourceEntity? Get(string url);
        Error? Delete(string url);
        Error? Create(ResourceDto resourceDto);
        ResourceDto EntityToDto(ResourceEntity resourceEntity);
        void DeleteAll();
    }
}
