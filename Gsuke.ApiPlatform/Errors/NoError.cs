namespace Gsuke.ApiPlatform.Errors
{
    public class NoError : Error
    {
        public NoError()
        {
            Message = "エラーはありません。";
        }
    }
}
