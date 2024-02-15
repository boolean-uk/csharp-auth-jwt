namespace exercise.wwwapi.Models.InputModels
{
    public class EntryPut
    {
        /// <summary>
        /// The title of the Entry to be generated. Can be null!
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// The entry text of the Entry to be generated. Can be null!
        /// </summary>
        public string? Text { get; set; }
    }
}
