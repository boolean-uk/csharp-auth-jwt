﻿using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTO.Response
{
    [NotMapped]
    public class Payload<T> where T : class
    {
        public string status { get; set; } = "success";
        public T data { get; set; }
    }
}
