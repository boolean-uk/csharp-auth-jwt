namespace exercise.wwwapi.DataModels
{
    public class Result<T>
    {
        public bool Succeeded { get; }
        public T Data { get; }
        public string ErrorMessage { get; }

        public Result(bool succeeded, T data = default, string errorMessage = null)
        {
            Succeeded = succeeded;
            Data = data;
            ErrorMessage = errorMessage;
        }

        public static Result<T> Success(T data)
        {
            return new Result<T>(true, data);
        }

        public static Result<T> Failure(string errorMessage)
        {
            return new Result<T>(false, errorMessage: errorMessage);
        }
    }

}
