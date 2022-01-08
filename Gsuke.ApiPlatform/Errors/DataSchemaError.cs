namespace Gsuke.ApiPlatform.Errors
{
    public class DataSchemaError : Error
    {
        public override string ToString()
        {
            return $"データスキーマの形式が不正です。";
        }
    }
}
