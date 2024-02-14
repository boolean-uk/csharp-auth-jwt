using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exercise.Application
{
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public bool Success = true;
        public string Message = "";
    }
}
