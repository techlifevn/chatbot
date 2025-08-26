namespace Chatbot.Common.Result
{
    public class SuccessResult<T> : Result<T>
    {
        public Errors[] ValidationErrors { get; set; }

        public SuccessResult(T resultObj, string message = "")
        {
            IsSuccessed = true;
            ResultObj = resultObj;
            Message = message;
            ValidationErrors = null;
        }

        public SuccessResult(string message)
        {
            IsSuccessed = true;
            Message = message;
        }

        public SuccessResult()
        {
            IsSuccessed = true;
        }
    }
}
