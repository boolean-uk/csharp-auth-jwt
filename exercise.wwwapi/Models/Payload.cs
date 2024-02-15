namespace exercise.wwwapi.Models
{
    /// <summary>
    /// Generic payload class to standardize outputs.
    /// </summary>
    /// <typeparam name="T"> Type of data that will be transmitted with the payload to the client.</typeparam>
    /// <param name="Data"> The data that is transfered. </param>
    public class Payload<T>(T Data) where T : class
    {
        public string status { get; set; } = "Success";

        public T Data { get; set; } = Data;
    }
}
