namespace Gsuke.ApiPlatform.Errors
{
    public class DataSchemaValidationError : Error
    {
        public DataSchemaValidationError()
        {
            Message = $"データの形式がデータスキーマに対応していません。";
        }

        public DataSchemaValidationError(string message) : this()
        {
            Message += message;
        }
    }
}
