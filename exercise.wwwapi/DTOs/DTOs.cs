using exercise.wwwapi.Models;


namespace exercise.wwwapi.DTOs
{
    public class DTOs
    {

        public class PostDTO
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Text { get; set; }
            public int AuthorId { get; set; }

            public PostDTO(Blogpost post)
            {
                Id = post.Id;
                Title = post.Title;
                Text = post.Text;
                AuthorId = post.AuthorId;
            }

        }

        public class PostResponseDTO
        {
            public int Id { get; set; }
            public string Title { get; set; }

            public string Text { get; set; }
            public int AuthorId { get; set; }

            //public AuthorDTO Author { get; set; }

            public PostResponseDTO(Blogpost post)
            {
                Id = post.Id;
                Title = post.Title;
                Text = post.Text;

                AuthorId = post.AuthorId;
            }

            public static List<PostResponseDTO> FromRepository(IEnumerable<Blogpost> posts)
            {
                var results = new List<PostResponseDTO>();
                foreach (var post in posts)
                {
                    results.Add(new PostResponseDTO(post));

                }
                return results;
            }
        }

    }
}
