using exercise.wwwapi.DTOs.UserRelationStatus;
using exercise.wwwapi.Model;

namespace exercise.wwwapi.DTOs.Author
{
    public class GetAuthorFollowerDTO
    {
        public string Name { get; set; }
        public List<GetUserRelationDTO> Following { get; set; }

    }
}
