namespace Gsuke.ApiPlatform.Errors
{
    public class NoError : JsonError
    {
        public NoError()
        {
            Message = "エラーはありません。";
        }
    }
}
