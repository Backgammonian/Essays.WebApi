namespace Essays.WebApi.Models
{
    public class EssaysAboutSubjects
    {
        public string? EssayId { get; set; }
        public string? SubjectId { get; set; }
        public Essay? Essay { get; set; }
        public Subject? Subject { get; set; }
    }
}
