using exercise.wwwapi.Enums;
using exercise.wwwapi.Models.PureModels;

namespace exercise.wwwapi.Data.Seed
{
    public class Seeder
    {
        Random rng = new Random(12345);

        private List<ApplicationUser> _users = new List<ApplicationUser>();

        public Seeder() 
        {

            for (int i = 1; i <= 10; i++) 
            {
                Role role = Role.User;
                if (i == 2 || i == 7) 
                {
                    role = Role.Administrator;
                }

                string Name = $"{NameLists.FirstNames[rng.Next(NameLists.FirstNames.Count)]} {NameLists.LastNames[rng.Next(NameLists.LastNames.Count)]}";

                ApplicationUser user = new ApplicationUser()
                {
                    UserName = Name,
                    Email = $"{Name.Replace(" ", "_")}@{NameLists.Domains[rng.Next(NameLists.Domains.Count)]}",
                    Role = role,
                };

                _users.Add(user);
            }
        }
        public List<ApplicationUser> Users { get { return _users; } }
    }
}
