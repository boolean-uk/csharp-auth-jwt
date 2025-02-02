namespace exercise.wwwapi.Models
{
    public class Blog
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Description { get; set; }

        public string Author { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
    }
}
