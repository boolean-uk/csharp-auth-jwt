namespace exercise.wwwapi.DataTransferObjects
{
    public class Payload<T> where T : class
    {
        public T data { get; set; }
    }
}
