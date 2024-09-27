using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTOs.DinnerRequest
{
    public class OutDinnerDTO
    {
       
        public string Name { get; set; }

        public int Cost { get; set; }
    }
}
