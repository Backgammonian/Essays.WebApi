namespace Essays.WebApi.Models
{
    public class EssayImage
    {
        public string ImageId { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public Essay Essay { get; set; }
    }
}
