using Gsuke.ApiPlatform.Models;
using Newtonsoft.Json.Schema;

namespace Gsuke.ApiPlatform.Repositories
{
    public interface IApiRepository
    {
        List<dynamic> GetList(Guid containerId);
        dynamic? Get(Guid containerId, string id);
        int Delete(Guid containerId, string id);
    }
}
