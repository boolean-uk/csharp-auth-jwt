using exercise.wwwapi.DataModels;

namespace exercise.wwwapi.DataTransferObjects
{
    public class UnfollowDTO
    {
        public string User { get; set; }
        public string Unfollows { get; set; }

        public UnfollowDTO(Follow model)
        {
            User = model.User.Username;
            Unfollows = model.OtherUser.Username;
        }
    }
}
