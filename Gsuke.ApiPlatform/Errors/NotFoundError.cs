namespace Gsuke.ApiPlatform.Errors
{
    public class NotFoundError : Error
    {
        // TODO: ResourceNotFoundErrorとItemNotFoundErrorに派生させたい
        public NotFoundError(string message)
        {
            Message = $"{message}は存在しません。";
        }
    }
}
