namespace exercise.wwwapi.DTOs
{
    public class Payload<T> where T : class
    {
        public string Status { get; set; } = "Success";
        public T Data { get; set; }
    }
}
