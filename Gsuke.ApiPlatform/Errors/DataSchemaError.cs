namespace Gsuke.ApiPlatform.Errors
{
    public class DataSchemaError : Error
    {
        public DataSchemaError()
        {
            Message = $"データスキーマの形式が不正です。";
        }
    }
}
