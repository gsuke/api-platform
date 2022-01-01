using Gsuke.ApiPlatform.Models;

namespace Gsuke.ApiPlatform.Services
{
    public interface IResourceService
    {
        IEnumerable<Resource> GetList();
        Resource Get(string url);
        bool Exists(string url);
    }
}
