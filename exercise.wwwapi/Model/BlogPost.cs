namespace exercise.wwwapi.Model
{
    public class BlogPost
    {
        public int Id { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

       

    }
}
