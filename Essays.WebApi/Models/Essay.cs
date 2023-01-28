namespace Essays.WebApi.Models
{
    public class Essay
    {
        public string EssayId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public Author Author { get; set; }
        public ICollection<EssayImage> Images { get; set; }
        public ICollection<EssaysAboutSubjects> EssaysAboutSubjects { get; set; }
    }
}