namespace Gsuke.ApiPlatform.Errors
{
    public class AlreadyExistsError : Error
    {
        public override string ToString()
        {
            return $"{Message}は既に存在しています。";
        }
    }
}
