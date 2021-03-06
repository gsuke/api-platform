using Gsuke.ApiPlatform.Errors;

namespace Gsuke.ApiPlatform.Services
{
    public interface IApiService
    {
        (List<dynamic>?, Error) GetList(string url);
        (dynamic?, Error) Get(string url, string id);
        Error Delete(string url, string id);
        Error Post(string url, Dictionary<string, dynamic> item);
        Error DeleteAll(string url);
        (Dictionary<string, dynamic>?, Error) JsonToDictionary(string json);
    }
}
