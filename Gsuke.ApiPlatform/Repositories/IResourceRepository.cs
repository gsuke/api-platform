using Gsuke.ApiPlatform.Models;

namespace Gsuke.ApiPlatform.Repositories
{
    public interface IResourceRepository
    {
        IEnumerable<Resource> GetList();
        Resource Get(string url);
        bool Exists(string url);
        int Delete(string url);
        int Create(Resource resource);
    }
}
