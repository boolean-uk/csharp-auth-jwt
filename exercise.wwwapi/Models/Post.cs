namespace exercise.wwwapi.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
