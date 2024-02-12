using System.ComponentModel.DataAnnotations;

namespace BlogApplication.Models
{
    public record BlogPostPostPayload
    {
        [Required(ErrorMessage = "Text is required")]
        public string Text { get; init; }

        public BlogPostPostPayload(string text)
        {
            Text = text;
        }
    }
}
