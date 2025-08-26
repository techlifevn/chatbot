namespace Chatbot.Common.Result
{
    public class Errors
    {
        public string Description { get; set; }
    }

    public class ErrorResult<T> : Result<T>
    {
        public Errors[] ValidationErrors { get; set; }

        public ErrorResult()
        {
        }

        public ErrorResult(string message)
        {
            IsSuccessed = false;
            Message = message;
            ValidationErrors = null;
        }

        public ErrorResult(Errors[] validationErrors, string message)
        {
            IsSuccessed = false;
            ValidationErrors = validationErrors;
            Message = message;
        }
    }
}
