namespace Gsuke.ApiPlatform.Errors
{
    public class JsonIdIsNotIncludedError : JsonError
    {
        public JsonIdIsNotIncludedError() : base()
        {
            Message += "JSONには値「id」を含む必要があります。";
        }
    }
}
