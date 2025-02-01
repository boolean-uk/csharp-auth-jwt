using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Metadata;

namespace exercise.wwwapi.Payload
{

    public class Payload<T, Y>
    {
        public Payload() { }
        public string Status { get; set; }
        public T Data { get; set; }
    }

}
