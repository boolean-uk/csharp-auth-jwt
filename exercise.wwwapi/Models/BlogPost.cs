namespace exercise.wwwapi.Models
{
    public class BlogPost
    {
        public int id { get; set; }
        public string text { get; set; }
        public int authorId { get; set; }
        public User Author { get; set; }

        public void Update(BlogPost post)
        {
            if (post.text != null)
            {
                text = post.text;
            }
        }
    }
}
