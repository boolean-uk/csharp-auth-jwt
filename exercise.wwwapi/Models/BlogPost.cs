namespace exercise.wwwapi.Models
{
    public class BlogPost
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        public List<Comment> Comments { get; set; } = [];

    }
}
