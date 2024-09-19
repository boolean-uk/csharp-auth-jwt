namespace exercise.wwwapi.Models.DataTransferObjects
{
    public class Payload<T>
    {
        public string status { get; set; }
        public T data { get; set; }
    }
}
