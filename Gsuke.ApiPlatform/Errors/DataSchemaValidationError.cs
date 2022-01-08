namespace Gsuke.ApiPlatform.Errors
{
    public class DataSchemaValidationError : Error
    {
        public DataSchemaValidationError()
        {
            Message = $"データの形式が不正です。対応したデータスキーマを参照し、正しい値に修正してください。";
        }
    }
}
