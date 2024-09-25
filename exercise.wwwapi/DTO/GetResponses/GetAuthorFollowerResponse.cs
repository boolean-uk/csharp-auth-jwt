namespace exercise.wwwapi.DTO.GetResponses
{
    public class GetAuthorFollowerResponse
    {
        public string Name { get; set; }
        public List<GetUserRelationsResponse> Following { get; set; }
    }
}
