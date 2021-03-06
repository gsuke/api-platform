using Gsuke.ApiPlatform.Models;

namespace Gsuke.ApiPlatform.Repositories
{
    public interface IResourceRepository
    {
        List<ResourceEntity> GetList();
        ResourceEntity? Get(string url);
        bool Exists(string url);
        int Delete(string url);
        int Create(ResourceEntity resource);
    }
}
