namespace exercise.wwwapi.DTOs
{
    public class ServiceReponse<T> where T : class
    {
        public T data { get; set; }
        public string status { get; set; }  

    }
}
