namespace Essays.WebApi.Models
{
    public class Subject
    {
        public string SubjectId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public SubjectCategory Category { get; set; }
        public ICollection<EssaysAboutSubjects> EssaysAboutSubjects { get; set; }
    }
}
