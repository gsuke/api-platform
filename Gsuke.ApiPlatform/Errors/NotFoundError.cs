namespace Gsuke.ApiPlatform.Errors
{
    public class NotFoundError : Error
    {
        public NotFoundError(string message)
        {
            Message = $"{message}は存在しません。";
        }
    }
}
