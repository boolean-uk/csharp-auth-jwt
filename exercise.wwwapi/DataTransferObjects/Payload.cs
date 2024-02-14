namespace exercise.wwwapi.DataTransferObjects
{
    public class Payload<T> where T : class
    {
        public T Data { get; set; }

        public Payload(T data)
        {
            Data = data;
        }
    }
}
