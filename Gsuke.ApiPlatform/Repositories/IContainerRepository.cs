using Gsuke.ApiPlatform.Models;
using Newtonsoft.Json.Schema;

namespace Gsuke.ApiPlatform.Repositories
{
    public interface IContainerRepository
    {
        int Create(ResourceEntity resource, JSchema dataSchema);
        int Delete(Guid containerId);
        List<dynamic> GetList(Guid containerId);
    }
}
