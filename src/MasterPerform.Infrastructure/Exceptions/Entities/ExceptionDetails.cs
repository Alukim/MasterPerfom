namespace MasterPerform.Infrastructure.Exceptions.Entities
{
    public class ExceptionDetails
    {
        public string Code { get; }
        public string Message { get; }
        public ExceptionDetails(string code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}
