using System.ComponentModel.DataAnnotations;

namespace Essays.WebApi.Models
{
    public class SubjectCategory
    {
        [Key]
        public string SubjectCategoryId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public ICollection<Subject>? Subjects { get; set; }
    }
}