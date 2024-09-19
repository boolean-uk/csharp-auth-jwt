using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DataTransferObjects
{
    [NotMapped]
    public class Payload<T> where T : class
    {
        public string Status { get; set; } = "success";
        public T Data { get; set; }
    }
}