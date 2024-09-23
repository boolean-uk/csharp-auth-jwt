using System.ComponentModel;

namespace exercise.wwwapi.ViewModels
{
    public class BlogPUTModel
    {
        [DefaultValue("")]
        public string Text { get; set; }
        [DefaultValue(0)]
        public int AuthorId { get; set; }
    }
}
