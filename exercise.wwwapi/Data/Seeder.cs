//using exercise.wwwapi.Model;

//namespace exercise.wwwapi.Data
//{
//    public static class Seeder
//    {
//        public async static void SeedBlogPostsDatabase(this WebApplication app)
//        {
//            using (var db = new DatabaseContext()) {
//                if (!db.Authors.Any()) {
//                    db.Authors.Add(new Author
//                    {
//                        Id = "1",
//                        UserName = "Test",
                        
//                    });
//                    await db.SaveChangesAsync();    
//                }
//                if (!db.Posts.Any()) {
//                    db.Posts.Add(new BlogPost
//                    {
//                        Id = 1,
//                        Text = "Test123123123",

//                    });
//                    await db.SaveChangesAsync();
//                }
//            }
//        }
//    }
//}
