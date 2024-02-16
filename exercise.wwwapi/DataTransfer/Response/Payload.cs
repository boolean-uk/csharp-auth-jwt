namespace exercise.wwwapi.DataTransfer.Response
{
    public class Payload<T>(T data) where T : class
    {
        public string Status { get; set; } = "success";
        public T Data { get; set; } = data;
    }
}
