namespace exercise.wwwapi.Models
{
    public class Author
    {
        public string Id { get; set; }

        public string userName { get; set; }
        public string Name { get; set; }

        public string passwordHash { get; set; }

        public ICollection<BlogPost> Blogs { get; set; }

        public List<AuthorFollower> Following { get; set; }

        public List<AuthorFollower> Followers { get; set; }




    }
}
