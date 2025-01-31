using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [NotMapped]
    public class Payload<T> where T : class
    {
        public string status { get; set; } = "success";
        public T data { get; set; }

        public Payload(T data)
        {
            this.status = "success";
            this.data = data;
        }
        public Payload() { }
    }
}
