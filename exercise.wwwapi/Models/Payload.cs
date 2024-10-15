﻿namespace exercise.wwwapi.Models
{
    public class Payload<T> where T : class
    {
        public string Status { get; set; } = "success";
        public T Data { get; set; }
    }
}
