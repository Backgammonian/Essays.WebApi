namespace Essays.WebApi.Models
{
    public class EssaysAboutSubjects
    {
        public string EssayId { get; set; } = string.Empty;
        public string SubjectId { get; set; } = string.Empty;
        public Essay Essay { get; set; }
        public Subject Subject { get; set; }
    }
}
