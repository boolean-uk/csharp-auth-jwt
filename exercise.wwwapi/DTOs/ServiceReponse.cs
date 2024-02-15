namespace exercise.wwwapi.DTOs
{
    public class ServiceReponse<T> where T : class
    {
        public string status { get; set; }  
        public T data { get; set; }

    }
}
