using System.ComponentModel.DataAnnotations;

namespace Essays.WebApi.Models
{
    public class Subject
    {
        [Key]
        public string SubjectId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CategoryId { get; set; } = string.Empty;
        public SubjectCategory? Category { get; set; }
        public ICollection<EssaysAboutSubjects>? EssaysAboutSubjects { get; set; }
    }
}
