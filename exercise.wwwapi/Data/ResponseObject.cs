using exercise.wwwapi.Data.Enums;

namespace exercise.wwwapi.Data
{

    public class ResponseObject<T>
    {
        public ResponseStatus Status { get; set; } = ResponseStatus.Success;
        public string ErrorMessage { get; set; } = string.Empty;
        public T? Data { get; set; }
    }
}
