namespace exercise.wwwapi.Data
{
    public enum ResponseStatus
    {
        Success,
        Failure
    }
    public class ResponseObject<T>
    {
        public ResponseStatus Status { get; set; } = ResponseStatus.Success;
        public string ErrorMessage { get; set; } = string.Empty;
        public T? Data { get; set; }
    }
}
