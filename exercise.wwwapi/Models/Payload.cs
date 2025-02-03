using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [NotMapped]
    public class Payload<T>
        where T : class
    {
        public string status { get; set; } = "success";
        public required T data { get; set; }
    }
}
