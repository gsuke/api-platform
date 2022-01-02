using Gsuke.ApiPlatform.Models;

namespace Gsuke.ApiPlatform.Repositories
{
    public interface IContainerRepository
    {
        int Create(ResourceEntity resource);
        int Delete(string containerId);
    }
}
