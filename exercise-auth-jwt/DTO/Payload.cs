namespace exercise_auth_jwt.DTO
{
    public class Payload<T> where T : class
    {
        public T data { get; set; }
    }
}
