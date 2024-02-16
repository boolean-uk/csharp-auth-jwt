using exercise.wwwapi.Models;

namespace exercise.wwwapi.Data
{
    public class Seeder
    {
        private List<Blogpost> _posts = [];
        public Seeder() {

            _posts = new List<Blogpost>()
            {
                new Blogpost { Id = 1, Text = "This will contain alot of text" },
                new Blogpost { Id = 2, Text = "This will contain more of different text" }
            };
        }

        public List<Blogpost> Blogposts { get { return _posts; } }
    }
}
