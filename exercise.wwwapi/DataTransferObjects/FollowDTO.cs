using exercise.wwwapi.DataModels;

namespace exercise.wwwapi.DataTransferObjects
{
    public class FollowDTO
    {
        public string User { get; set; }
        public string Follows { get; set; }

        public FollowDTO(Follow model) 
        {
            User = model.User.Username;
            Follows = model.OtherUser.Username;
        }
    }
}
