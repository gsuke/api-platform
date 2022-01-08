namespace Gsuke.ApiPlatform.Errors
{
    public class NotFoundError : Error
    {
        public override string ToString()
        {
            return $"{Message}は存在しません。";
        }
    }
}
