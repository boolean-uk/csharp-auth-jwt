namespace exercise.wwwapi.Models
{
    public class Payload<T>(T Data) where T : class
    {
        public string status { get; set; } = "Success";

        public T Data { get; set; } = Data;
    }
}
