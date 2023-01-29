using System.ComponentModel.DataAnnotations;

namespace Essays.WebApi.Models
{
    public class Essay
    {
        [Key]
        public string EssayId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string AuthorId { get; set; } = string.Empty;
        public Author? Author { get; set; }
        public ICollection<EssaysAboutSubjects>? EssaysAboutSubjects { get; set; }
    }
}