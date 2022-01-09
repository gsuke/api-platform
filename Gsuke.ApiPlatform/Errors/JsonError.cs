namespace Gsuke.ApiPlatform.Errors
{
    public class JsonError : Error
    {
        public JsonError()
        {
            Message = $"JSONの書式が不正です。";
        }
    }
}
