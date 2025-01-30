namespace exercise.wwwapi.Exceptions
{
    public class EntityNotFoundException : ArgumentException
    {
        public EntityNotFoundException() { }

        public EntityNotFoundException(string? message) : base(message) { }

        public EntityNotFoundException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
