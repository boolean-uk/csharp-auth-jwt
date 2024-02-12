using System.ComponentModel.DataAnnotations;

namespace exercise.minimalapi.Models.Payloads
{
    public record PostPayload
    {
        [Required(ErrorMessage = "Text is required")]
        public required string Text { get; init; }
        public string CheckPayload()
        {
            if (string.IsNullOrWhiteSpace(Text)) { return "Title is required"; }
            return string.Empty;
        }
    }


}