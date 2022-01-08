namespace Gsuke.ApiPlatform.Errors
{
    public class Error
    {
        public string Message { get; set; }

        public Error()
        {
            Message = String.Empty;
        }

        public Error(string message)
        {
            Message = message;
        }

        public override string ToString()
        {
            return Message;
        }
    }
}
