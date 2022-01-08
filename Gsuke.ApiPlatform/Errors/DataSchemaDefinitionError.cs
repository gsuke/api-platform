namespace Gsuke.ApiPlatform.Errors
{
    public class DataSchemaDefinitionError : Error
    {
        public DataSchemaDefinitionError()
        {
            Message = $"データスキーマの形式が不正です。";
        }
    }
}
