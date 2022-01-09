namespace Gsuke.ApiPlatform.Errors
{
    public class AlreadyExistsError : Error
    {
        // TODO: ResourceAlreadyExistsErrorとItemAlreadyExistsErrorに派生させたい
        public AlreadyExistsError(string message)
        {
            Message = $"{message}は既に存在しています。";
        }
    }
}
