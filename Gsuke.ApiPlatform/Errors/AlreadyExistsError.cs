namespace Gsuke.ApiPlatform.Errors
{
    public class AlreadyExistsError : Error
    {
        public AlreadyExistsError(string message)
        {
            Message = $"{message}は既に存在しています。";
        }
    }
}
