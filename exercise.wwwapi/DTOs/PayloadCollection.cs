namespace exercise.wwwapi.DTOs
{
    public class PayloadCollection<T> where T : class
    {
        public string Status { get; set; } = "Success";
        public IEnumerable<T> Data { get; set; }
    }
}
