using System;

namespace exercise.wwwapi.Payload;

public class Payload<T>
{
    public string Status {get;set;} = "Success";
    public T Data {get;set;}

}
