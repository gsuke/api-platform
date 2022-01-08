using Gsuke.ApiPlatform.Errors;

namespace Gsuke.ApiPlatform.Services
{
    public interface IApiService
    {
        (List<dynamic>?, Error?) GetList(string url);
    }
}
